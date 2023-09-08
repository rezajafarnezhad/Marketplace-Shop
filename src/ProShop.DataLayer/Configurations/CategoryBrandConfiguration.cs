using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class CategoryBrandConfiguration : IEntityTypeConfiguration<CategoryBrand>
{
    public void Configure(EntityTypeBuilder<CategoryBrand> builder)
    {

        builder.ToTable("CategoryBrand");
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => (new { c.BrandId,c.CategoryId})).IsUnique();


        builder.HasOne(c => c.Brand)
            .WithMany(c => c.CategoryBrands)
            .HasForeignKey(c => c.BrandId);
        
        builder.HasOne(c => c.Category)
            .WithMany(c => c.CategoryBrands)
            .HasForeignKey(c => c.CategoryId);

    }
}