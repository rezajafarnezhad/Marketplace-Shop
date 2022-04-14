using ProShop.Entities;
using ProShop.ViewModels.Categories;

namespace ProShop.Services.Contracts;

public interface ICategoryService : IGenericService<Category>
{
    Task<ShowCategoriesViewModel> GetCategories(ShowCategoriesViewModel model);
    Dictionary<long, string> GetCategoriesToShowInSelectBox();
    Task<EditCategoryViewModel> GetForEdit(long Id);
}