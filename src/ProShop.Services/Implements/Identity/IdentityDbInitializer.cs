using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Utilities;
using ProShop.Common.GuardToolkit;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Entities.Identity;
using ProShop.Services.Contracts;
using ProShop.Services.Contracts.Identity;
using ProShop.ViewModels.Identity.Settings;

namespace ProShop.Services.Implements.Identity;


public class IdentityDbInitializer : IIdentityDbInitializer
{
    private readonly IOptionsSnapshot<SiteSettings> _options;
    private readonly IApplicationUserManager _applicationUserManager;
    private readonly ILogger<IdentityDbInitializer> _logger;
    private readonly IApplicationRoleManager _roleManager;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProvinceAndCityService _provinceAndCityService;
    private readonly ISellerService _sellerService;
    private readonly IProductShortLinkService _productShortLinkService;
    public IdentityDbInitializer(
        IApplicationUserManager applicationUserManager,
        IServiceScopeFactory scopeFactory,
        IApplicationRoleManager roleManager,
        IOptionsSnapshot<SiteSettings> adminUserSeedOptions,
        ILogger<IdentityDbInitializer> logger, IUnitOfWork unitOfWork, IProvinceAndCityService provinceAndCityService, ISellerService sellerService, IProductShortLinkService productShortLinkService)
    {
        _applicationUserManager = applicationUserManager;
        _applicationUserManager.CheckArgumentIsNull(nameof(_applicationUserManager));

        _scopeFactory = scopeFactory;
        _scopeFactory.CheckArgumentIsNull(nameof(_scopeFactory));

        _roleManager = roleManager;
        _roleManager.CheckArgumentIsNull(nameof(_roleManager));

        _options = adminUserSeedOptions;
        _options.CheckArgumentIsNull(nameof(_options));

        _logger = logger;
        _unitOfWork = unitOfWork;
        _provinceAndCityService = provinceAndCityService;
        _sellerService = sellerService;
        _logger.CheckArgumentIsNull(nameof(_logger));
        _productShortLinkService = productShortLinkService;
    }

    /// <summary>
    /// Applies any pending migrations for the context to the database.
    /// Will create the database if it does not already exist.
    /// </summary>
    public void Initialize()
    {
        _scopeFactory.RunScopedService<ApplicationDbContext>(context =>
        {
            context.Database.Migrate();
        });
    }


    /// <summary>
    /// Adds some default values to the IdentityDb
    /// </summary>
    public void SeedData()
    {
        _scopeFactory.RunScopedService<IIdentityDbInitializer>(identityDbSeedData =>
        {
            var result = identityDbSeedData.SeedDatabaseWithAdminUserAsync().Result;
            if (result == IdentityResult.Failed())
            {
                throw new InvalidOperationException(result.DumpErrors());
            }

            var sellerRole = identityDbSeedData.SeedSellerRole().Result;
            if (sellerRole == IdentityResult.Failed())
            {
                throw new InvalidOperationException(sellerRole.DumpErrors());
            }
            var WarehouseRole = identityDbSeedData.SeedWarehouseRole().Result;
            if (WarehouseRole == IdentityResult.Failed())
            {
                throw new InvalidOperationException(WarehouseRole.DumpErrors());
            } 
            
            var DeliveryManRole = identityDbSeedData.SeedDeliveryManRole().Result;
            if (DeliveryManRole == IdentityResult.Failed())
            {
                throw new InvalidOperationException(DeliveryManRole.DumpErrors());
            }
            var UserForseller = identityDbSeedData.SeedUserForSeller().Result;
            if (UserForseller == IdentityResult.Failed())
            {
                throw new InvalidOperationException(UserForseller.DumpErrors());
            }

            identityDbSeedData.SeedProvincesAndCities().GetAwaiter().GetResult();
            identityDbSeedData.SeedSeller().GetAwaiter().GetResult();
            identityDbSeedData.ProductShortLinks().GetAwaiter().GetResult();
        });
    }

    public async Task<IdentityResult> SeedDatabaseWithAdminUserAsync()
    {
        var adminUserSeed = _options.Value.AdminUserSeed;
        adminUserSeed.CheckArgumentIsNull(nameof(adminUserSeed));

        var name = adminUserSeed.Username;
        var password = adminUserSeed.Password;
        var email = adminUserSeed.Email;

        var thisMethodName = nameof(SeedDatabaseWithAdminUserAsync);

        var adminUser = await _applicationUserManager.FindByNameAsync(name);
        if (adminUser != null)
        {
            _logger.LogInformation($"{thisMethodName}: adminUser already exists.");
            return IdentityResult.Success;
        }

        //Create the `Admin` Role if it does not exist
        var adminRole = await _roleManager.FindByNameAsync(ConstantRoles.Admin);
        if (adminRole == null)
        {
            adminRole = new Role(ConstantRoles.Admin, "ادمین کل سیستم");
            var adminRoleResult = await _roleManager.CreateAsync(adminRole);
            if (adminRoleResult == IdentityResult.Failed())
            {
                _logger.LogError($"{thisMethodName}: adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation($"{thisMethodName}: adminRole already exists.");
        }

        adminUser = new User
        {
            UserName = name,
            Email = email,
            EmailConfirmed = true,
            Avatar = _options.Value.UserDefaultAvatar,
            CreatedDateTime = DateTime.Now,
            IsActive = true,
            SendSmsLastTime = DateTime.Now,

        };
        var adminUserResult = await _applicationUserManager.CreateAsync(adminUser, password);
        if (adminUserResult == IdentityResult.Failed())
        {
            _logger.LogError($"{thisMethodName}: adminUser CreateAsync failed. {adminUserResult.DumpErrors()}");
            return IdentityResult.Failed();
        }

        await _unitOfWork.SaveChangesAsync();
        var addToRoleResult = await _applicationUserManager.AddToRoleAsync(adminUser, adminRole.Name);
        if (addToRoleResult == IdentityResult.Failed())
        {
            _logger.LogError($"{thisMethodName}: adminUser AddToRoleAsync failed. {addToRoleResult.DumpErrors()}");
            return IdentityResult.Failed();
        }

        await _unitOfWork.SaveChangesAsync();

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> SeedSellerRole()
    {
        var thisMethodName = nameof(SeedSellerRole);
        //Create the `Seller` Role if it does not exist
        var sellerRole = await _roleManager.FindByNameAsync(ConstantRoles.Seller);
        if (sellerRole == null)
        {
            sellerRole = new Role(ConstantRoles.Seller, "فروشنده سیستم");
            var sellerRoleResult = await _roleManager.CreateAsync(sellerRole);
            if (sellerRoleResult == IdentityResult.Failed())
            {
                _logger.LogError($"{thisMethodName}: sellerRole CreateAsync failed. {sellerRoleResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation($"{thisMethodName}: sellerRole already exists.");
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> SeedWarehouseRole()
    {
        var thisMethodName = nameof(SeedWarehouseRole);
        //Create the `Warehouse` Role if it does not exist
        var warehouseRole = await _roleManager.FindByNameAsync(ConstantRoles.Warehouse);
        if (warehouseRole == null)
        {
            warehouseRole = new Role(ConstantRoles.Warehouse, "انباردار سیستم");
            var warehouseRoleResult = await _roleManager.CreateAsync(warehouseRole);
            if (warehouseRoleResult == IdentityResult.Failed())
            {
                _logger.LogError($"{thisMethodName}: warehouseRole CreateAsync failed. {warehouseRoleResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation($"{thisMethodName}: warehouseRole already exists.");
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> SeedDeliveryManRole()
    {
        var thisMethodName = nameof(SeedDeliveryManRole);
        //Create the `Warehouse` Role if it does not exist
        var deliveryManRole = await _roleManager.FindByNameAsync(ConstantRoles.DeliveryMan);
        if (deliveryManRole == null)
        {
            deliveryManRole = new Role(ConstantRoles.DeliveryMan, "پیک");
            var deliveryManRoleResult = await _roleManager.CreateAsync(deliveryManRole);
            if (deliveryManRoleResult == IdentityResult.Failed())
            {
                _logger.LogError($"{thisMethodName}: deliveryManRole CreateAsync failed. {deliveryManRoleResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation($"{thisMethodName}: warehouseRole already exists.");
        }

        return IdentityResult.Success;
    }


    public async Task<IdentityResult> SeedUserForSeller()
    {
        var user = await _applicationUserManager.FindByNameAsync("09103332211");
        if (user is null)
        {
            var userToAdd = new User()
            {
                FirstName = "میلاد",
                LastName = "احمدی",
                BirthDate = new DateTime(1990, 8, 22),
                Email = "milad.ahmadi@gmail.com",
                EmailConfirmed = true,
                Avatar = _options.Value.UserDefaultAvatar,
                Gender = Gender.Man,
                IsSeller = true,
                NationalCode = "123456789",
                IsActive = true,
                PhoneNumber = "09103332211",
                UserName = "09103332211",
                SendSmsLastTime = DateTime.Now,
                PhoneNumberConfirmed = true
            };
            await _applicationUserManager.CreateAsync(userToAdd);
            await _unitOfWork.SaveChangesAsync();
        }
        return IdentityResult.Success;
    }

    public async Task SeedProvincesAndCities()
    {
        if (!await _provinceAndCityService.AnyAsync())
        {
            var p1 = new ProvinceAndCity()
            {
                Title = "تهران",
                IsDeleted = false,
            };
            var c1 = new ProvinceAndCity()
            {
                Title = "تهران",
                Parent = p1,
                IsDeleted = false,
            };
            var c2 = new ProvinceAndCity()
            {
                Title = "شهریار",
                Parent = p1,
                IsDeleted = false,
            };
            var p2 = new ProvinceAndCity()
            {
                Title = "اصفهان",
                IsDeleted = false,
            };
            var c3 = new ProvinceAndCity()
            {
                Title = "اصفهان",
                Parent = p2,
                IsDeleted = false,
            };
            var c4 = new ProvinceAndCity()
            {
                Title = "کاشان",
                Parent = p2,
                IsDeleted = false,
            };
            await _provinceAndCityService.AddAsync(p1);
            await _provinceAndCityService.AddAsync(p2);
            await _provinceAndCityService.AddAsync(c1);
            await _provinceAndCityService.AddAsync(c2);
            await _provinceAndCityService.AddAsync(c3);
            await _provinceAndCityService.AddAsync(c4);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task SeedSeller()
    {
        if (!await _sellerService.IsExistsBy(nameof(Entities.Seller.ShopName),
                "فروشگاه برادران احمدی"))
        {
            var user = await _applicationUserManager.FindByNameAsync("09103332211");
            var (provinceId, cityId) = await _provinceAndCityService.GetForSeedData();
            var seller = new Entities.Seller()
            {
                SellerCode = 1,
                AboutSeller =
                    "<p>شرکت رها گستر دانا از سال 1380 شروع به کار کرده و در این مدت توانسته با تکیه بر دانش کارکنان خود در زمنیه فروش لوارم جانبی موبایل اقدام به فعالیت کند</p>",
                Address = "خیابان آزادی - کوچه بهار 5 - پلاک 39",
                IsRealPerson = false,
                CompanyName = "شرکت رها گستر دانا",
                CompanyType = CompanyType.LimitedResponsibility,
                SignatureOwners = "میلاد احمدی - منصور کریمی - سینا شاهرخی",
                RegisterNumber = "12356488942128489",
                EconomicCode = "123234345456",
                NationalId = "159357258456147369",
                Telephone = "02199335484",
                Website = "https://rahagostardana.com",
                IsActive = true,
                PostalCode = "5544184933",
                ShopName = "فروشگاه برادران احمدی",
                ShabaNumber = "123456789123456789123456",
                Logo = "9dc9cb1134ab42b8978758834445c15b.png",
                IdCartPicture = "35366c86a8fc4a67ac5abba5978ef381.jpg",
                IsDeleted = false,
                ProvinceId = provinceId,
                CityId = cityId,
                DocumentStatus = DocumentStatus.Confirmed,
                User = user
            };
            await _sellerService.AddAsync(seller);
            await _applicationUserManager.AddToRoleAsync(user, ConstantRoles.Seller);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task ProductShortLinks()
    {
        if (!await _productShortLinkService.AnyAsync())
        {
            var links = new List<ProductShortLink>();
            for (char letter1 = 'A'; letter1 <= 'Z'; letter1++)
            {
                if (!char.IsLetterOrDigit(letter1))
                    continue;
               
                
                for (char letter2 = '0'; letter2 <= 'Z'; letter2++)
                {
                    if (!char.IsLetterOrDigit(letter2))
                        continue;

                    for (char letter3 = '0'; letter3 <= 'Z'; letter3++)
                    {
                        if (!char.IsLetterOrDigit(letter3))
                            continue;

                        var link = letter1.ToString() + letter2 + letter3;
                        links.Add(new ProductShortLink()
                        {
                            Link = link,
                            IsDeleted = false,
                        });
                    }
                }


            }

            await _productShortLinkService.AddRangeAsync(links);
            await _unitOfWork.SaveChangesAsync();

        }

    }
}