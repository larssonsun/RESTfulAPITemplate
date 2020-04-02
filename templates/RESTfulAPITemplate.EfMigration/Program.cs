using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RESTfulAPITemplate.EfMigration.Demo;
using Serilog;

namespace RESTfulAPITemplate.EfMigration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

            try
            {
                Console.Title = "RESTfulAPITemplate";

                Log.Information("Starting web host");
                var host = CreateHostBuilder(args).Build();

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
                        return;
                    }
                }

                // host.Run();
                Log.Information("Data migration completed.");
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
                        logger.ReadFrom.Configuration(context.Configuration);
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
