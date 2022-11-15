using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class UserProductFavoriteConfiguration : IEntityTypeConfiguration<UserProductFavorite>
{
    public void Configure(EntityTypeBuilder<UserProductFavorite> builder)
    {

        builder.ToTable("UserProductFavorite");
       
        builder.Ignore(c => c.Id);
        builder.Ignore(c => c.IsDeleted);

        builder.HasKey(c => (new { c.UserId,c.ProductId}));


        builder.HasOne(c => c.Product)
            .WithMany(c => c.UserProductFavorites)
            .HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(c => c.User)
            .WithMany(c => c.UserProductFavorites)
            .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction); ;

    }
}

