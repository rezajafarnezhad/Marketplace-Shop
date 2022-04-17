using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {

        builder.ToTable("Features");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title).HasMaxLength(150).IsRequired();
        
        builder.HasIndex(c => (new { c.Title})).IsUnique();
    }
}