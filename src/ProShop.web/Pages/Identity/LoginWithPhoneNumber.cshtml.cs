using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Helpers;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts.Identity;
using ProShop.ViewModels.Identity;

namespace ProShop.web.Pages.Identity;

public class LoginWithPhoneNumberModel : PageModel
{
    private readonly IApplicationUserManager _applicationUserManager;
    private readonly IApplicationSigninManager _applicationSigninManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISmsSender _smsSender;
    public LoginWithPhoneNumberModel(IApplicationUserManager applicationUserManager, IApplicationSigninManager applicationSigninManager, ISmsSender smsSender, IUnitOfWork unitOfWork)
    {
        _applicationUserManager = applicationUserManager;
        _applicationSigninManager = applicationSigninManager;
        _smsSender = smsSender;
        _unitOfWork = unitOfWork;
    }


    public LoginWithPhoneNumberViewModel LoginWithPhoneNumber { get; set; } = new LoginWithPhoneNumberViewModel();
    
    [ViewData]
    public string ActivationCode { get; set; }
    public async Task<IActionResult> OnGetAsync(string phoneNumber)
    {
        var userSendSmsLastTime = await _applicationUserManager.GetSendSmsLastTime(phoneNumber);
        if (userSendSmsLastTime is null)
        {
            return RedirectToPage("/Error");
        }


        #region Development

        var User = await _applicationUserManager.FindByNameAsync(phoneNumber);
        var PhoneNumberToken = await _applicationUserManager.GenerateChangePhoneNumberTokenAsync(User, phoneNumber);
        ActivationCode = PhoneNumberToken;


        #endregion


        var (min, sec) = userSendSmsLastTime.Value.GetMinuteAndSecondForLoginWithPhoneNumberPage();
        LoginWithPhoneNumber.SendSmsLastTimeMinute = min;
        LoginWithPhoneNumber.SendSmsLastTimeSecond = sec;
        LoginWithPhoneNumber.PhoneNumber = phoneNumber;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(LoginWithPhoneNumberViewModel loginWithPhoneNumber)
    {
        if (!ModelState.IsValid)
            return Page();


        var _user = await _applicationUserManager.FindByNameAsync(loginWithPhoneNumber.PhoneNumber);
        if (_user == null)
            return Page();


        var result = await _applicationUserManager.VerifyChangePhoneNumberTokenAsync(_user, loginWithPhoneNumber.ActivationCode, loginWithPhoneNumber.PhoneNumber);

        if (!result)
            return Page();


        await _applicationSigninManager.SignInAsync(_user, true);
        return RedirectToPage("/Index");
    }


    public async Task<IActionResult> OnPostReSendUserSmsActivationAsync(string phoneNumber)
    {
        //System.Threading.Thread.Sleep(2000);
        var _user = await _applicationUserManager.FindByNameAsync(phoneNumber);
        if (_user is null)
            return new JsonResult(new JsonResultOperation(false));

        if(_user.SendSmsLastTime.AddMinutes(3) > DateTime.Now)
            return new JsonResult(new JsonResultOperation(false));

        var phoneNumberToken = await _applicationUserManager.GenerateChangePhoneNumberTokenAsync(_user, phoneNumber);
        //Send Sms
        //var sendSmsResult = await _smsSender.SendSmsAsync(_user.PhoneNumber, $"کد فعال سازی شما\n {phoneNumberToken}");
        //if (!sendSmsResult)
        //{
        //    return new JsonResult(new JsonResultOperation(false, "در ارسال پیامک خطایی به وجود آمد، لطفا دوباره سعی نمایید"));
        //}
        _user.SendSmsLastTime = DateTime.Now;
        await _unitOfWork.SaveChangesAsync();
        return new JsonResult(new JsonResultOperation(true, "کد فعال سازی مجددا پیامک شد")
        {
            Data = new
            {
                activationCode = phoneNumberToken
            }

        });

    }
}

