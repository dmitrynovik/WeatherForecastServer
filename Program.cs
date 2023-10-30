using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Hosting;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Host;
using WeatherForecastServer.Services;

namespace WeatherForecastServer
{
    public class Program
    {
         static void Main(string[] args)
        {
            RabbitMQHost
              .CreateDefaultBuilder()
              .ConfigureServices((context, services) =>
              {
                  services.AddLogging(builder =>
                  {
                      builder.AddDebug();
                      builder.AddConsole();
                  });

                  // Add Rabbit template
                  services.AddRabbitMQConnection(context.Configuration);
                  //services.AddRabbitServices();
                  //services.AddRabbitTemplate();
                  services.AddHostedService<WeatherService>();

                  services.AddHttpClient();
              })
              .UseCloudHosting(55006)
              .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
              .Build()
              .Run();
        }

        // public static IHostBuilder CreateHostBuilder(string[] args) =>
        //     StreamHost.CreateDefaultBuilder(args);
    }
}
