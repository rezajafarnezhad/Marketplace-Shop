using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ParcelPostItemConfiguration : IEntityTypeConfiguration<ParcelPostItem>
{
    public void Configure(EntityTypeBuilder<ParcelPostItem> builder)
    {

        builder.ToTable("ParcelPostItems");
        builder.Ignore(c => c.Id);
        builder.Ignore(c => c.IsDeleted);


        builder.HasKey(c => new { c.ParcalPostId, c.ProductVariantId });


        builder.HasOne(c => c.ProductVariant)
            .WithMany(c => c.ParcelPostItems)
            .HasForeignKey(c => c.ProductVariantId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.ParcalPost)
            .WithMany(c => c.ParcelPostItems)
            .HasForeignKey(c => c.ParcalPostId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}