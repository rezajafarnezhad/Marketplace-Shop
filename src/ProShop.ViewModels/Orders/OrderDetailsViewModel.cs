using ProShop.Entities;

namespace ProShop.ViewModels.Orders;

public class OrderDetailsViewModel
{
    public long Id { get; set; }
    public long OrderNumber { get; set; }
    public string AddressFullName { get; set; }
    public string AddressPhoneNumber { get; set; }
    public string AddressAddressLine { get; set; }
    public string CreatedDateTime { get; set; }
    public bool PayFromWallet { get; set; }
    public int TotalPrice { get; set; }
    public int? DiscountPrice { get; set; }
    public byte TotalScore { get; set; }
    public byte ShippingCount { get; set; }
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

    public int? DiscountPrice { get; set; }

    public int Count { get; set; }

    public byte Score { get; set; }
    public string ProductPicture { get; set; }
    public int ProductVariantProductProductCode { get; set; }
    public string ProductVariantProductSlug { get; set; }
    public string ProductVariantVariantColorCode { get; set; }
    public bool? ProductVariantVariantIsColor { get; set; }
    public string ProductVariantVariantValue { get; set; }
}