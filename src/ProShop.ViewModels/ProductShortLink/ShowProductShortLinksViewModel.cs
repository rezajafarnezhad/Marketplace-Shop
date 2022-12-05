using ProShop.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProShop.ViewModels.ProductShortLink;

public class ShowProductShortLinksViewModel
{
    public PaginationViewModel Pagination { get; set; }
   = new();

    public ProductShortLinkSearchViewModel shortLinkSearch { get; set; }
    public List<ShowProductShortLinkViewModel> shortLink { get; set; } = new();

}

public class ShowProductShortLinkViewModel
{
    [Display(Name ="شناسه")]
    public long Id { get; set; }

    [Display(Name = "لینک")]

    public string Link { get; set; }

    [Display(Name = "وضعیت")]

    public bool IsUsed { get; set; }

    [Display(Name = "نام محصول")]

    public string? productPersianTitle { get; set; }
    [Display(Name = "slug")]

    public string? productSlug { get; set; }

    [Display(Name = "کد محصول")]

    public int? productProductCode { get; set; }
}

public class ProductShortLinkSearchViewModel
{
    [EqualSearch]
    public string Link { get; set; }

    [EqualSearch]
    [Display(Name = "وضعیت")]
    public bool? IsUsed { get; set; }
}


