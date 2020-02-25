using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RESTfulAPISample.Core.Entity;

namespace RESTfulAPISample.Infrastructure
{
    public class RESTfulAPISampleContextSeed
    {
        public static async Task SeedAsync(RESTfulAPISampleContext context, ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                // TODO: Only run this if using a real database
                // myContext.Database.Migrate();

                if (!context.Products.Any())
                {
                    var now = DateTime.Now;
                    context.Products.AddRange(
                        new Product
                        {
                            Name = "A Learning ASP.NET Core",
                            Description = "C best-selling book covering the fundamentals of ASP.NET Core",
                            IsOnSale = true,
                            CreateTime = now.AddDays(1),
                        },
                        new Product
                        {
                            Name = "D Learning EF Core",
                            Description = "A best-selling book covering the fundamentals of C#",
                            IsOnSale = true,
                            CreateTime = now,
                        },
                        new Product
                        {
                            Name = "D Learning EF Core",
                            Description = "B best-selling book covering the fundamentals of .NET Standard",
                            CreateTime = now.AddDays(2),
                        },
                        new Product
                        {
                            Name = "C Learning .NET Core",
                            Description = "D best-selling book covering the fundamentals of .NET Core",
                            CreateTime = now.AddDays(13),
                        },
                        new Product
                        {
                            Name = "Learning C#",
                            Description = "A best-selling book covering the fundamentals of C#",
                            CreateTime = now,
                        });
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<RESTfulAPISampleContextSeed>();
                    logger.LogError(ex.Message);
                    await SeedAsync(context, loggerFactory, retryForAvailability);
                }
            }
        }
    }

}