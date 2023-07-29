using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.Common.Helpers;
using ProShop.DataLayer.Context;
using ProShop.DataLayer.Migrations;
using ProShop.Entities;
using ProShop.ViewModels.ProductShortLink;

namespace ProShop.Services.Implements;

public class ProductShortLinkService : GenericService<Entities.ProductShortLink>, IProductShortLinkService
{

    private readonly DbSet<Entities.ProductShortLink> _productShortLinks;
    private readonly IMapper _mapper;
    public ProductShortLinkService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _productShortLinks = uow.Set<Entities.ProductShortLink>();
    }
    public async Task<Entities.ProductShortLink> GetProductShortLinkForCreateProduct()
    {
        return await _productShortLinks.Where(c => !c.IsUsed).OrderBy(c => Guid.NewGuid()).FirstAsync();
    }

    public async Task<ShowProductShortLinksViewModel> GetAllProductShowLink(ShowProductShortLinksViewModel model)
    {

        var _sortLinks = _productShortLinks.AsNoTracking().AsQueryable();
        
        //search
        _sortLinks = _sortLinks.CreateSearchExpressions(model.shortLinkSearch, callDeletedStatusExpression: false);

        
       
        var paginationResult = await GenericPagination(_sortLinks, model.Pagination);

        return new ShowProductShortLinksViewModel()
        {
            shortLink = await _mapper.ProjectTo<ShowProductShortLinkViewModel>(paginationResult.Query).ToListAsync(),
            Pagination = paginationResult.Pagination,
        };

    }
}
