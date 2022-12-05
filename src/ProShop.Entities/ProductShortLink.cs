namespace ProShop.Entities;

public class ProductShortLink : EntityBase, IAuditableEntity
{
    public string Link { get; set; }
    public bool IsUsed { get; set; }
    public Product product { get; set; }
}
