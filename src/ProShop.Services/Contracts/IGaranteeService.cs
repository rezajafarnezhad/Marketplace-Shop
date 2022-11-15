using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using ProShop.ViewModels.Garantee;

public interface IGaranteeService : IGenericService<Garantee>
{
    Task<ShowGarantiesViewModel> GetGaranties(ShowGarantiesViewModel model);
    Task<Dictionary<long, string>> GetGaranteesForAddProductVaraint();
    List<ShowSelect2DataByAjaxViewModel> SearchOnGatanteesForSelect2Ajax(string input);
}