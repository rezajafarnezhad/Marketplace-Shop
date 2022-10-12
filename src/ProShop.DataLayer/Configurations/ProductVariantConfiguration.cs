using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {

        builder.ToTable("ProductVariant");
        builder.HasKey(c => c.Id);
        

        builder.HasIndex(c => (new { c.SellerId,c.ProductId,c.VariantId })).IsUnique();
        builder.HasIndex(c => (new { c.VariantCode })).IsUnique();

        builder.HasOne(c => c.Product)
            .WithMany(c => c.ProductVariants)
            .HasForeignKey(c => c.ProductId);


        builder.HasOne(c => c.Seller)
            .WithMany(c => c.ProductVariants)
            .HasForeignKey(c => c.SellerId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasMany(c => c.ConsignmentItems)
            .WithOne(c => c.ProductVariant)
            .HasForeignKey(c => c.ProductVariantId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}

