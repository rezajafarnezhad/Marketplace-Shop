using ProShop.Entities;
using ProShop.ViewModels.Product;

namespace ProShop.Services.Contracts;

public interface IProductService : IGenericService<Product>
{
    Task<List<string>> GetPersianTitlesForAutocomplete(string input);
    Task<ProductDetailsViewModel> GetProductDetails(long productId);
    Task<ShowProductsViewModel> GetProducts(ShowProductsViewModel model);
    Task<Entities.Product> GetProductToRemoveInManagingProducts(long id);
}