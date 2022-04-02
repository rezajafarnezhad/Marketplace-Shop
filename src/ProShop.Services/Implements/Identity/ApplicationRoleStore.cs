using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities.Identity;
using ProShop.Services.Contracts.Identity;

namespace ProShop.Services.Implements.Identity
{
    public class ApplicationRoleStore
        : RoleStore<Role, ApplicationDbContext, long, UserRole, RoleClaim>, IApplicationRoleStore
    {
        public ApplicationRoleStore(IUnitOfWork Uow, IdentityErrorDescriber describer = null) : base((ApplicationDbContext)Uow, describer)
        {
        }
    }
}