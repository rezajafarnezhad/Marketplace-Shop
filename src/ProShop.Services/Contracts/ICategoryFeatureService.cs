using ProShop.Entities;
using ProShop.ViewModels.Categories;

namespace ProShop.Services.Contracts;

public interface ICategoryFeatureService : IGenericService<CategoryFeature>
{

    Task<CategoryFeature> GetCategoryFeature(long featureId, long categoryId);
}