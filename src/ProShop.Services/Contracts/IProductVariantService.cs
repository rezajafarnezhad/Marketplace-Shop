using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.ProductVariant;

public interface IProductVariantService : IGenericService<ProductVariant>
{
    Task<bool> existsProductVariant(long productId, long garanteeId, long variantId,long sellerId);
    Task<ShowProductVariantInCreateConsignmentViewModel> GetProductVariantForCreateConsignmet(int VariantCode);
    Task<List<ShowProductVariantViewModel>> GetProductVariants(long productId);
    Task<List<GetProductVariantInCreateConsignmentViewModel>> GetProductVariantsForCreateConsignmet(List<int> variantCodes);
    Task<int> GetVariantCodeForCreateProductVariant();
    Task<List<ProductVariant>> GetProductVariantsToAddCount(List<long> ids);
    Task<EditProductVariantViewModel> GetDateForEdit(long ProductVariantId);
    Task<AddEditDiscountViewModel> GetDateForAddEditDiscount(long ProductVariantId);
    Task<ProductVariant> GetforEdit(long ProductVariantId);
}
