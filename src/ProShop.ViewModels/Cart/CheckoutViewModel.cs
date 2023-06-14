
using ProShop.Entities;

namespace ProShop.ViewModels.Cart
{

    public class CheckoutViewModel
    {
        public AddressInCheckoutPageInViewModel UserAddress { get; set; }
        public List<ShowCartInChackoutPageViewModel> CartItems { get; set; }
    }

    public class AddressInCheckoutPageInViewModel
    {
        public string FullName { get; set; }
        public string AddressLine { get; set; }
        public string ProvinceTitle { get; set; }
        public string CityTitle { get; set; }

    }

    public class ShowCartInDropDownViewModel
    {
        public string ProductVariantProductPersianTitle { get; set; }
        public bool IsDiscountActive { get; set; }
        public int ProductVariantPrice { get; set; }
        public int? ProductVariantOffPrice { get; set; }
        public string ProductVariantVariantColorCode { get; set; }
        public bool? ProductVariantVariantIsColor { get; set; }
        public string ProductVariantVariantValue { get; set; }
        public short Count { get; set; }
        public string ProductPicture { get; set; }
        public int ProductVariantCount { get; set; }
        public short ProductVariantMaxCountInCart { get; set; }
        public long ProductVariantId { get; set; }
    }

    public class ShowCartInCartPageViewModel : ShowCartInDropDownViewModel
    {
        public string ProductVariantGaranteeFullTitle { get; set; }
        public string ProductVariantSellerShopName { get; set; }
        public byte ProductVariantCount2 { get; set; }
        public int ProductVariantProductProductCode { get; set; }
        public int ProductVariantProductProductSlug { get; set; }
        public byte Score
        {
            get
            {
                var result = (ProductVariantPrice * Count) / 10000;
                if (result <= 1)
                    return 1;
                if (result >= 150)
                    return 150;
                return (byte)result;

            }
        }
    }



    public class ShowCartInChackoutPageViewModel : ShowCartInCartPageViewModel
    {
        public ProductDimensions ProductVariantProductDimensions { get; set; }
    }

}

