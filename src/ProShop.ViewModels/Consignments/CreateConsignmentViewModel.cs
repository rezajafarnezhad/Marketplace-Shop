using ProShop.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ProShop.ViewModels.Consignments;

public class CreateConsignmentViewModel
{
    [Display(Name ="تاریخ تحویل")]
    [Required(ErrorMessage =AttributesErrorMessages.RequiredMessage)]
    public string DeliveryDate { get; set; }

    public List<string> Variants { get; set; } = new();
}
