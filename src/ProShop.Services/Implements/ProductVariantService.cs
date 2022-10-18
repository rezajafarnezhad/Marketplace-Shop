using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.DataLayer.Migrations;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.ProductVariant;

namespace ProShop.Services.Implements;

public class ProductVariantService : GenericService<Entities.ProductVariant>, IProductVariantService
{

    private readonly DbSet<Entities.ProductVariant> _productVariants;
    private readonly IMapper _mapper;
    private readonly ISellerService _sellerService;
    public ProductVariantService(IUnitOfWork uow, IMapper mapper, ISellerService sellerService) : base(uow)
    {
        _productVariants = uow.Set<Entities.ProductVariant>();
        _mapper = mapper;
        _sellerService = sellerService;
    }


    public async Task<List<ShowProductVariantViewModel>> GetProductVariants(long productId)
    {
        var sellerId = await _sellerService.GetSellerId();
        var data = await _mapper.ProjectTo<ShowProductVariantViewModel>(_productVariants.Where(c=>c.ProductId == productId && c.SellerId ==sellerId)).ToListAsync();
        return data;
    }

    public async Task<int> GetVariantCodeForCreateProductVariant()
    {
        var code = await _productVariants.OrderByDescending(c => c.Id).Select(c => c.VariantCode).FirstOrDefaultAsync();
        return code + 1;
    }

    public async Task<ShowProductVariantInCreateConsignmentViewModel> GetProductVariantForCreateConsignmet (int VariantCode)
    {
        var sellerId = await _sellerService.GetSellerId();
        return await _mapper.ProjectTo<ShowProductVariantInCreateConsignmentViewModel>(_productVariants.Where(c => c.SellerId == sellerId)).SingleOrDefaultAsync(c=>c.VariantCode == VariantCode);
    }
    public async Task<bool> existsProductVariant(long productId,long garanteeId,long variantId)
    {
        return await _productVariants.AnyAsync(c => c.ProductId == productId && c.GaranteeId == garanteeId && c.VariantId == variantId);
    }

    public async Task<List<GetProductVariantInCreateConsignmentViewModel>> GetProductVariantsForCreateConsignmet(List<int> variantCodes)
    {
        var sellerId = await _sellerService.GetSellerId();
        return await _mapper.ProjectTo<GetProductVariantInCreateConsignmentViewModel>(_productVariants.Where(c => variantCodes.Contains(c.VariantCode)).Where(c=>c.SellerId == sellerId)).ToListAsync();
    }

    public async Task<List<ProductVariant>> GetProductVariantsToAddCount(List<long> ids)
    {
        return await _productVariants.Where(c => ids.Contains(c.Id)).ToListAsync();
    }

   
}
