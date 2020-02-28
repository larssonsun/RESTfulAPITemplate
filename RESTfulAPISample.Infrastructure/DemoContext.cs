using Microsoft.EntityFrameworkCore;
using RESTfulAPISample.Core.Entity;

#if (!DBINMEMORY)

using RESTfulAPISample.Infrastructure.EntityConfiguration;

#endif

namespace RESTfulAPISample.Infrastructure
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

#if (!DBINMEMORY)

            modelBuilder.ApplyConfiguration(new ProductConfiguration());

#endif
        }

        public DbSet<Product> Products { get; set; }
    }
}