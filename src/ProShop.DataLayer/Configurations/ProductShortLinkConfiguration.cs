using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProductShortLinkConfiguration : IEntityTypeConfiguration<ProductShortLink>
{
    public void Configure(EntityTypeBuilder<ProductShortLink> builder)
    {

        builder.ToTable("ProductShortLink");
       
        builder.HasKey(c =>c.Id);
        builder.Property(c => c.Link).IsRequired().HasMaxLength(200);
        

        builder.HasOne(c => c.product)
            .WithOne(c => c.ProductShortLink)
            .HasForeignKey<Product>(c => c.ProductShortLinkId);

       
    }
}
