using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Brands;

public interface IBrandService : IGenericService<Brand>
{
    Task<ShowBrandsViewModel> GetBrands(ShowBrandsViewModel model);
    Task<EditBrandViewModel> GetForEdit(long Id);
}