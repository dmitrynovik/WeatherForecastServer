using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenMeteo;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherForecastServer.Services
{
    public class WeatherService : IHostedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WeatherService> _logger;
        private RabbitTemplate template;
        private Timer timer;

        public WeatherService(IServiceProvider services, IHttpClientFactory httpClientFactory, ILogger<WeatherService> logger)
        {
            template = services.GetRabbitTemplate();
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(WeatherService)}");

            timer = new Timer(Sender, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            return Task.CompletedTask;
        }

        private void Sender(object state)
        {
           const double lat = -33.52;
           const double lon = 151.12;
           const string name = "Sydney, NSW, Australia";

            var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,windspeed_10m&hourly=temperature_2m,relativehumidity_2m,windspeed_10m";
            _logger.LogInformation("Producing a forecast for: " + name);

            var client = _httpClientFactory.CreateClient();        
            var forecast = client.GetFromJsonAsync<WeatherForecast>(url).GetAwaiter().GetResult();
            forecast.Name = name;
            template.ConvertAndSend("weather", "", System.Text.Json.JsonSerializer.Serialize(forecast));
        }
    }
}
