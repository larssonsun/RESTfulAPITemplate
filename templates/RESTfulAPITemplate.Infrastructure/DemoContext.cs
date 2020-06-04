using Microsoft.EntityFrameworkCore;
using RESTfulAPITemplate.Core.Entity;

namespace RESTfulAPITemplate.Infrastructure
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

            modelBuilder.Entity<Product>(e => e.ToTable("My_Product").HasKey(t => t.Id));

#endif
        }

        public DbSet<Product> Products { get; set; }
    }
}