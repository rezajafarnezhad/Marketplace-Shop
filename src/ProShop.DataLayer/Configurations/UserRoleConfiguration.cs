using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities.Identity;

namespace ProShop.DataLayer.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {

        builder.HasOne(c => c.Role)
            .WithMany(c => c.UserRoles)
            .HasForeignKey(c => c.RoleId);


        builder.HasOne(c => c.User)
            .WithMany(c => c.UserRoles)
            .HasForeignKey(c => c.UserId);

        builder.ToTable("UserRoles");
    }

}