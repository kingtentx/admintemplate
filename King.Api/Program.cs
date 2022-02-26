using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace King.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithProperty("Application", "Api")
                .Enrich.FromLogContext()
                .WriteTo.File($"Logs/{DateTime.Now:yyyyMMdd}.txt")
                //.WriteTo.Elasticsearch(
                //    new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Url"]))
                //    {
                //        AutoRegisterTemplate = true,
                //        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                //        IndexFormat = "xdl-log-{0:yyyy.MM}"
                //    })
                .WriteTo.Console()
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
