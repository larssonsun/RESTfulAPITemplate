using Microsoft.EntityFrameworkCore;
using RESTfulAPISample.Core.Entity;
using RESTfulAPISample.EfMigration.Demo.EntityConfiguration;

namespace RESTfulAPISample.EfMigration.Demo
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions<DemoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }

        public DbSet<Product> Products { get; set; }
    }
}