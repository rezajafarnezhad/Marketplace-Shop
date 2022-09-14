using ProShop.Entities;
using ProShop.ViewModels.Sellers;

namespace ProShop.Services.Contracts;

public interface ISellerService : IGenericService<Seller>
{
    Task<int> GetSellerCodeForCreateSeller();
    Task<ShowSellersViewModel> GetSellers(ShowSellersViewModel model);
    Task<SellerDetailsViewModel> GetSellerDetails(long sellerId);
    Task<Seller> GetSellerToRemoveInManagingSeller(long Id);
    Task<long> GetSellerId(long userId);
    Task<List<string>> GetShopNameForAutocomplete(string input);
}
