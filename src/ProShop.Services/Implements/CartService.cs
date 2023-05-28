using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.Services.Implements;
using ProShop.ViewModels.Cart;
using ProShop.ViewModels.Product;

public class CartService : GenericService<Cart>, ICartService
{
    private readonly DbSet<Cart> _carts;
    private readonly IMapper _mapper;
    public CartService(IUnitOfWork uow, IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _carts = uow.Set<Cart>();
    }

    public async Task<List<ProductVariantInCartForProductInfoViewModel>> GetProductVariantsInCart(List<long> productVariantsIds, long userId)
    {
        return await _mapper.ProjectTo<ProductVariantInCartForProductInfoViewModel>
            (_carts.Where(c => c.UserId == userId).Where(c => productVariantsIds.Contains(c.ProductVaraintId))).ToListAsync();
    }

    public async Task<List<ShowCartInDropDownViewModel>> GetCartForDropDown(long userId)
    {
        return await _carts.AsNoTracking().Where(c => c.UserId == userId)
            .ProjectTo<ShowCartInDropDownViewModel>(configuration: _mapper.ConfigurationProvider, parameters: new { now=DateTime.Now}).ToListAsync();
    }
    
    public async Task<List<ShowCartInCartPageViewModel>> GetCartForCartPage(long userId)
    {
        return await _carts.AsNoTracking().Where(c => c.UserId == userId)
            .ProjectTo<ShowCartInCartPageViewModel>(configuration: _mapper.ConfigurationProvider, parameters: new { now=DateTime.Now}).ToListAsync();
    }

    public async Task<List<Cart>> GetAllCartItems(long userId)
    {

       return await _carts.AsNoTracking().Where(c => c.UserId == userId).ToListAsync();
    }


    public async Task<List<ShowCartInChackoutPageViewModel>> GetCartsForCheckoutPage(long userId)
    {

        return await _carts.AsNoTracking().Where(c => c.UserId == userId)
            .ProjectTo<ShowCartInChackoutPageViewModel>(configuration: _mapper.ConfigurationProvider, parameters: new { now = DateTime.Now})
            .ToListAsync();

    }

}