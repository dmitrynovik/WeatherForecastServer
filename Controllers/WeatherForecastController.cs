using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenMeteo;

namespace WeatherForecastServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IHttpClientFactory httpClientFactory, ILogger<WeatherForecastController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<WeatherForecast> Get(double lat = -33.52, double lon = 151.12)
        {
            //_weatherProcessor.Handle(new Model.Coordinate { Latitude = lat, Longitude = lon });
            
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,windspeed_10m&hourly=temperature_2m,relativehumidity_2m,windspeed_10m";
            _logger.LogInformation("Getting forecast");

            var client = _httpClientFactory.CreateClient();        
            var response = await client.GetFromJsonAsync<WeatherForecast>(url);
            return response;
        }
    }
}
