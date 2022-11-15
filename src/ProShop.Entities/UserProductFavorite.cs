using ProShop.Entities.Identity;

namespace ProShop.Entities;

public class UserProductFavorite : EntityBase ,IAuditableEntity
{

    public long UserId { get; set; }
    public long ProductId { get; set; }



    public User User { get; set; }
    public Product Product { get; set; }

}

