using ProShop.Entities;
using System.Reflection.PortableExecutable;

namespace ProShop.ViewModels.Product;

public class ShowProductInfoViewModel
{
    public long Id { get; set; }
    public bool IsVariantTypeNull { get; set; }
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
    public long productQuestionsCount { get; set; }
    public string ProductShortLinkLink { get; set; }
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


    public string ShortDescription { get; set; }
    public string SpecialCheck { get; set; }
    public List<ProductMediaForProductInfoViewModel> ProductMedia { get; set; }
    public List<ProductCategoryForProductInfoViewModel> productCategories { get; set; }
    public List<ProductFeatureForProductInfoViewModel> ProductFeatures { get; set; } = new();
    public List<ProductVariantForProductInfoViewModel> ProductVariants { get; set; } = new();
    public List<ProductVariantInCartForProductInfoViewModel> ProductVariantInCart { get; set; } = new();
    public List<ProductCommentForProductInfoViewModel> productComments { get; set; } = new();
    public List<ProductQuestionsForProductInfoViewModel> ProductsQuestionsAndAnswers { get; set; }
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

public class ProductCommentForProductInfoViewModel
{
    public long Id { get; set; }
    public string ShopName { get; set; }
    public string CommentTitle { get; set; }
    public byte Score { get; set; }
    public string CommentText { get; set; }
    public string CreatedDateTime { get; set; }

    public bool IsBuyer { get; set; }

    public bool IsConfirmed { get; set; }
    public long Like { get; set; }
    public long DisLike { get; set; }
    public string Name { get; set; }

    public bool IsShop { get; set; }

    public string VariantValue { get; set; }
    public bool VariantIsColor { get; set; }
    public string VariantColorCode { get; set; }
}

public class ProductVariantForProductInfoViewModel
{
    public long Id { get; set; }
    public string VariantValue { get; set; }
    public string VariantColorCode { get; set; }
    public bool? VariantIsColor { get; set; }
    public int Price { get; set; }
    public int FinalPrice { get; set; }
    public byte? offPercentage { get; set; }
    public string SellerShopName { get; set; }
    public string SellerLogo { get; set; }
    public string GaranteeFullTitle { get; set; }
    public string EndDateTime { get; set; }
    public bool IsDiscountActive { get; set; }
    public byte Count { get; set; }
    public short MaxCountInCart { get; set; }
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


public class ProductVariantInCartForProductInfoViewModel
{
    public long ProductVariantId { get; set; }
    public short Count { get; set; }
}


/// <summary>
/// سوالای مخصول
/// </summary>
public class ProductQuestionsForProductInfoViewModel
{
    public string Body { get; set; }
    public List<ProductQuestionAnswerForProductInfoViewModel> Answers { get; set; }
}

/// <summary>
/// جواب های محصول
/// </summary>
public class ProductQuestionAnswerForProductInfoViewModel
{
    public string Body { get; set; }
    
    /// <summary>
    /// آبا این پاسخ توسط یک فروشگاه به ثبت رسبده
    /// </summary>
    public bool IsShop { get; set; }
    public bool IsBuyer { get; set; }
    public string Name { get; set; }
    public long Like { get; set; }
    public long Dislike{ get; set; }

}