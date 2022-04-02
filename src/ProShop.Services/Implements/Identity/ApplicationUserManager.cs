using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProShop.DataLayer.Context;
using ProShop.Entities.Identity;
using ProShop.Services.Contracts.Identity;

namespace ProShop.Services.Implements.Identity
{
    public class ApplicationUserManager
        : UserManager<User>, IApplicationUserManager
    {
        private readonly DbSet<User> _users;
        public ApplicationUserManager(IApplicationUserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<ApplicationUserManager> logger,
            IUnitOfWork unitOfWork)
            : base(
                (UserStore<User,
                    Role,
                    ApplicationDbContext,
                    long,
                    UserClaim,
                    UserRole,
                    UserLogin,
                    UserToken,
                    RoleClaim>)store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger)
        {
            _users = unitOfWork.Set<User>();
        }





        public async Task<DateTime?> GetSendSmsLastTime(string phoneNumber)
        {

            var result = await _users.Where(c => c.UserName == phoneNumber)
                .Select(c => c.SendSmsLastTime).FirstOrDefaultAsync();

            return result;

        }
    }
}