using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using ProShop.Common.Constants;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities.Identity;
using ProShop.Services.Contracts.Identity;
using ProShop.ViewModels.Identity.Settings;
using ProShop.ViewModels.Sellers;

namespace ProShop.web.Pages.Seller;
public class RegisterModel : PageModel
{

    private readonly IApplicationUserManager _userManager;
    private readonly SiteSettings _siteSettings;
    private readonly ILogger<RegisterModel> _logger;
    private readonly ISmsSender _smsSender;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterModel(IApplicationUserManager userManager, ISmsSender smsSender, IOptionsMonitor<SiteSettings> siteSettings, ILogger<RegisterModel> logger, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _smsSender = smsSender;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _siteSettings = siteSettings.CurrentValue;
    }


    [BindProperty]
    public RegisterSellerViewModel RegisterSeller { get; set; }
    public void OnGet()
    {
    }


    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(String.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return Page();
        }

        var AddUser = false;
        var _user = await _userManager.FindByNameAsync(RegisterSeller.PhoneNumber);
        if (_user is null)
        {
            _user = new User()
            {
                Email = RegisterSeller.Email,
                PhoneNumber = RegisterSeller.PhoneNumber,
                UserName = RegisterSeller.PhoneNumber,
                Avatar = _siteSettings.UserDefaultAvatar,
                IsSeller = true,

            };
            var result = await _userManager.CreateAsync(_user);
            if (result.Succeeded)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation(LogCodes.RegisterCode, $"{_user.UserName} Created a new account with phone number");
                AddUser = true;
            }
            else
            {
                ModelState.AddErrorsFromResult(result);
                return Page();
            }
        }
        if (DateTime.Now > _user.SendSmsLastTime.AddMinutes(3) || AddUser)
        {
            var phoneNumberToken = await _userManager.GenerateChangePhoneNumberTokenAsync(_user, RegisterSeller.PhoneNumber);
            // Send Sms token to the user
            //var sendSmsResult = await _smsSender.SendSmsAsync(_user.PhoneNumber, $"کد فعال سازی شما\n {phoneNumberToken}");
            //if (!sendSmsResult)
            //{
            //    ModelState.AddModelError(string.Empty, "در ارسال پیامک خطایی به وجود آمد، لطفا دوباره سعی نمایید.");
            //    return Page();
            //}

            _user.SendSmsLastTime = DateTime.Now;
            await _userManager.UpdateAsync(_user);
            await _unitOfWork.SaveChangesAsync();
        }

        return RedirectToPage("./ConfirmationPhoneNumber", new { phoneNumber = RegisterSeller.PhoneNumber });
    }

}

