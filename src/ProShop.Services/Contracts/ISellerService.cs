using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface ISellerService : IGenericService<Seller>
{
    Task<int> GetSellerCodeForCreateSeller();
}