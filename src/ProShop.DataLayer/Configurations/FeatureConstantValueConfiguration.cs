using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class FeatureConstantValueConfiguration : IEntityTypeConfiguration<FeatureConstantValue>
{
    public void Configure(EntityTypeBuilder<FeatureConstantValue> builder)
    {

        builder.ToTable("FeatureConstantValue");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Value).HasMaxLength(200).IsRequired();


        builder.HasOne(c => c.Category)
            .WithMany(c => c.FeatureConstantValues)
            .HasForeignKey(c => c.CategoryId);

        builder.HasOne(c => c.Feature)
            .WithMany(c => c.FeatureConstantValues)
            .HasForeignKey(c => c.FeatureId);
    }
}