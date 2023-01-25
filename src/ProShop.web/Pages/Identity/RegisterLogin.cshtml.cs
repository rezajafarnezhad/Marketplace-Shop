using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities.Identity;
using ProShop.Services.Contracts.Identity;
using ProShop.ViewModels.Identity;
using ProShop.ViewModels.Identity.Settings;
namespace ProShop.web.Pages.Identity;

public class RegisterLoginModel : PageBase
{

    private readonly SiteSettings _siteSettings;
    private readonly IApplicationUserManager _applicationUserManager;
    private readonly IApplicationSigninManager _applicationSigninManager;
    private readonly ISmsSender _smsSender;
    private readonly ILogger<RegisterLoginModel> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterLoginModel(IOptionsMonitor<SiteSettings> siteSettings, IApplicationUserManager applicationUserManager, ILogger<RegisterLoginModel> logger, ISmsSender smsSender, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, IApplicationSigninManager applicationSigninManager)
    {
        _applicationUserManager = applicationUserManager;
        _logger = logger;
        _smsSender = smsSender;
        _webHostEnvironment = webHostEnvironment;
        _unitOfWork = unitOfWork;
        _siteSettings = siteSettings.CurrentValue;
        _applicationSigninManager = applicationSigninManager;
    }

    public RegisterLoginViewModel RegisterLogin { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(RegisterLoginViewModel registerLogin)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(String.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return Page();
        }

        var IsInputEmail = registerLogin.PhoneNumberOrEmail.IsEmail();
        if (!IsInputEmail)
        {
            var AddNewUser = false;
            var _user = await _applicationUserManager.FindByNameAsync(registerLogin.PhoneNumberOrEmail);
            if (_user is null)
            {
                _user = new User()
                {
                    UserName = registerLogin.PhoneNumberOrEmail,
                    PhoneNumber = registerLogin.PhoneNumberOrEmail,
                    Avatar = _siteSettings.UserDefaultAvatar,
                    Email = $"{StringHelpers.GenerateGuid()}@test.com",

                };

                var result = await _applicationUserManager.CreateAsync(_user);
                if (result.Succeeded)
                {
                    await _unitOfWork.SaveChangesAsync();
                    _logger.LogInformation(LogCodes.RegisterCode, $"{_user.UserName} Created a new account with phone number");
                    AddNewUser = true;
                }
                else
                {
                    ModelState.AddErrorsFromResult(result);
                    return Page();
                }

            }
            if (DateTime.Now > _user.SendSmsLastTime.AddMinutes(3) || AddNewUser)
            {
                var phoneNumberToken = await _applicationUserManager.GenerateChangePhoneNumberTokenAsync(_user, registerLogin.PhoneNumberOrEmail);
                // Send Sms token to the user
                //var sendSmsResult = await _smsSender.SendSmsAsync(_user.PhoneNumber, $"کد فعال سازی شما\n {phoneNumberToken}");
                //if (!sendSmsResult)
                //{
                //    ModelState.AddModelError(string.Empty, "در ارسال پیامک خطایی به وجود آمد، لطفا دوباره سعی نمایید.");
                //    return Page();
                //}

                _user.SendSmsLastTime = DateTime.Now;
                await _applicationUserManager.UpdateAsync(_user);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        return RedirectToPage("./LoginWithPhoneNumber", new { phoneNumber = registerLogin.PhoneNumberOrEmail });
    }

    public async Task<IActionResult> OnPostLogOut()
    {
        var user = User.Identity is { IsAuthenticated: true }

            ? await _applicationUserManager.FindByNameAsync(User.Identity.Name)
            : null;

        if (user != null)
        {
            await _applicationUserManager.UpdateSecurityStampAsync(user);
        }

        await _applicationSigninManager.SignOutAsync();

        return RedirectToPage("../Index");
    }
}

