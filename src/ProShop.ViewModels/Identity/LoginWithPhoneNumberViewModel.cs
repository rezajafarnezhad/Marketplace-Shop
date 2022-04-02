using System.ComponentModel.DataAnnotations;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Identity;

public class LoginWithPhoneNumberViewModel
{
    [Display(Name = "کد فعال سازی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [StringLength(6,MinimumLength = 6, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public string ActivationCode { get; set; }
    public string PhoneNumber { get; set; }
    public byte SendSmsLastTimeMinute { get; set; }
    public byte SendSmsLastTimeSecond { get; set; }
        

}