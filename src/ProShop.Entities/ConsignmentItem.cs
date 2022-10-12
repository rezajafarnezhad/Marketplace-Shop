namespace ProShop.Entities;

public class ConsignmentItem : EntityBase, IAuditableEntity
{
    public long ProductVariantId { get; set; }
    public long ConsignmentId { get; set; }
    public int Count { get; set; }


    public ProductVariant ProductVariant { get; set; }
    public Consignment  Consignment { get; set; }
}