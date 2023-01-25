using ProShop.Common.Constants;
using ProShop.Entities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProShop.ViewModels.Consignments;

public class ShowConsignmentDetailsViewModel
{
    public long Id { get; set; }
 
    [Display(Name = "تاریخ تحویل")]
    public string DeliveryDate { get; set; }

    [Display(Name = "توضیحات ارسالی شما")]
    public string Description { get; set; }
    public string SellerShopName { get; set; }
    public ConsignmentStatus ConsignmentStatus { get; set; }

    public List<ShowConsignmentItemsViewModel> ConsignmentItems { get; set; } = new();
}

public class ShowConsignmentItemsViewModel
{
    [Display(Name = "شناسه آیتم محموله")]
    public long Id { get; set; }

    [Display(Name = "شناسه کالا")]
    public long ProductVariantProductId { get; set; }

    [Display(Name = "عنوان محصول")]
    public string ProductVariantProductPersianTitle { get; set; }

    [Display(Name = "مقدار تنوع")]
    public string ProductVariantVariantValue { get; set; }

    [Display(Name = "بارکد")]
    public string Barcode { get; set; }

    public string ProductVariantVariantColorCode { get; set; }

    public bool? ProductVariantVariantIsColor { get; set; }

    [Display(Name = "قیمت")]
    public int ProductVariantPrice { get; set; }

    [Display(Name = "تعداد")]
    public int Count { get; set; }


}