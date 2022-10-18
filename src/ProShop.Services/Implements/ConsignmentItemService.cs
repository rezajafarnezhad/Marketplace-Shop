using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;

namespace ProShop.Services.Implements;

public class ConsignmentItemService : GenericService<ConsignmentItem>, IConsignmentItemService
{

    private readonly DbSet<ConsignmentItem> _consignmentItem;
    private readonly IMapper _mapper;
    public ConsignmentItemService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _consignmentItem = uow.Set<Entities.ConsignmentItem>();
        _mapper = mapper;
    }

    public async Task<bool> IsExistsByProductVariantIdAndConsignmentId(long productvariantId,long consignmentId)
    {
        return await _consignmentItem.AsNoTracking().AnyAsync(c=>c.ProductVariantId == productvariantId && c.ConsignmentId == consignmentId);
    }

}
public class ProductStockService : GenericService<ProductStock>, IProductStockService
{

    private readonly DbSet<ProductStock> _productStocks;
    private readonly IMapper _mapper;
    public ProductStockService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _productStocks = uow.Set<Entities.ProductStock>();
        _mapper = mapper;
    }

    public async Task<ProductStock> GetByProductVariantIdAndConsignmentId(long productvariantId, long consignmentId)
    {
        return await _productStocks.SingleOrDefaultAsync(c => c.ProductVariantId == productvariantId && c.ConsignmentId == consignmentId);
    }

    public async Task<Dictionary<long,int>> GetProductStocksForAddProductVariantCount(long consignmentId)
    {

        return await _productStocks.Where(c => c.ConsignmentId == consignmentId)
            .ToDictionaryAsync(c=>c.ProductVariantId,c=>c.Count);
    }
}