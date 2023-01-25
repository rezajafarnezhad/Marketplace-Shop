using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Veriants;

public interface IVariantService : IGenericService<Variant>
{
    Task<(bool result, bool isVariantNull)> checkProductAndVariantTypeForAddVariant(long productId, long variantId);
    Task<bool> CheckVariantsCountAddConfirmStatusForEditCategoryVariants(List<long> variantsIds, bool isColor);
    Task<ShowVeriantsViewModel> GetVariants(ShowVeriantsViewModel model);
    Task<List<ShowVariantInEditCategoryVariantViewModel>> GetVariantsForEditCategoryVariants(bool isColor);
}
