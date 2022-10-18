using Microsoft.AspNetCore.Mvc;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ProShop.ViewModels.Consignments;

public class AddDescriptionForConsignmentViewModel
{
    [Display(Name = "شناسه محموله")]
    [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    public long ConsignmentId { get; set; }

    [Display(Name = "توضیحات محموله")]
    [MakeTinyMceRequired]
    public string Description { get; set; }
}