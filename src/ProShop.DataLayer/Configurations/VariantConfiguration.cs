using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class VariantConfiguration : IEntityTypeConfiguration<Variant>
{
    public void Configure(EntityTypeBuilder<Variant> builder)
    {

        builder.ToTable("Variant");
       
        builder.HasKey(c =>c.Id);
        builder.Property(c => c.Value).IsRequired().HasMaxLength(150);
        builder.Property(c => c.ColorCode).HasMaxLength(7);

        builder.HasMany(c => c.categoryVarieants)
            .WithOne(c => c.Variant)
            .HasForeignKey(c => c.VariantId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

       
    }
}
