using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RESTfulAPITemplate.Core.Entity;

namespace RESTfulAPITemplate.Infrastructure
{
    public class ScetiaIndentityContext : DbContext
    {
        public ScetiaIndentityContext(DbContextOptions<ScetiaIndentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AspnetMembership>(e => e.ToTable("aspnet_Membership").HasKey(e => e.UserId));
            modelBuilder.Entity<AspnetUsers>(e => e.ToTable("aspnet_users").HasKey(e => e.UserId));
        }

        public virtual DbSet<AspnetMembership> AspnetMembership { get; set; }
        public virtual DbSet<AspnetUsers> AspnetUsers { get; set; }
    }
}
