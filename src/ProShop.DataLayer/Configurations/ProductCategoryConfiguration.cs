using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {

        builder.ToTable("ProductCategory");
        builder.Ignore(c => c.Id);
        builder.Ignore(c => c.IsDeleted);


        builder.HasKey(c => new { c.ProductId, c.CategoryId });


        builder.HasOne(c => c.Product)
            .WithMany(c => c.productCategories)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

        builder.HasOne(c => c.Category)
            .WithMany(c => c.productCategories)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
    }
}
