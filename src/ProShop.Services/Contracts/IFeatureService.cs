using ProShop.Entities;
using ProShop.ViewModels.Features;

namespace ProShop.Services.Contracts;

public interface IFeatureService:IGenericService<Feature>
{
    Task<ShowFeaturesViewModel> GetCategoryFeatures(ShowFeaturesViewModel model);

    Task<Feature> FindByTitle(string title);

    Task<List<string>> AutocompleteSearch(string input);

}
