using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface IProductStockService : IGenericService<ProductStock>
{
    Task<ProductStock> GetByProductVariantIdAndConsignmentId(long productvariantId, long consignmentId);
    Task<Dictionary<long, int>> GetProductStocksForAddProductVariantCount(long consignmentId);
}