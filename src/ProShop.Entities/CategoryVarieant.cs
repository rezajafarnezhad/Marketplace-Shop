namespace ProShop.Entities;

public class CategoryVarieant : EntityBase, IAuditableEntity
{

    public long VariantId { get; set; }
    public long CategoryId { get; set; }



    public Category Category { get; set; }
    public Variant  Variant{ get; set; }
}