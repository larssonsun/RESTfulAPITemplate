using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RESTfulAPITemplate.Infrastructure;
using Serilog;
using Serilog.Events;

namespace RESTfulAPITemplate.Api
{
    public class Program
    {
        private static readonly bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile(isDevelopment ? "appsettings.Development.json" : "appsettings.json")
            .Build();

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

            try
            {
                Console.Title = "RESTfulAPITemplate";

                Log.Information("Starting web host");
                var host = CreateHostBuilder(args).Build();

#if (DBINMEMORY)

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var seed = services.GetRequiredService<DemoContextSeed>();
                        seed.SeedAsync().Wait();

                        // Add your own data seed here..
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "An error occurred seeding the DB.");
                    }
                }

#endif

                host.Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Third-party log providers
                    webBuilder.UseSerilog((context, logger) =>
                    {
                        var configuration = new ConfigurationBuilder()
                        .AddJsonFile(isDevelopment ? "appsettings.Development.json" : "appsettings.json")
                        .Build();

                        logger.ReadFrom.Configuration(configuration);
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
