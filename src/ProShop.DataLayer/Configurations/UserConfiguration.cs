using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities.Identity;

namespace ProShop.DataLayer.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.HasKey(x => x.Id);
        builder.Property(c => c.LastName).IsRequired(false).HasMaxLength(100);
        builder.Property(c => c.FirstName).IsRequired(false).HasMaxLength(100);
        builder.Property(c => c.NationalCode).IsRequired(false).HasMaxLength(11);
        builder.Property(c => c.Avatar).IsRequired().HasMaxLength(300);

        builder.ToTable("Users");
    }
}