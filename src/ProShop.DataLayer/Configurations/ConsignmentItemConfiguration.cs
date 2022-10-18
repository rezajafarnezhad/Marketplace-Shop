using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ConsignmentItemConfiguration : IEntityTypeConfiguration<ConsignmentItem>
{
    public void Configure(EntityTypeBuilder<ConsignmentItem> builder)
    {

        builder.ToTable("ConsignmentItem");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Count).IsRequired();
        builder.Property(c => c.Barcode).HasMaxLength(40).IsRequired();


        builder.HasIndex(c => (new { c.ProductVariantId,c.ConsignmentId })).IsUnique();



        builder.HasOne(c => c.Consignment)
            .WithMany(c => c.ConsignmentItems)
            .HasForeignKey(c => c.ConsignmentId);
        
        builder.HasOne(c => c.ProductVariant)
            .WithMany(c => c.ConsignmentItems)
            .HasForeignKey(c => c.ProductVariantId)
            .OnDelete(DeleteBehavior.NoAction);

        
    }
}


