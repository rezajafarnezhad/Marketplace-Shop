using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities.Identity;
using ProShop.Services.Contracts.Identity;

namespace ProShop.Services.Implements.Identity
{
    public class ApplicationUserStore : UserStore<User, Role, ApplicationDbContext, long, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>, IApplicationUserStore
    {
        public ApplicationUserStore(
            IUnitOfWork Uow,
            IdentityErrorDescriber describer = null)
            : base((ApplicationDbContext)Uow, describer)
        {
            AutoSaveChanges = false;
        }
    }
}