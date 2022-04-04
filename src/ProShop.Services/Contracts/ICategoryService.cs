using ProShop.Entities;
using ProShop.ViewModels.Categories;

namespace ProShop.Services.Contracts;

public interface ICategoryService : IGenericService<Category>
{
    Task<ShowCategoriesViewModel> GetCategories(SearchCategoriesViewModel search);

}