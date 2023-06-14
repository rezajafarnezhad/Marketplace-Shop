using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {

        builder.ToTable("Orders");
        builder.HasKey(c => c.Id);
        

        builder.HasIndex(c => (new { c.OrderNumber })).IsUnique();


        builder.HasOne(c => c.User)
            .WithMany(c => c.Orders)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        
        builder.HasOne(c => c.Address)
            .WithMany(c => c.Orders)
            .HasForeignKey(c => c.AddressId).OnDelete(DeleteBehavior.NoAction);

    }
}