﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {

        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(300); 
        builder.Property(c => c.Description).IsRequired(false).HasMaxLength(2000);
        builder.Property(c => c.Picture).IsRequired(false).HasMaxLength(300);
        builder.Property(c => c.ProductPageGuid).IsRequired(false).HasMaxLength(1000);
        

        
        builder.HasIndex(c => (new { c.Title})).IsUnique();
        builder.HasIndex(c => (new { c.Slug})).IsUnique();
       
        
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.ChildCategory)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}