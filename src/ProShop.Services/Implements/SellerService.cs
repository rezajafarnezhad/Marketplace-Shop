using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using ProShop.ViewModels.Sellers;

namespace ProShop.Services.Implements;

public class SellerService : GenericService<Seller>, ISellerService
{
    private readonly DbSet<Seller> _sellers;
    private readonly IMapper _mapper;
    public SellerService(IUnitOfWork uow, IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _sellers = uow.Set<Seller>();
    }

    public override async Task<DuplicateColumns> AddAsync(Seller entity)
    {
        var result = new List<string>();

        if (await _sellers.AnyAsync(c => c.ShopName == entity.ShopName))
            result.Add(nameof(Seller.ShopName));

        if (await _sellers.AnyAsync(c => c.ShabaNumber == entity.ShabaNumber))
            result.Add(nameof(Seller.ShabaNumber));

        if (!result.Any())
            await base.AddAsync(entity);

        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };
    }

    public async Task<int> GetSellerCodeForCreateSeller()
    {
        var LastSeller = await _sellers.OrderByDescending(c => c.Id)
            .Select(c => c.SellerCode)
            .FirstOrDefaultAsync();

        return LastSeller + 1;
    }

    public async Task<ShowSellersViewModel> GetSellers(ShowSellersViewModel model)
    {
        var sellers = _sellers.AsNoTracking().AsQueryable();

        #region Searching

        string _ShopNameSearch = model.SearchSellers.ShopName;
        if (!string.IsNullOrWhiteSpace(_ShopNameSearch))
            sellers = sellers.Where(c => c.ShopName.Contains(_ShopNameSearch.Trim()));


        string _fullNameSearch = model.SearchSellers.FullName;
        if (!string.IsNullOrWhiteSpace(_fullNameSearch))
            sellers = sellers.Where(c => (c.User.FirstName + " " + c.User.LastName).Contains(_fullNameSearch.Trim()));

        string _SellerCodeSearch = model.SearchSellers.SellerCode.ToString();
        if (!string.IsNullOrWhiteSpace(_SellerCodeSearch))
            sellers = sellers.Where(c => c.SellerCode.ToString().Contains(_SellerCodeSearch.Trim()));


        switch (model.SearchSellers.IsActiveStatus)
        {
            case IsActiveStatus.Active:
                sellers = sellers.Where(c => c.IsActive);
                break;

            case IsActiveStatus.Disabled:
                sellers = sellers.Where(c => c.IsActive == false);
                break;
        }

        switch (model.SearchSellers.IsRealPersonStatus)
        {
            case IsRealPersonStatus.IsRealPerson:
                sellers = sellers.Where(c => c.IsRealPerson);
                break;

            case IsRealPersonStatus.IsLegalPerson:
                sellers = sellers.Where(c => c.IsRealPerson == false);
                break;
        }


        var _DocumentStatusSearch = model.SearchSellers.DocumentStatus;
        if (_DocumentStatusSearch != null)
        {
            sellers = sellers.Where(c => c.DocumentStatus == _DocumentStatusSearch);
        }

        #endregion

        #region Sorting

        if (model.SearchSellers.Sorting == SortingSellers.FullName)
        {

            if (model.SearchSellers.SortingOrder == SortingOrder.Asc)
            {
                sellers = sellers.OrderBy(c => c.User.FirstName + " " + c.User.LastName);
            }
            else
            {
                sellers = sellers.OrderByDescending(c => c.User.FirstName + " " + c.User.LastName);
            }
        }
        else if (model.SearchSellers.Sorting == SortingSellers.Province)
        {

            if (model.SearchSellers.SortingOrder == SortingOrder.Asc)
            {
                sellers = sellers.OrderBy(c => c.Province.Title);
            }
            else
            {
                sellers = sellers.OrderByDescending(c => c.Province.Title);
            }
        }
        else if (model.SearchSellers.Sorting == SortingSellers.City)
        {

            if (model.SearchSellers.SortingOrder == SortingOrder.Asc)
            {
                sellers = sellers.OrderBy(c => c.City.Title);
            }
            else
            {
                sellers = sellers.OrderByDescending(c => c.City.Title);
            }
        }
        else
        {
            sellers = sellers.CreateOrderByExpression(model.SearchSellers.Sorting.ToString(),
                model.SearchSellers.SortingOrder.ToString());
        }
        #endregion




        var paginationResult = await GenericPagination(sellers, model.Pagination);

        return new()
        {
            Sellers = await _mapper.ProjectTo<ShowSellerViewModel>(paginationResult.Query).ToListAsync(),
            Pagination = paginationResult.Pagination,
        };


    }

    public async Task<SellerDetailsViewModel> GetSellerDetails(long sellerId)
    {

        return await _mapper.ProjectTo<SellerDetailsViewModel>(_sellers).SingleOrDefaultAsync(c => c.Id == sellerId);

    }

    public async Task<Seller> GetSellerToRemoveInManagingSeller(long Id)
    {
        return await _sellers.Where(c => c.DocumentStatus == DocumentStatus.AwaitingInitialApproval)
            .Include(c=>c.User)
            .SingleOrDefaultAsync(c => c.Id == Id);
    }

    public async Task<long> GetSellerId(long userId)
    {
        return await _sellers.Where(c => c.UserId == userId).Select(c => c.Id).SingleAsync();
    }

    public async Task<List<string>> GetShopNameForAutocomplete(string input)
    {
        return await _sellers.Where(c => c.ShopName.Contains(input))
            .Take(20)
            .Select(c => c.ShopName)
            .ToListAsync();
    }
}