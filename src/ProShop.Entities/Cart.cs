using ProShop.Entities.Identity;

namespace ProShop.Entities;

public class Cart :EntityBase, IAuditableEntity
{
    public long UserId { get; set; }
    public long ProductVaraintId { get; set; }
    public short Count { get; set; }


    public User User { get; set; }
    public ProductVariant ProductVariant{ get; set; }

}