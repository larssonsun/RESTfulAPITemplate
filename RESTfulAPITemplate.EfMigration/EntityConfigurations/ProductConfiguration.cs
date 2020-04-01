using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RESTfulAPITemplate.Core.Entity;

namespace RESTfulAPITemplate.EfMigration.Demo.EntityConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> e)
        {
            e.ToTable("My_Product").HasKey(t => t.Id);
            e.HasIndex(t => t.Name).IsUnique();
            e.Property(t => t.Name).HasMaxLength(64).IsUnicode().IsRequired();
            e.Property(t => t.Description).HasMaxLength(256).IsUnicode().IsRequired();
            e.Property(t => t.CreateTime).HasColumnType("datetime").IsRequired(); // larsson：别忘记加入 Microsoft.EntityFrameworkCore.Relational, ctm的..
        }
    }
}