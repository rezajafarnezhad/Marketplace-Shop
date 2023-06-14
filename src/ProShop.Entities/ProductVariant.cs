namespace ProShop.Entities;

public class ProductVariant: EntityBase, IAuditableEntity
{

    public long ProductId { get; set; }
    public long SellerId { get; set; }
    public long? VariantId { get; set; }
    public long GaranteeId { get; set; }
    public int Price { get; set; }
    public int VariantCode { get; set; }
    public int Count { get; set; }
    public short MaxCountInCart { get; set; }
    public int? OffPrice { get; set; }
    public byte? offPercentage { get; set; }
    public int FinalPrice => OffPrice ?? Price;
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public Seller Seller { get; set; }
    public Product Product { get; set; }
    public Variant Variant { get; set; }
    public Garantee Garantee { get; set; }
    public ICollection<ConsignmentItem> ConsignmentItems { get; set; }
    public ICollection<ProductStock> ProductStocks { get; set; }
    public ICollection<Cart> Carts { get; set; }
    public ICollection<ParcelPostItem> ParcelPostItems { get; set; }


}
