using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using ProShop.Entities;
using ProShop.ViewModels.Brands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ProShop.ViewModels.Orders;

public class ShowOrdersViewModel
{
    public PaginationViewModel Pagination { get; set; } = new();
    public List<ShowOrder> Orders { get; set; } = new();
    public SearchOrders SearchOrders { get; set; } = new();

}

public class ShowOrder
{

    [Display(Name = "شناسه")]
    public long Id { get; set; }

    [Display(Name = "گیرنده")]
   
    public string AddressFullName { get; set; }

    [Display(Name = "مقصد")]
    public string Destination { get; set; }

    [Display(Name = "درگاه پرداخت")]

    public PaymentGateway? PaymentGateway { get; set; }

    [Display(Name = "وضعیت")]

    public OrderStatus OrderStatus { get; set; }

    [Display(Name = "شماره سفارش")]
    public long OrderNumber { get; set; }

    [Display(Name = "کد تراکنش بانک")]
    public string BankTransactionCode { get; set; }
    [Display(Name ="تاریخ ایجاد")]
    public string CreatedDateTime { get; set; }

}

public class SearchOrders
{
    [EqualSearch]
    [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    [Display(Name ="شماره سفارش")]
    public long? OrderNumber { get; set; }

    [Display(Name = "گیرنده")]
    [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
        ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
    [MaxLength(400,ErrorMessage =AttributesErrorMessages.MaxLengthMessage)]
    public string FullName { get; set; }

    [Display(Name = "تاریخ ایجاد")]
    [RegularExpression(@"^[۰-۹]{4}\/(۰[۱-۹]|۱[۰-۲])\/(۰[۱-۹]|[۱۲][۰-۹]|۳[۰۱])$", ErrorMessage = "تاریخ به درستی وارد شود")]
    [EqualDateTimeSearch]

    public string CreatedDateTime { get; set; }

    [Display(Name = "نمایش بر اساس")]
    public SortingOrders Sorting { get; set; }

    [Display(Name = "مرتب سازی بر اساس")] 
    public SortingOrder SortingOrder { get; set; } = SortingOrder.Desc;


    [Display(Name = "استان")]
    [Range(1,long.MaxValue,ErrorMessage=AttributesErrorMessages.RegularExpressionMessage)]
    public int? ProvinceId { get; set; }
    public List<SelectListItem> Provinces { get; set; } = new();

    [Display(Name = "شهرستان")]
   // [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public int? CityId { get; set; }

    [EqualEnumSearch] 
    [Display(Name = "درگاه پرداخت")]
    public PaymentGateway? PaymentGateway { get; set; }

    [EqualSearch]
    [Display(Name = "وضعیت")]

    public OrderStatus? OrderStatus { get; set; }

    [Display(Name = "فقط پرداخت شده ها")]
    public bool OnlyPayedOrders { get; set; } = true;

}
public enum SortingOrders
{
    [Display(Name = "شماره سفارش")]
    OrderNumber,
    [Display(Name = "تاریخ ایجاد")]
    CreatedDateTime,

}


public class OrderDetailsViewModel
{
    public long Id { get; set; }
    public long OrderNumber { get; set; }
    public string AddressFullName { get; set; }
    public string AddressPhoneNumber { get; set; }
    public string AddressAddressLine { get; set; }
    public string CreatedDateTime { get; set; }
    public bool PayFromWallet { get; set; }
    public List<ParcelPostForOrderDetailsViewModel> ParcalPosts { get; set; } = new();
}

public class ParcelPostForOrderDetailsViewModel
{
    public ProductDimensions Dimensions { get; set; }
    public ParcelPostStatus ParcelPostStatus { get; set; }
    public string PostTrackingCode { get; set; }
    public int ShippingPrice { get; set; }
    public List<ParcelPostItemForOrderDetailsViewModel> ParcelPostItems { get; set; } = new();

}
public class ParcelPostItemForOrderDetailsViewModel
{
    public string ProductVariantProductPersianTitle { get; set; }
    public string ProductVariantSellerShopName { get; set; }
    public string GaranteeFullTitle { get; set; }
    public int Price { get; set; }

    public int DiscountPrice { get; set; }

    public int Count { get; set; }

    public int Score { get; set; }
    public string ProductPicture { get; set; }
    public int ProductVariantProductProductCode { get; set; }
    public string ProductVariantProductSlug { get; set; }
}