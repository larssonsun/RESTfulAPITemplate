using Microsoft.EntityFrameworkCore;
using RESTfulAPISample.Core.DomainModel;

#if (!DBINMEMORY)

using RESTfulAPISample.Infrastructure.EntityConfiguration;

#endif

namespace RESTfulAPISample.Infrastructure
{
    public class RESTfulAPISampleContext : DbContext
    {
        public RESTfulAPISampleContext(DbContextOptions<RESTfulAPISampleContext> options)
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