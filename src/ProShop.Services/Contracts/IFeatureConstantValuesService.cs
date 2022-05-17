using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.FeatureConstantValue;

public interface IFeatureConstantValuesService: IGenericService<FeatureConstantValue>
{
    Task<ShowFeatureConstantValuesViewModel> GetFeatureConstants(ShowFeatureConstantValuesViewModel model);
}