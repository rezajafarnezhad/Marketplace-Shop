using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities.Identity;

namespace ProShop.DataLayer.Configurations
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.HasOne(c => c.Role)
                .WithMany(c => c.RoleClaims)
                .HasForeignKey(c => c.RoleId);

            builder.ToTable("RoleClaims");


        }
    }
}
