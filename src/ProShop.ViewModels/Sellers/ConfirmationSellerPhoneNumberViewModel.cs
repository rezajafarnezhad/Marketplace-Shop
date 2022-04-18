using System.ComponentModel.DataAnnotations;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Sellers;

public class ConfirmationSellerPhoneNumberViewModel
{
    [Display(Name = "کد فعال سازی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [RegularExpression(@"[\d]{6}", ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    [MaxLength(6, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string ActivationCode { get; set; }
    [Display(Name = "شماره تلفن")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]

    public string PhoneNumber { get; set; }
    public byte SendSmsLastTimeMinute { get; set; }
    public byte SendSmsLastTimeSecond { get; set; }
}