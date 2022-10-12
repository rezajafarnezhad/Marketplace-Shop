namespace ProShop.Entities;

public class ProductVariant: EntityBase, IAuditableEntity
{

    public long ProductId { get; set; }
    public long SellerId { get; set; }
    public long VariantId { get; set; }
    public long GaranteeId { get; set; }
    public int Price { get; set; }
    public int VariantCode { get; set; }

    public Seller Seller { get; set; }
    public Product Product { get; set; }
    public Variant Variant { get; set; }
    public Garantee Garantee { get; set; }
    public ICollection<ConsignmentItem> ConsignmentItems { get; set; }


}
