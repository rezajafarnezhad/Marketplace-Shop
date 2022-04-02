using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities.Identity;

namespace ProShop.DataLayer.Configurations;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {

        builder.HasOne(c => c.User)
            .WithMany(c => c.UserLogins)
            .HasForeignKey(c => c.UserId);
        builder.ToTable("UserLogins");
    }
}