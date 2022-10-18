using ProShop.Common.Attributes;
using ProShop.ViewModels.Veriants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProShop.ViewModels.Garantee;

public class ShowGarantiesViewModel
{
    public List<ShowGarantieeViewModel> Garanties { get; set; } = new();
    public SearchGarantieeViewModel SearchGarantiee { get; set; } = new();
    public PaginationViewModel Pagination { get; set; }
    = new();

}

public class ShowGarantieeViewModel
{
    [Display(Name = "شناسه")]
    public long Id { get; set; }

    [Display(Name = "عنوان کامل گارانتی")]
    public string FullTitle { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "وضعیت")]
    public bool IsConfirmed { get; set; }

    [Display(Name = "تصویر")]
    public string Picture { get; set; }

    [Display(Name = "تعداد ماه گارانتی")]
    public byte MonthCount { get; set; }

  
}

public class SearchGarantieeViewModel
{

    [ContainsSearch]
    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [EqualSearch]
    [Display(Name = "تعداد ماه گارانتی")]
    public byte? MonthCount { get; set; }

    [EqualSearch]
    [Display(Name = "وضعیت")]
    public bool? IsConfirmed { get; set; }

    [Display(Name = "نمایش بر اساس")]
    public SortingGuarantees Sorting { get; set; }

    [Display(Name = "مرتب سازی بر اساس")]
    public SortingOrder SortingOrder { get; set; }
}


public enum SortingGuarantees
{
    [Display(Name = "شناسه")]
    Id,

    [Display(Name = "عنوان")]
    Title,

    [Display(Name = "تعداد ماه گارانتی")]
    MonthsCount
}
