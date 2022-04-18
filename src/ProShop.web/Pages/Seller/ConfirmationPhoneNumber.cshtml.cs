using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts.Identity;
using ProShop.ViewModels.Sellers;

namespace ProShop.web.Pages.Seller
{
    public class ConfirmationPhoneNumberModel : PageBase
    {

        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationSigninManager _applicationSigninManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsSender _smsSender;
        public ConfirmationPhoneNumberModel(IApplicationUserManager userManager, IApplicationSigninManager applicationSigninManager, IUnitOfWork unitOfWork, ISmsSender smsSender)
        {
            _userManager = userManager;
            _applicationSigninManager = applicationSigninManager;
            _unitOfWork = unitOfWork;
            _smsSender = smsSender;
        }

        [BindProperty]
        public ConfirmationSellerPhoneNumberViewModel Confirmation { get; set; } = new();

        [ViewData]
        public string PhoneNumber { get; set; }

        [ViewData]
        public string ActivationCode { get; set; }
        public async Task<IActionResult> OnGet(string phoneNumber)
        {
            PhoneNumber = phoneNumber;

            var userSendSmsLastTime = await _userManager.GetSendSmsLastTime(phoneNumber);
            if (userSendSmsLastTime is null)
            {
                return RedirectToPage("/Error");
            }


            #region Development

            var User = await _userManager.FindByNameAsync(phoneNumber);
            var PhoneNumberToken = await _userManager.GenerateChangePhoneNumberTokenAsync(User, phoneNumber);
            ActivationCode = PhoneNumberToken;


            #endregion

            var (min, sec) = userSendSmsLastTime.Value.GetMinuteAndSecondForLoginWithPhoneNumberPage();
            Confirmation.SendSmsLastTimeMinute = min;
            Confirmation.SendSmsLastTimeSecond = sec;
            Confirmation.PhoneNumber = phoneNumber;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }

            var _user = await _userManager.FindByNameAsync(Confirmation.PhoneNumber);
            if (_user == null)
                return Json(new JsonResultOperation(false,"کاربری یافت نشد"));



            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(_user, Confirmation.ActivationCode, Confirmation.PhoneNumber);

            if (!result)
                return Json(new JsonResultOperation(false, "کد وارد شده صحیح نمیباشد"));



            await _applicationSigninManager.SignInAsync(_user, true);
            return Json(new JsonResultOperation(true, "با موفقیت وارد شدید"));

        }


        public async Task<IActionResult> OnPostReSendSellerSmsActivation(string phoneNumber)
        {
            //System.Threading.Thread.Sleep(2000);
            var _user = await _userManager.FindByNameAsync(phoneNumber);
            if (_user is null)
                return Json(new JsonResultOperation(false));

            if (_user.SendSmsLastTime.AddMinutes(3) > DateTime.Now)
                return Json(new JsonResultOperation(false));

            var phoneNumberToken = await _userManager.GenerateChangePhoneNumberTokenAsync(_user, phoneNumber);
            //Send Sms
            //var sendSmsResult = await _smsSender.SendSmsAsync(_user.PhoneNumber, $"کد فعال سازی شما\n {phoneNumberToken}");
            //if (!sendSmsResult)
            //{
            //    return Json(new JsonResultOperation(false, "در ارسال پیامک خطایی به وجود آمد، لطفا دوباره سعی نمایید"));
            //}
            _user.SendSmsLastTime = DateTime.Now;
            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "کد فعال سازی مجددا پیامک شد")
            {
                Data = new
                {
                    activationCode = phoneNumberToken
                }

            });

        }
    }
}
