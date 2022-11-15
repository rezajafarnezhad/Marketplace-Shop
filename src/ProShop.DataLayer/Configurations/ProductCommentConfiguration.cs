using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProductCommentConfiguration : IEntityTypeConfiguration<ProductComment>
{
    public void Configure(EntityTypeBuilder<ProductComment> builder)
    {

        builder.ToTable("ProductComment");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CommentTitle).HasMaxLength(200);
        builder.Property(c => c.CommentText).HasMaxLength(1000);
        builder.Property(c => c.NegativeItems).HasMaxLength(1000);
        builder.Property(c => c.PositiveItems).HasMaxLength(1000);
       

        builder.HasIndex(c => (new { c.UserId , c.ProductId })).IsUnique();
        builder.HasIndex(c => (new { c.SellerId , c.ProductId })).IsUnique();

       
    }
}


