using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.FeatureConstantValue;

public interface IFeatureConstantValuesService: IGenericService<FeatureConstantValue>
{
    Task<bool> CheckConstantValue(long categoryId, List<long> featureIds);
    Task<bool> CheckNonConstantValue(long categoryId, List<long> featureIds);
    Task<ShowFeatureConstantValuesViewModel> GetFeatureConstants(ShowFeatureConstantValuesViewModel model);
    Task<List<ShowCategoryFeatureConstantValueViewModel>> GetFeatureConstantValuesByCategoryId(long categoryId);
    Task<List<FeatureConstantValueForCreateProductViewModel>> GetFeatureConstantValuesForCreateProduct(long categoryId);
}