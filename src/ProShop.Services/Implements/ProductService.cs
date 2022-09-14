using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.DataLayer.Migrations;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using ProShop.ViewModels.Product;

namespace ProShop.Services.Implements;

public class ProductService : GenericService<Product>, IProductService
{
    private readonly DbSet<Product> _products;
    private readonly IMapper _mapper;
    public ProductService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _products = uow.Set<Product>();
    }

    public async Task<List<string>> GetPersianTitlesForAutocomplete(string input)
    {
        return await _products.Where(c => c.PersianTitle.Contains(input))
            .Take(20)
            .Select(c => c.PersianTitle)
            .ToListAsync();
    }

    public async Task<ShowProductsViewModel> GetProducts(ShowProductsViewModel model)
    {
        var Products = _products.AsNoTracking().AsQueryable();

        #region Search Products


        var SearchShopName = model.SearchProducts.ShopName;
        if (!string.IsNullOrWhiteSpace(SearchShopName))
            Products = Products.Where(c => c.Seller.ShopName.Contains(SearchShopName.Trim()));

        var searchStatus = model.SearchProducts.Status;
        if (searchStatus is not null)
            Products = Products.Where(c => c.Status == searchStatus);

        Products = Products.CreateSearchExpressions(model.SearchProducts);
        #endregion

        #region sorting Productd

        var sorting = model.SearchProducts.Sorting;
        var IssortingAsc = model.SearchProducts.SortingOrder == SortingOrder.Asc;
        if (sorting == SortingProducts.ShopName)
        {
            if (IssortingAsc)
                Products = Products.OrderBy(c => c.Seller.ShopName);
            else
                Products = Products.OrderByDescending(c => c.Seller.ShopName);

        }
        else if (sorting == SortingProducts.BrandFa)
        {
            if (IssortingAsc)
                Products = Products.OrderBy(c => c.Brand.TitleFa);
            else
                Products = Products.OrderByDescending(c => c.Brand.TitleFa);
        }
        else
        {

            Products = Products.CreateOrderByExpression(model.SearchProducts.Sorting.ToString(),
                    model.SearchProducts.SortingOrder.ToString());
        }
        #endregion



        var paginationResult = await GenericPagination(Products, model.pagination);

        return new ShowProductsViewModel
        {
            Products = await _mapper.ProjectTo<ShowProductViewModel>(paginationResult.Query).ToListAsync(),
            pagination = paginationResult.Pagination,
        };

    }

    public Task<ProductDetailsViewModel> GetProductDetails(long productId)
    {
        return _mapper.ProjectTo<ProductDetailsViewModel>(_products.AsNoTracking().AsSplitQuery().Include(c=>c.ProductFeatures).ThenInclude(c=>c.Feature)).SingleOrDefaultAsync(c => c.Id == productId);
    }

    public async Task<Product> GetProductToRemoveInManagingProducts(long id)
    {

        return await _products.Where(c=>c.Status == Entities.ProductStatus.AwaitingInitialApproval)
            .Include(c=>c.ProductMedia)
            .SingleOrDefaultAsync(c => c.Id == id);

    }
}