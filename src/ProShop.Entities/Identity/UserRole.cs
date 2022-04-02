using Microsoft.AspNetCore.Identity;

namespace ProShop.Entities.Identity;

public class UserRole : IdentityUserRole<long>, IAuditableEntity
{
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}