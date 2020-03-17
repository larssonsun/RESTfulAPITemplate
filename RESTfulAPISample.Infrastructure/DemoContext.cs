using Microsoft.EntityFrameworkCore;
using RESTfulAPISample.Core.Entity;

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

            modelBuilder.Entity<Product>(e => e.ToTable("My_Product"));
#endif
        }

        public DbSet<Product> Products { get; set; }
    }
}