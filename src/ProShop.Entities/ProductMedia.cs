namespace ProShop.Entities;

public class ProductMedia : EntityBase, IAuditableEntity
{

    public string FileName { get; set; }
    public bool IsVideo { get; set; }
    public long ProductId { get; set; }

    public Product Product { get; set; }
}
