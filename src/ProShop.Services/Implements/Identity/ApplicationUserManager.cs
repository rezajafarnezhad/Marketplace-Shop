using AutoMapper;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProShop.Common.Helpers;
using ProShop.DataLayer.Context;
using ProShop.Entities.Identity;
using ProShop.Services.Contracts.Identity;
using ProShop.ViewModels.Sellers;

namespace ProShop.Services.Implements.Identity
{
    public class ApplicationUserManager
        : UserManager<User>, IApplicationUserManager
    {
        private readonly DbSet<User> _users;
        private readonly IMapper _mapper;
        public ApplicationUserManager(IApplicationUserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<ApplicationUserManager> logger,
            IUnitOfWork unitOfWork, IMapper mapper)
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
            _mapper = mapper;
            _users = unitOfWork.Set<User>();
        }





        public async Task<DateTime?> GetSendSmsLastTime(string phoneNumber)
        {

            var result = await _users.Where(c => c.UserName == phoneNumber)
                .Select(c => c.SendSmsLastTime).FirstOrDefaultAsync();

            return result;

        }

        public async Task<bool> CheckForUserIsSeller(string phoneNumber)
        {
            return await _users.Where(c => c.UserName == phoneNumber)
                .Where(c=>c.UserRoles.All(r=>r.Role.Name != ConstantRoles.Seller))
                .AnyAsync(c => c.IsSeller);
        }

        public async Task<CreateSellerViewModel> GetUserInfoForCreateSeller(string phoneNumber)
        {
            var result =  await _mapper.ProjectTo<CreateSellerViewModel>(_users)
                .SingleOrDefaultAsync(c => c.PhoneNumber == phoneNumber);

            if (result?.BirthDate != null)
            {
                var parsedDateTime = DateTime.Parse(result.BirthDate);
                result.BirthDateEn = parsedDateTime.ToString("yyyy/MM/dd");
                result.BirthDate = parsedDateTime.ToShortPersianDate().ToPersianNumbers();
            }

            return result;

        }

        public async Task<User> GetUserForCreateSeller(string phoneNumber)
        {

            return await _users.Where(c => c.IsSeller)
                .Where(c => c.UserRoles.All(r => r.Role.Name != ConstantRoles.Seller))
                .SingleOrDefaultAsync(c => c.UserName == phoneNumber);
        }
    }
}