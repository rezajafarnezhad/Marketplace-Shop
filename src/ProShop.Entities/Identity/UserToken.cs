using Microsoft.AspNetCore.Identity;

namespace ProShop.Entities.Identity;

public class UserToken : IdentityUserToken<long>, IAuditableEntity
{
    public virtual User User { get; set; }
}