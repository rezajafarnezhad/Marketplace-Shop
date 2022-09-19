namespace ProShop.Entities;

public class Variant : EntityBase, IAuditableEntity
{
    public string Value { get; set; }
    public bool IsColor { get; set; }
    public string ColorCode { get; set; }
    public bool IsConfirmed { get; set; }


    public ICollection<CategoryVarieant> categoryVarieants { get; set; }
}
