using ProShop.Entities;
using ProShop.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProShop.Services.Contracts;

public interface IAddressService : IGenericService<Address>
{
    Task<AddressInCheckoutPageInViewModel> GetAddressForCheckoutPage(long userId);
}
