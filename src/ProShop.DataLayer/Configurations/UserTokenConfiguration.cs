using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities.Identity;

namespace ProShop.DataLayer.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {

        builder.HasOne(c => c.User)
            .WithMany(c => c.UserTokens)
            .HasForeignKey(c => c.UserId);
        builder.ToTable("UserTokens");
    }
}