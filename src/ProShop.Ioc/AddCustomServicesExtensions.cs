using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProShop.DataLayer.Context;
using ProShop.Entities.Identity;
using ProShop.Services.Contracts;
using ProShop.Services.Contracts.Identity;
using ProShop.Services.Implements;
using ProShop.Services.Implements.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace ProShop.Ioc;

public static class AddCustomServicesExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IPrincipal>(provider =>
            provider.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.User ?? ClaimsPrincipal.Current);


        services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationClaimsPrincipalFactory>();
        services.AddScoped<UserClaimsPrincipalFactory<User, Role>, ApplicationClaimsPrincipalFactory>();

        services.AddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();

        services.AddScoped<IApplicationUserStore, ApplicationUserStore>();
        services.AddScoped<UserStore<User, Role, ApplicationDbContext, long, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>, ApplicationUserStore>();

        services.AddScoped<IApplicationRoleStore, ApplicationRoleStore>();
        services.AddScoped<RoleStore<Role, ApplicationDbContext, long, UserRole, RoleClaim>, ApplicationRoleStore>();

        services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
        services.AddScoped<UserManager<User>, ApplicationUserManager>();

        services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
        services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();

        services.AddScoped<IApplicationSigninManager, ApplicationSigninManager>();
        services.AddScoped<SignInManager<User>, ApplicationSigninManager>();

        services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();


        services.AddScoped<ISmsSender, AuthMessageSender>();
        services.AddScoped<IHttpClientService, HttpClientService>();
        services.AddScoped<IUploadFileService, UploadFileService>();


        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IFeatureService,FeatureService>();
        services.AddScoped<ICategoryFeatureService, CategoryFeatureService>();

        return services;
    }
}