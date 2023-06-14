using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ParcalPostConfiguration : IEntityTypeConfiguration<ParcalPost>
{
    public void Configure(EntityTypeBuilder<ParcalPost> builder)
    {

        builder.ToTable("ParcalPosts");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.PostTrackingCode).HasMaxLength(30);

        builder.HasIndex(c => (new { c.PostTrackingCode })).IsUnique();
       

        builder.HasOne(c => c.Order)
            .WithMany(c => c.ParcalPosts)
            .HasForeignKey(c => c.OrderId); 
        
       

    }
}