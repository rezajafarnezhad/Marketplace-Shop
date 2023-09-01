using AutoMapper;
using AutoMapper.QueryableExtensions;
using DNTPersianUtils.Core;
using Microsoft.EntityFrameworkCore;
using ProShop.Common.Helpers;
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
        var data = await _mapper.ProjectTo<ShowProductVariantViewModel>(_productVariants.Where(c => c.ProductId == productId && c.SellerId == sellerId)).ToListAsync();
        return data;
    }

    public async Task<int> GetVariantCodeForCreateProductVariant()
    {
        var code = await _productVariants.OrderByDescending(c => c.Id).Select(c => c.VariantCode).FirstOrDefaultAsync();
        return code + 1;
    }

    public async Task<ShowProductVariantInCreateConsignmentViewModel> GetProductVariantForCreateConsignmet(int VariantCode)
    {
        var sellerId = await _sellerService.GetSellerId();
        return await _mapper.ProjectTo<ShowProductVariantInCreateConsignmentViewModel>(_productVariants.Where(c => c.SellerId == sellerId)).SingleOrDefaultAsync(c => c.VariantCode == VariantCode);
    }
    public async Task<bool> existsProductVariant(long productId, long garanteeId, long? variantId, long sellerId)
    {
        return await _productVariants.AnyAsync(c => c.ProductId == productId && c.GaranteeId == garanteeId && c.VariantId == variantId && c.SellerId == sellerId);
    }

    public async Task<List<GetProductVariantInCreateConsignmentViewModel>> GetProductVariantsForCreateConsignmet(List<int> variantCodes)
    {
        var sellerId = await _sellerService.GetSellerId();
        return await _mapper.ProjectTo<GetProductVariantInCreateConsignmentViewModel>(_productVariants.Where(c => variantCodes.Contains(c.VariantCode)).Where(c => c.SellerId == sellerId)).ToListAsync();
    }

    public async Task<List<ProductVariant>> GetProductVariantsToAddCount(List<long> ids)
    {
        return await _productVariants.Where(c => ids.Contains(c.Id)).ToListAsync();
    }

    public async Task<EditProductVariantViewModel> GetDateForEdit(long ProductVariantId)
    {
        var sellerId = await _sellerService.GetSellerId();

        return await _productVariants.AsNoTracking()
            .Where(c => c.SellerId == sellerId)
            .ProjectTo<EditProductVariantViewModel>(
            _mapper.ConfigurationProvider, parameters: new { now = DateTime.Now })
            .SingleOrDefaultAsync(c => c.Id == ProductVariantId);

    }

    public async Task<AddEditDiscountViewModel> GetDateForAddEditDiscount(long ProductVariantId)
    {
        var sellerId = await _sellerService.GetSellerId();

        var result =  await _mapper.ProjectTo<AddEditDiscountViewModel>(
            _productVariants.Where(c => c.SellerId == sellerId)
            ).SingleOrDefaultAsync(c => c.Id == ProductVariantId);

        if(result?.offPercentage >0)
        {
            var parsedDateTime = DateTime.Parse(result.StartDateTime);
            result.StartDateTimeEn = parsedDateTime.ToString("yyyy/MM/dd HH:mm");
            result.StartDateTime = parsedDateTime.ToShortPersianDateTime().ToPersianNumbers();

            var parsedDateTime2 = DateTime.Parse(result.EndDateTime);
            result.EndDateTimeEn = parsedDateTime2.ToString("yyyy/MM/dd HH:mm");
            result.EndDateTime = parsedDateTime2.ToShortPersianDateTime().ToPersianNumbers();
        }

        return result;
    }

    public async Task<ProductVariant> GetforEdit(long ProductVariantId)
    {
        var sellerId = await _sellerService.GetSellerId();

        return await _productVariants.Where(c => c.SellerId == sellerId).SingleOrDefaultAsync(c => c.Id == ProductVariantId);
    }

    public async Task<List<long>> GetAddedVariantsToProductVariants(List<long> VariantsIds, long categoryId)
    {
        // برای مثال این دسته بندی 3 رنگ دارد
        // از کدام یک از این رنگ ها در بخش تنوع محصولات استفاده شده
        // آیدی اون تنوع ها رو برگشت میزنیم
        // که به ادمین اجازه ندیم که اون تنوع هارو از این دسته بندی حذف کنه
        return await _productVariants
            .Where(c=>c.Product.MainCategoryId == categoryId)
            .Where(c => c.VariantId != null)
            .Where(c => VariantsIds.Contains(c.VariantId.Value))
            .GroupBy(c=>c.VariantId)
            .Select(c => c.First().VariantId.Value)
            .ToListAsync();
    }


}
