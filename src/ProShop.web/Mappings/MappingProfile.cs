using AutoMapper;
using ProShop.Common.Helpers;
using ProShop.Entities.Identity;
using ProShop.ViewModels.Brands;
using ProShop.ViewModels.Sellers;

namespace ProShop.web.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //this.CreateMap<string, string>()
        //    .ConvertUsing(str => str != null ? str.Trim() : null);

        this.CreateMap<User, CreateSellerViewModel>();

        this.CreateMap<CreateSellerViewModel, Entities.Seller>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        this.CreateMap<CreateSellerViewModel, Entities.Identity.User>()
            .AddTransform<string>(str => str != null ? str.Trim() : null)
            .ForMember(x => x.BirthDate,
            opt => opt.Ignore()); ;

        this.CreateMap<Entities.Seller, ShowSellerViewModel>()
            .ForMember(dest => dest.FullName, options => options.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.ProvinceAndCity, options => options.MapFrom(src => $"{src.Province.Title} - {src.City.Title}"))
            .ForMember(dest => dest.CreatedDateTime, options => options.MapFrom(src => src.CreatedDateTime.ToLongPersianDate()));

        this.CreateMap<Entities.Seller,SellerDetailsViewModel>()
            .ForMember(dest => dest.CreatedDateTime, options => options.MapFrom(src => src.CreatedDateTime.ToLongPersianDate()));


        this.CreateMap<Entities.Brand, ShowBrandViewModel>();
        this.CreateMap<AddBrandViewModel,Entities.Brand>()
            .AddTransform<string>(str => str != null ? str.Trim() : null); 
      
        this.CreateMap<Entities.Brand,EditBrandViewModel>().ReverseMap()
            .AddTransform<string>(str => str != null ? str.Trim() : null);
            
    }
}