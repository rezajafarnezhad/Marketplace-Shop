using Microsoft.AspNetCore.Identity;

namespace ProShop.Entities.Identity;

public class UserClaim : IdentityUserClaim<long>, IAuditableEntity
{
    public virtual User User { get; set; }
}