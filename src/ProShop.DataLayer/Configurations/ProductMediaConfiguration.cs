using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProductMediaConfiguration : IEntityTypeConfiguration<ProductMedia>
{
    public void Configure(EntityTypeBuilder<ProductMedia> builder)
    {

        builder.ToTable("ProductMedia");
        builder.HasKey(c =>c.Id);
        builder.Property(c => c.FileName).IsRequired().HasMaxLength(200);


        builder.HasOne(c => c.Product)
            .WithMany(c => c.ProductMedia)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
    }
}

