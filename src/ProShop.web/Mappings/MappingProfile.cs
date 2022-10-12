using AutoMapper;
using ProShop.Common.Helpers;
using ProShop.Entities;
using ProShop.Entities.Identity;
using ProShop.ViewModels.Brands;
using ProShop.ViewModels.Categories;
using ProShop.ViewModels.CategoryFeatures;
using ProShop.ViewModels.FeatureConstantValue;
using ProShop.ViewModels.Garantee;
using ProShop.ViewModels.Product;
using ProShop.ViewModels.ProductVariant;
using ProShop.ViewModels.Sellers;
using ProShop.ViewModels.Veriants;

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

        this.CreateMap<Entities.Seller, SellerDetailsViewModel>()
            .ForMember(dest => dest.CreatedDateTime, options => options.MapFrom(src => src.CreatedDateTime.ToLongPersianDate()));


        this.CreateMap<Entities.Brand, ShowBrandViewModel>();
        this.CreateMap<AddBrandViewModel, Entities.Brand>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        this.CreateMap<AddBrandBySelllerViewModel, Entities.Brand>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        this.CreateMap<Entities.Brand, EditBrandViewModel>().ReverseMap()
            .AddTransform<string>(str => str != null ? str.Trim() : null);


        this.CreateMap<AddCategoryViewModel, Entities.Category>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        this.CreateMap<Entities.Brand, BrandDetailsViewModel>();
        this.CreateMap<Entities.CategoryFeature, CategoryFeatureForCreateProductViewModel>();
        this.CreateMap<Entities.FeatureConstantValue, ShowFeatureConstantValueViewModel>();

        this.CreateMap<AddFeatureConstantValue, Entities.FeatureConstantValue>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        this.CreateMap<Entities.FeatureConstantValue, EditFeatureConstantValue>();

        this.CreateMap<EditFeatureConstantValue, Entities.FeatureConstantValue>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        this.CreateMap<Entities.FeatureConstantValue, ShowCategoryFeatureConstantValueViewModel>();

        this.CreateMap<AddProductViewModel, Product>()
             .ForMember(dest => dest.SpecialCheck, options => options.MapFrom(src => src.SpecialtyCheck))
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        this.CreateMap<Entities.FeatureConstantValue, FeatureConstantValueForCreateProductViewModel>();

        this.CreateMap<Product, ShowProductViewModel>()
             .ForMember(dest => dest.MainPicure, options => options.MapFrom(src => src.ProductMedia.First().FileName));

        this.CreateMap<Product, ShowProductInSellerPanelViewModel>()
             .ForMember(dest => dest.MainPicure, options => options.MapFrom(src => src.ProductMedia.First().FileName));

        this.CreateMap<Product, ShowAllProductInSellerPanelViewModel>()
             .ForMember(dest => dest.MainPicure, options => options.MapFrom(src => src.ProductMedia.First().FileName));

        this.CreateMap<Product, ProductDetailsViewModel>();
        this.CreateMap<ProductMedia, ProductMediaForDetailProductViewModel>();
        this.CreateMap<ProductFeature, ProductFeatureForDetailProductViewModel>();
        this.CreateMap<Entities.Variant, ShowVeriantViewModel>();
        this.CreateMap<Entities.Garantee, ShowGarantieeViewModel>();
        this.CreateMap<Entities.CategoryVarieant, ShowCategoryVariantInAddVariantViewModel>();

        this.CreateMap<Entities.Product, AddVariantViewModel>()
           .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Id))
           .ForMember(dest => dest.MainPicture, options => options.MapFrom(src => src.ProductMedia.First().FileName))
           .ForMember(dest => dest.ProductTitle, options => options.MapFrom(src => src.PersianTitle))
           .ForMember(dest => dest.Variants, options => options.MapFrom(src => src.Category.categoryVarieants))
           .ForMember(dest => dest.CommissionPercentage, options =>
             options.MapFrom(src => src.Category.CategoryBrands.Select(c => new { c.BrandId, c.CommissionPercentage }).Single(c => c.BrandId == src.BrandId).CommissionPercentage));


        this.CreateMap<AddVariantViewModel,Entities.ProductVariant>();
        this.CreateMap<Entities.ProductVariant,ShowProductVariantViewModel>()
             .ForMember(dest => dest.GatanteeFullTitle, options => options.MapFrom(src => src.Garantee.FullTitle));
        this.CreateMap<Entities.ProductVariant, ShowProductVariantInCreateConsignmentViewModel>();
        this.CreateMap<Entities.ProductVariant, GetProductVariantInCreateConsignmentViewModel>();




















    }
}