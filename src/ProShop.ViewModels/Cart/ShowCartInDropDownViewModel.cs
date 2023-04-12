using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProShop.ViewModels.Cart;

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
    public byte Score
    {
        get
        {
            var result = ProductVariantPrice / 10000;
            if (result <= 1)
                return 1;
            if (result >= 150)
                return 150;
            return (byte)result;

        }
    }
}

public class ShowCartInChackoutPage : ShowCartInCartPageViewModel
{

}
