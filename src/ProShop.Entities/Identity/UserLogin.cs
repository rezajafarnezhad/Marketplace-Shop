using Microsoft.AspNetCore.Identity;

namespace ProShop.Entities.Identity;

public class UserLogin : IdentityUserLogin<long>, IAuditableEntity
{
    public virtual User User { get; set; }
}