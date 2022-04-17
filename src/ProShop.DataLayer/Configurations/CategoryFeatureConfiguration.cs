using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class CategoryFeatureConfiguration : IEntityTypeConfiguration<CategoryFeature>
{
    public void Configure(EntityTypeBuilder<CategoryFeature> builder)
    {

        builder.ToTable("CategoryFeature");
        builder.Ignore(c => c.Id);
        builder.HasKey(c => new{c.FeatureId,c.CategoryId});
        
        builder.HasOne(c => c.Category)
            .WithMany(c => c.CategoryFeatures)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        builder.HasOne(c => c.Feature)
            .WithMany(c => c.CategoryFeatures)
            .HasForeignKey(c => c.FeatureId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}