using System.Reflection.PortableExecutable;

namespace ProShop.ViewModels.Product;

public class ShowProductInfoViewModel
{
    public long Id { get; set; }
    public int ProductCode { get; set; }
    public string Slug { get; set; }
    public string PersianTitle { get; set; }
    public string EnglishTitle { get; set; }
    public string BrandTitleFa { get; set; }
    public string CategoryTitle { get; set; }
    public string CategoryProductPageGuid { get; set; }
    public string BrandLogoPicture { get; set; }
    public double Score { get; set; }
    public long productCommentsLongCount { get; set; }
    public long productCommentsCount { get; set; }
    public long SuggestCount { get; set; }
    public long BuyerCount { get; set; }
    public bool isFavorite { get; set; }

    public double SuggestPercentage
    {
        get
        {
            var percentage = (double)BuyerCount / SuggestCount;
            return 100 / percentage;
        }
    }

    public List<ProductMediaForProductInfoViewModel> ProductMedia { get; set; }
    public List<ProductCategoryForProductInfoViewModel> productCategories { get; set; }
    public List<ProductFeatureForProductInfoViewModel> ProductFeatures { get; set; }
    public List<ProductVariantForProductInfoViewModel> ProductVariants { get; set; }

}

public class ProductMediaForProductInfoViewModel
{
    public string FileName { get; set; }
    public bool IsVideo { get; set; }
}

public class ProductCategoryForProductInfoViewModel
{
    public string CategoryTitle { get; set; }
    public string CategorySlug { get; set; }
}

public class ProductFeatureForProductInfoViewModel
{
    public string Value { get; set; }
    public string FeatureTitle { get; set; }
    public bool FeatureShowNextToProduct { get; set; }

}


public class ProductVariantForProductInfoViewModel
{
    public string VariantValue { get; set; }
    public string VariantColorCode { get; set; }
    public bool VariantIsColor { get; set; }
    public int Price { get; set; }
    public string SellerShopName { get; set; }
    public string SellerLogo { get; set; }
    public string GaranteeFullTitle { get; set; }
    public byte Score
    {
        get
        {
            var result = Price / 10000;
            if (result <= 1)
                return 1;
            if (result >= 150)
                return 150;
            return (byte)result;

        }
    }

}


