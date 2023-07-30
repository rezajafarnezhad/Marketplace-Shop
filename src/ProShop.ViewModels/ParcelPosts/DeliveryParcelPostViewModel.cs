using ProShop.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProShop.ViewModels.ParcelPosts;

public class DeliveryParcelPostViewModel
{
    public long Id { get; set; }

    [Display(Name ="کد رهگیری اداره پست")]
    [Required(ErrorMessage =AttributesErrorMessages.RequiredMessage)]
    [MaxLength(30,ErrorMessage =AttributesErrorMessages.MaxLengthMessage)]
    public string PostTrackingCode { get; set; }
}