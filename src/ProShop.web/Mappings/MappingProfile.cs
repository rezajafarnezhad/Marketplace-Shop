using AutoMapper;
using ProShop.Entities.Identity;
using ProShop.ViewModels.Sellers;

namespace ProShop.web.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<string, string>()
            .ConvertUsing(str => str != null ? str.Trim() : null);
        this.CreateMap<User, CreateSellerViewModel>();
        this.CreateMap<CreateSellerViewModel, Entities.Seller>();
        this.CreateMap<CreateSellerViewModel, Entities.Identity.User>().ForMember(x => x.BirthDate,
            opt => opt.Ignore()); ;
    }
}