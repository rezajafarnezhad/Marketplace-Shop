using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class GaranteeConfiguration : IEntityTypeConfiguration<Garantee>
{
    public void Configure(EntityTypeBuilder<Garantee> builder)
    {

        builder.ToTable("Garantee");
       
        builder.HasKey(c =>c.Id);
        builder.Ignore(c => c.FullTitle);

        builder.Property(c => c.Title).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Picture).IsRequired().HasMaxLength(60);
    }
}