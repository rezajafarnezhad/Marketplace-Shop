using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.ProductVariant;

public interface IProductVariantService : IGenericService<ProductVariant>
{
    Task<bool> existsProductVariant(long productId, long garanteeId, long variantId);
    Task<ShowProductVariantInCreateConsignmentViewModel> GetProductVariantForCreateConsignmet(int VariantCode);
    Task<List<ShowProductVariantViewModel>> GetProductVariants(long productId);
    Task<List<GetProductVariantInCreateConsignmentViewModel>> GetProductVariantsForCreateConsignmet(List<int> variantCodes);
    Task<int> GetVariantCodeForCreateProductVariant();
}
