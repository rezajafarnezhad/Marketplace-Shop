using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {

        builder.ToTable("Brands");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.TitleFa).HasMaxLength(150).IsRequired();
        builder.Property(c => c.TitleEn).HasMaxLength(150).IsRequired();
        builder.Property(c => c.Description).IsRequired();
        builder.Property(c => c.LogoPicture).IsRequired().HasMaxLength(50);
        builder.Property(c => c.JudiciaryLink).HasMaxLength(200);
        builder.Property(c => c.BrandRegistrationPicture).HasMaxLength(200);
        builder.Property(c => c.BrandLinkEn).HasMaxLength(200);

        builder.HasIndex(c => (new { c.TitleEn })).IsUnique();
        builder.HasIndex(c => (new { c.TitleFa })).IsUnique();

        builder.HasOne(c => c.Seller)
            .WithMany(c => c.Brands)
            .HasForeignKey(c => c.SellerId);

        builder.HasMany(c => c.Products)
            .WithOne(c => c.Brand)
            .HasForeignKey(c => c.BrandId);
    }
}


