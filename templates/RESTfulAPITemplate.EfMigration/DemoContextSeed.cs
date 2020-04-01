using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RESTfulAPITemplate.Core.Entity;
using Microsoft.EntityFrameworkCore;


namespace RESTfulAPITemplate.EfMigration.Demo
{
    public class DemoContextSeed
    {

        private ILogger<DemoContextSeed> _logger;
        private readonly DemoContext _context;

        public DemoContextSeed(ILogger<DemoContextSeed> logger, DemoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task SeedAsync(int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                _context.Database.Migrate();

                if (!_context.Products.Any())
                {
                    var now = DateTime.Now;
                    _context.Products.AddRange(
                        new Product
                        {
                            Name = "A Learning ASP.NET Core",
                            Description = "C best-selling book covering the fundamentals of ASP.NET Core",
                            IsOnSale = true,
                            CreateTime = now.AddDays(1),
                        },
                        new Product
                        {
                            Name = "D Learning EF Core 2",
                            Description = "A best-selling book covering the fundamentals of C#",
                            IsOnSale = true,
                            CreateTime = now,
                        },
                        new Product
                        {
                            Name = "D Learning EF Core 3",
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
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Seed data created.");
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    _logger.LogError(ex.Message);
                    await SeedAsync(retryForAvailability);
                }
            }
        }
    }
}