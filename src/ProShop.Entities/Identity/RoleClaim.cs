using Microsoft.AspNetCore.Identity;

namespace ProShop.Entities.Identity;

public class RoleClaim : IdentityRoleClaim<long>, IAuditableEntity
{
    public virtual Role Role { get; set; }
}