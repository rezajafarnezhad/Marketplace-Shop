using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.ProductShortLink;

public interface IProductShortLinkService : IGenericService<ProductShortLink>
{
    Task<ShowProductShortLinksViewModel> GetAllProductShowLink(ShowProductShortLinksViewModel model);
    Task<ProductShortLink> GetProductShortLinkForCreateProduct();
}
