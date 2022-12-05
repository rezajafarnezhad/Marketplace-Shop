using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {

        builder.ToTable("Product");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.PersianTitle).HasMaxLength(200).IsRequired();
        builder.Property(c => c.EnglishTitle).HasMaxLength(200);
        builder.Property(c => c.PackLength).IsRequired();
        builder.Property(c => c.PackWeight).IsRequired();
        builder.Property(c => c.Packwidth).IsRequired();
        builder.Property(c => c.PackLength).IsRequired();
        builder.Property(c => c.Slug).HasMaxLength(200).IsRequired();
        builder.Property(c => c.RejectReason);
        builder.HasIndex(c => (new { c.ProductCode })).IsUnique();


        builder.HasMany(c => c.ProductMedia)
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId);

        builder.HasMany(c => c.productCategories)
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId);

        builder.HasMany(c => c.ProductFeatures)
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId);
        
        
        builder.HasOne(c => c.Brand)
            .WithMany(c => c.Products)
            .HasForeignKey(c => c.BrandId);


         builder.HasOne(c => c.Seller)
            .WithMany(c => c.Products)
            .HasForeignKey(c => c.SelerId);
        
        builder.HasOne(c => c.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(c => c.MainCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.ProductShortLink)
           .WithOne(c => c.product)
           .HasForeignKey<Product>(c => c.ProductShortLinkId);

    }
}