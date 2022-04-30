using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class CategoryBrandConfiguration : IEntityTypeConfiguration<CategoryBrand>
{
    public void Configure(EntityTypeBuilder<CategoryBrand> builder)
    {

        builder.ToTable("CategoryBrand");
        builder.Ignore(c => c.Id);
        builder.Ignore(c => c.IsDeleted);
    

        builder.HasKey(c => new{c.BrandId,c.CategoryId});


        builder.HasOne(c => c.Brand)
            .WithMany(c => c.CategoryBrands)
            .HasForeignKey(c => c.BrandId)
            .OnDelete(deleteBehavior:DeleteBehavior.Restrict);
        
        builder.HasOne(c => c.Category)
            .WithMany(c => c.CategoryBrands)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(deleteBehavior:DeleteBehavior.Restrict);
    }
}