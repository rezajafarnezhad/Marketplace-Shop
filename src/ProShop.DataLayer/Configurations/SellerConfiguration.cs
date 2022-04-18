using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class SellerConfiguration : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> builder)
    {

        builder.ToTable("Sellers");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.IdCartPicture).IsRequired().HasMaxLength(200);
        builder.Property(c => c.ShabaNumber).IsRequired().HasMaxLength(30);
        builder.Property(c => c.Telephone).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Address).IsRequired().HasMaxLength(300);
        builder.Property(c => c.PostalCode).IsRequired().HasMaxLength(10);
        builder.Property(c => c.ShopName).IsRequired().HasMaxLength(300);
        builder.Property(c => c.AboutSeller).IsRequired(false);
        builder.Property(c => c.CompanyName).IsRequired(false).HasMaxLength(200);
        builder.Property(c => c.RegisterNumber).IsRequired(false).HasMaxLength(100);
        builder.Property(c => c.EconomicCode).IsRequired(false).HasMaxLength(12);
        builder.Property(c => c.SignatureOwners).IsRequired(false).HasMaxLength(300);
        builder.Property(c => c.NationalId).IsRequired(false).HasMaxLength(30);
        builder.Property(c => c.SignatureOwners).IsRequired(false).HasMaxLength(300);
        builder.Property(c => c.Logo).IsRequired(false).HasMaxLength(200);
        builder.Property(c => c.Website).IsRequired(false).HasMaxLength(40);
        builder.Property(c => c.Website).IsRequired(false).HasMaxLength(150);
        builder.Property(c => c.Location).IsRequired(false).HasMaxLength(200);


        builder.HasIndex(c => (new { c.ShopName })).IsUnique();
        builder.HasIndex(c => (new { c.ShabaNumber })).IsUnique();
        builder.HasIndex(c => (new { c.SellerCode })).IsUnique();


        builder.HasOne(c => c.User)
            .WithOne(c => c.Seller)
            .HasForeignKey<Seller>(c => c.UserId);


        builder.HasOne(c => c.City)
            .WithMany(c => c.Cities)
            .HasForeignKey(c => c.CityId);
        
        builder.HasOne(c => c.Province)
            .WithMany(c => c.Provinces)
            .HasForeignKey(c => c.ProvinceId);
    }
}