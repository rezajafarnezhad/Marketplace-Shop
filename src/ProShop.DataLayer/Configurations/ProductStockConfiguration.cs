using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(EntityTypeBuilder<ProductStock> builder)
    {

        builder.ToTable("ProductStock");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Count).IsRequired();
       


        builder.HasIndex(c => (new { c.ProductVariantId,c.ConsignmentId })).IsUnique();



        builder.HasOne(c => c.Consignment)
            .WithMany(c => c.ProductStocks)
            .HasForeignKey(c => c.ConsignmentId);
        
        builder.HasOne(c => c.ProductVariant)
            .WithMany(c => c.ProductStocks)
            .HasForeignKey(c => c.ProductVariantId)
            .OnDelete(DeleteBehavior.NoAction);

        
    }
}


