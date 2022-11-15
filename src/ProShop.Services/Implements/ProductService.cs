using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using ProShop.ViewModels.Product;
using ProShop.ViewModels.Veriants;

namespace ProShop.Services.Implements;

public class ProductService : GenericService<Product>, IProductService
{
    private readonly DbSet<Product> _products;
    private readonly ISellerService _sellerService;
    private readonly IHttpContextAccessor _httpContext;

    private readonly IMapper _mapper;
    public ProductService(IUnitOfWork uow, IMapper mapper, ISellerService sellerService, IHttpContextAccessor httpContext) : base(uow)
    {
        _mapper = mapper;
        _products = uow.Set<Product>();
        _sellerService = sellerService;
        _httpContext = httpContext;
    }

    public async Task<List<string>> GetPersianTitlesForAutocomplete(string input)
    {
        return await _products.Where(c => c.PersianTitle.Contains(input))
            .Take(20)
            .Select(c => c.PersianTitle)
            .ToListAsync();
    }
    public async Task<List<string>> GetPersianTitlesForAutocompleteInSellerPanel(string input)
    {
        var userid = _httpContext.HttpContext.User.Identity.GetLoggedUserId();
        var SellerId = await _sellerService.GetSellerId(userid);

        return await _products.Where(c => c.PersianTitle.Contains(input) && c.SelerId == SellerId)
            .AsNoTracking()
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

    public async Task<ShowProductsInSellerPanelViewModel> GetProductsInSellerPanel(ShowProductsInSellerPanelViewModel model)
    {
        
        var SellerId = await _sellerService.GetSellerId();
        var Products = _products.AsNoTracking().Where(c => c.SelerId == SellerId || c.ProductVariants.Any(c=>c.SellerId == SellerId)).AsQueryable();
        #region Search Products

        var searchStatus = model.SearchProducts.Status;
        if (searchStatus is not null)
            Products = Products.Where(c => c.Status == searchStatus);


        Products = Products.CreateSearchExpressions(model.SearchProducts);

        #endregion

        #region sorting Productd

        var sorting = model.SearchProducts.Sorting;
        var IssortingAsc = model.SearchProducts.SortingOrder == SortingOrder.Asc;
        if (sorting == SortingProductsInSellerPanel.BrandFa)
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


        return new ShowProductsInSellerPanelViewModel()
        {

            Products = await _mapper.ProjectTo<ShowProductInSellerPanelViewModel>(paginationResult.Query).ToListAsync(),
            pagination = paginationResult.Pagination,

        };
    }

    public async Task<ShowAllProductsInSellerPanelViewModel> GetAllProductsInSellerPanel(ShowAllProductsInSellerPanelViewModel model)
    {
        var Products = _products.AsNoTracking().Where(c => c.Status == ProductStatus.Confirmed).AsQueryable();
        #region Search Products



        Products = Products.CreateSearchExpressions(model.SearchProducts);

        #endregion

        #region sorting Productd

        var sorting = model.SearchProducts.Sorting;
        var IssortingAsc = model.SearchProducts.SortingOrder == SortingOrder.Asc;
        if (sorting == SortingAllProductsInSellerPanel.BrandFa)
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


        return new ShowAllProductsInSellerPanelViewModel()
        {

            Products = await _mapper.ProjectTo<ShowAllProductInSellerPanelViewModel>(paginationResult.Query).ToListAsync(),
            pagination = paginationResult.Pagination,

        };
    }


    public Task<ProductDetailsViewModel> GetProductDetails(long productId)
    {
        return _mapper.ProjectTo<ProductDetailsViewModel>
            (_products.AsNoTracking().AsSplitQuery()).SingleOrDefaultAsync(c => c.Id == productId);
    }

    public async Task<Product> GetProductToRemoveInManagingProducts(long id)
    {

        return await _products.Where(c => c.Status == Entities.ProductStatus.AwaitingInitialApproval)
            .Include(c => c.ProductMedia)
            .SingleOrDefaultAsync(c => c.Id == id);

    }

    public async Task<int> GetProductCode()
    {
        var LastProductCode = await _products.OrderByDescending(c => c.ProductCode).Select(c => c.ProductCode).FirstOrDefaultAsync();
        return LastProductCode + 1;
    }
    
    public async Task<AddVariantViewModel> GetProductInfoForAddVeriant(long productId)
    {
        return await _mapper.ProjectTo<AddVariantViewModel>(_products).SingleOrDefaultAsync(c => c.ProductId == productId);
    }
    public Task<ShowProductInfoViewModel> GetProductInfo(int productCode)
    {
        var userid = _httpContext.HttpContext.User.Identity.GetLoggedUserId();
        return _products.AsNoTracking().AsSplitQuery().ProjectTo<ShowProductInfoViewModel>
            (_mapper.ConfigurationProvider, new { userid = userid }).SingleOrDefaultAsync(c => c.ProductCode == productCode);

    }
}