using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProductFeatureConfiguration : IEntityTypeConfiguration<ProductFeature>
{
    public void Configure(EntityTypeBuilder<ProductFeature> builder)
    {

        builder.ToTable("ProductFeature");
        builder.Ignore(c => c.Id);
        builder.Ignore(c => c.IsDeleted);


        builder.HasKey(c => new { c.ProductId, c.FeatureId });


        builder.HasOne(c => c.Product)
            .WithMany(c => c.ProductFeatures)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

        builder.HasOne(c => c.Feature)
            .WithMany(c => c.ProductFeatures)
            .HasForeignKey(c => c.FeatureId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
    }
}