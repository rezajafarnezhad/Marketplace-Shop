using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Veriants;

public interface IVariantService : IGenericService<Variant>
{
    Task<bool> checkProductAndVariantTypeForAddVariant(long productId, long variantId);
    Task<ShowVeriantsViewModel> GetVariants(ShowVeriantsViewModel model);
    Task<List<ShowVariantInEditCategoryVariantViewModel>> GetVariantsForEditCategoryVariants(bool isColor);
}
