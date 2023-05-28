using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProShop.Services.Implements;

public class AddressService : GenericService<Entities.Address>, IAddressService
{
    private readonly DbSet<Entities.Address> _address;
    private readonly IMapper _mapper;
    public AddressService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _address = uow.Set<Entities.Address>();
    }

    public async Task<AddressInCheckoutPageInViewModel> GetAddressForCheckoutPage(long userId)
    {
        return await _mapper.ProjectTo<AddressInCheckoutPageInViewModel>(_address.Where(c=>c.UserId == userId)).FirstAsync();
    }

}
