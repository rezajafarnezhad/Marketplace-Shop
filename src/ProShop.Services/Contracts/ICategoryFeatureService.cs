using ProShop.Entities;
using ProShop.ViewModels.Categories;
using ProShop.ViewModels.CategoryFeatures;

namespace ProShop.Services.Contracts;

public interface ICategoryFeatureService : IGenericService<CategoryFeature>
{

    Task<CategoryFeature> GetCategoryFeature(long featureId, long categoryId);
    Task<List<CategoryFeatureForCreateProductViewModel>> GetCategoryFeatures(long categoryId);
    Task<Dictionary<long, string>> GetCategoryFeatureBy(long categoryId);
    Task<bool> CheckCategoryFeature(long categoryId, long featureId);
    Task<bool> CheckCategoryFeaturesCount(long categoryId, List<long> featuresIds);
}
