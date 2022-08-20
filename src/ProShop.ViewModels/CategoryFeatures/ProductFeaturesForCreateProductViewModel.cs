using ProShop.ViewModels.FeatureConstantValue;

namespace ProShop.ViewModels.CategoryFeatures;

public class ProductFeaturesForCreateProductViewModel
{
    
    public List<ShowCategoryFeatureConstantValueViewModel> FeaturesConstantValues { get; set; }
    public List<CategoryFeatureForCreateProductViewModel> Features { get; set; }
}