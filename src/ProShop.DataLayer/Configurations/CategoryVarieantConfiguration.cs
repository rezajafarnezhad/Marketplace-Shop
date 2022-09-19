using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class CategoryVarieantConfiguration : IEntityTypeConfiguration<CategoryVarieant>
{
    public void Configure(EntityTypeBuilder<CategoryVarieant> builder)
    {

        builder.ToTable("CategoryVarieant");
        builder.Ignore(c => c.Id);
        builder.Ignore(c => c.IsDeleted);



        builder.HasKey(c => new { c.VariantId, c.CategoryId });


        builder.HasOne(c => c.Variant)
            .WithMany(c => c.categoryVarieants)
            .HasForeignKey(c => c.VariantId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

        builder.HasOne(c => c.Category)
            .WithMany(c => c.categoryVarieants)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
    }
}
