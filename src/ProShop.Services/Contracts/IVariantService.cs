using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Veriants;

public interface IVariantService : IGenericService<Variant>
{
    Task<ShowVeriantsViewModel> GetVariants(ShowVeriantsViewModel model);

}