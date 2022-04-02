using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProShop.DataLayer.Context;
using ProShop.Entities.Identity;
using ProShop.Services.Contracts.Identity;

namespace ProShop.Services.Implements.Identity
{
    public class ApplicationRoleManager : RoleManager<Role>, IApplicationRoleManager
    {
        public ApplicationRoleManager(IApplicationRoleStore store, IEnumerable<IRoleValidator<Role>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<ApplicationRoleManager> logger)
            : base((RoleStore<Role, ApplicationDbContext, long, UserRole, RoleClaim>)store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}