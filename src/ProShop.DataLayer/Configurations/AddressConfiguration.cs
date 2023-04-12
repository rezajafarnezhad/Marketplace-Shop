using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {

        builder.ToTable("Address");
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.City)
           .WithMany(c => c.AddressCities)
           .HasForeignKey(c => c.CityId);

        builder.HasOne(c => c.Province)
            .WithMany(c => c.AddressProvinces)
            .HasForeignKey(c => c.ProvinceId);
    }
}