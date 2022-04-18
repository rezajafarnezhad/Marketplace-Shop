using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProvinceAndCityConfiguration : IEntityTypeConfiguration<ProvinceAndCity>
{
    public void Configure(EntityTypeBuilder<ProvinceAndCity> builder)
    {
        builder.ToTable("ProvincesAndCities");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title).IsRequired().HasMaxLength(100);


        builder.HasMany(x => x.Provinces)
            .WithOne(x => x.Province)
            .HasForeignKey(x => x.ProvinceId);

        builder.HasMany(x => x.Cities)
            .WithOne(x => x.City)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}