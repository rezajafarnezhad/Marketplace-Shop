using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProShop.Entities.Identity;
using ProShop.Services.Contracts.Identity;

namespace ProShop.Services.Implements.Identity
{
    public class ApplicationSigninManager
      : SignInManager<User>, IApplicationSigninManager
    {
        public ApplicationSigninManager(IApplicationUserManager userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<ApplicationSigninManager> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<User> confirmation)
            : base((UserManager<User>)userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }
    }
}
