using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(300); 
        builder.Property(c => c.Description).HasMaxLength(2000);
        builder.Property(c => c.Picture).HasMaxLength(300);

        
        builder.HasIndex(c => (new { c.Slug,c.Title })).IsUnique();
       
        
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.ChildCategory)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}