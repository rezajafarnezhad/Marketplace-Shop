using ProShop.Entities;
using ProShop.ViewModels.Product;
using ProShop.ViewModels.Veriants;

namespace ProShop.Services.Contracts;

public interface IProductService : IGenericService<Product>
{
    Task<ShowAllProductsInSellerPanelViewModel> GetAllProductsInSellerPanel(ShowAllProductsInSellerPanelViewModel model);
    Task<List<string>> GetPersianTitlesForAutocomplete(string input);
    Task<List<string>> GetPersianTitlesForAutocompleteInSellerPanel(string input);
    Task<int> GetProductCode();
    Task<ProductDetailsViewModel> GetProductDetails(long productId);
    Task<ShowProductsViewModel> GetProducts(ShowProductsViewModel model);
    Task<ShowProductsInSellerPanelViewModel> GetProductsInSellerPanel(ShowProductsInSellerPanelViewModel model);
    Task<Entities.Product> GetProductToRemoveInManagingProducts(long id);
    Task<AddVariantViewModel> GetProductInfoForAddVeriant(long productId);
    Task<ShowProductInfoViewModel> GetProductInfo(int productCode);
    Task<(int productCode, string slug)> FindByShortLink(string productShortLink);
    Task<List<Product>> GetProductsForChangeStatus(List<long> Ids);

    /// <summary>
    /// گرفتن محصولات برای نمایش در مقایسه محصول
    /// </summary>
    /// <param name="productCodes"></param>
    /// <returns></returns>
    Task<List<ShowProductInCompareViewModel>> GetProductCompare(params int[] productCodes);

    Task<ShowProductInComparePartialViewModel> GetProductsForAddProductInCompare(int pageNumber, string searchValue, int[] productCodeToHide);
    Task<long> GetProductCategory(long productCode);
}