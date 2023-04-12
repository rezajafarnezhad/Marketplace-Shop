using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {

        builder.ToTable("Cart");
        builder.Ignore(c => c.Id);
        builder.Ignore(c => c.IsDeleted);



        builder.HasKey(c => new { c.UserId, c.ProductVaraintId });


        builder.HasOne(c => c.User)
            .WithMany(c => c.Carts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

        builder.HasOne(c => c.ProductVariant)
            .WithMany(c => c.Carts)
            .HasForeignKey(c => c.ProductVaraintId)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

    }
}

