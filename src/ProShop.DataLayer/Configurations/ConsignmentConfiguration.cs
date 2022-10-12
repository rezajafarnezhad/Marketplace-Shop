using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ConsignmentConfiguration : IEntityTypeConfiguration<Consignment>
{
    public void Configure(EntityTypeBuilder<Consignment> builder)
    {

        builder.ToTable("Consignment");
        builder.HasKey(c => c.Id);
       

        builder.HasOne(c => c.Seller)
            .WithMany(c => c.Consignments)
            .HasForeignKey(c => c.sellerId);

        
    }
}


