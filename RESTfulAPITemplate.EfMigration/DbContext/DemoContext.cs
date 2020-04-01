using Microsoft.EntityFrameworkCore;
using RESTfulAPITemplate.Core.Entity;
using RESTfulAPITemplate.EfMigration.Demo.EntityConfiguration;

namespace RESTfulAPITemplate.EfMigration.Demo
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