using AutoMapper;
using ProShop.Common.Helpers;
using ProShop.Entities;
using ProShop.Entities.Identity;
using ProShop.ViewModels.Brands;
using ProShop.ViewModels.Categories;
using ProShop.ViewModels.CategoryFeatures;
using ProShop.ViewModels.Consignments;
using ProShop.ViewModels.FeatureConstantValue;
using ProShop.ViewModels.Garantee;
using ProShop.ViewModels.Product;
using ProShop.ViewModels.ProductShortLink;
using ProShop.ViewModels.ProductStock;
using ProShop.ViewModels.ProductVariant;
using ProShop.ViewModels.Sellers;
using ProShop.ViewModels.Veriants;
using ProShop.ViewModels.Cart;
using ProShop.ViewModels.Orders;

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


        this.CreateMap<AddVariantInPanelAdmin, Entities.Variant>();

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
        this.CreateMap<Entities.ProductShortLink, ShowProductShortLinkViewModel>();

        this.CreateMap<Entities.Product, AddVariantViewModel>()
           .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Id))
           .ForMember(dest => dest.MainPicture, options => options.MapFrom(src => src.ProductMedia.First().FileName))
           .ForMember(dest => dest.ProductTitle, options => options.MapFrom(src => src.PersianTitle))
           .ForMember(dest => dest.Variants, options => options.MapFrom(src => src.Category.categoryVarieants.Where(c => c.Variant.IsConfirmed)))
           .ForMember(dest => dest.CommissionPercentage, options =>
             options.MapFrom(src => src.Category.CategoryBrands.Select(c => new { c.BrandId, c.CommissionPercentage }).Single(c => c.BrandId == src.BrandId).CommissionPercentage));


        this.CreateMap<AddVariantViewModel, Entities.ProductVariant>();
        this.CreateMap<Entities.ProductVariant, ShowProductVariantViewModel>()
            .ForMember(dest => dest.StartDateTime, option => option.MapFrom(src => src.StartDateTime != null ? src.StartDateTime.Value.ToLongPersianDate() : null))
            .ForMember(dest => dest.EndDateTime, option => option.MapFrom(src => src.EndDateTime != null ? src.EndDateTime.Value.ToLongPersianDate() : null))
             .ForMember(dest => dest.GatanteeFullTitle, options => options.MapFrom(src => src.Garantee.FullTitle));
        this.CreateMap<Entities.ProductVariant, ShowProductVariantInCreateConsignmentViewModel>();
        this.CreateMap<Entities.ProductVariant, GetProductVariantInCreateConsignmentViewModel>();

        this.CreateMap<Entities.Consignment, ShowConsignmentViewModel>()
            .ForMember(dest => dest.DeliveryDate, options => options.MapFrom(src => src.DeliveryDate.ToLongPersianDate()));


        this.CreateMap<Entities.ConsignmentItem, ShowConsignmentItemsViewModel>();

        this.CreateMap<AddGarantee, Entities.Garantee>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);




        long consignmentId = 0;
        this.CreateMap<Entities.Consignment, ShowConsignmentDetailsViewModel>()
             .ForMember(dest => dest.DeliveryDate,
                options =>
                    options.MapFrom(src => src.DeliveryDate.ToLongPersianDate()))
             .ForMember(dest => dest.ConsignmentItems,
            options =>
                options.MapFrom(src => src.ConsignmentItems.Where(x => x.ConsignmentId == consignmentId)));

        this.CreateMap<Entities.ConsignmentItem, ShowConsignmentItemsViewModel>();
        this.CreateMap<AddProductStockByConsignmentViewModel, Entities.ProductStock>();


        var userid = 0;
        this.CreateMap<Entities.Product, ShowProductInfoViewModel>()

            .ForMember(dest => dest.Score,
                options =>
                    options.MapFrom(src => src.productComments.Any() ? src.productComments.Average(c => c.Score) : 0))

            .ForMember(dest => dest.productCommentsCount,
                options =>
                    options.MapFrom(src => src.productComments.LongCount(c => c.CommentTitle != null)))

            .ForMember(dest => dest.SuggestCount,
                options =>
                    options.MapFrom(src => src.productComments.Where(c => c.IsBuyer).LongCount(c => c.Suggest == true)))

            .ForMember(dest => dest.BuyerCount,
                options =>
                    options.MapFrom(src => src.productComments.LongCount(c => c.IsBuyer == true)))


            .ForMember(dest => dest.ProductVariants,
                options =>
                    options.MapFrom(src => src.ProductVariants.Where(c => c.Count > 0)))

            .ForMember(dest => dest.IsVariantTypeNull,
                options =>
                    options.MapFrom(src => src.Category.IsVariantColor == null))

            .ForMember(dest => dest.isFavorite,
                options =>
                    options.MapFrom(src => userid != 0 ? src.UserProductFavorites.Any(c => c.UserId == userid) : false));




        this.CreateMap<ProductMedia, ProductMediaForProductInfoViewModel>();
        this.CreateMap<ProductCategory, ProductCategoryForProductInfoViewModel>();
        this.CreateMap<ProductFeature, ProductFeatureForProductInfoViewModel>();


        DateTime now = default;
        this.CreateMap<Entities.ProductVariant, ProductVariantForProductInfoViewModel>()
            .ForMember(dest => dest.EndDateTime,
                options =>
                    options.MapFrom(src =>
                        src.EndDateTime != null ? src.EndDateTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : null
                    ))

            .ForMember(dest => dest.Count,
                options =>
                    options.MapFrom(src =>
                        src.Count > 5 ? (byte)0 : (byte)src.Count
                    ))


            .ForMember(dest => dest.IsDiscountActive,
                options =>
                    options.MapFrom(src =>
                        src.offPercentage != null && (src.StartDateTime <= now && src.EndDateTime >= now)
                    ));

        this.CreateMap<ProductVariant, EditProductVariantViewModel>()
          .ForMember(dest => dest.CategoryIsVariantColor, options => options.MapFrom(src => src.Product.Category.IsVariantColor))
           .ForMember(dest => dest.MainPicture, options => options.MapFrom(src => src.Product.ProductMedia.First().FileName))
           .ForMember(dest => dest.ProductTitle, options => options.MapFrom(src => src.Product.PersianTitle))
           .ForMember(dest => dest.IsDiscountActive, options => options.MapFrom(src => src.offPercentage != null && (src.StartDateTime <= now && src.EndDateTime >= now)))
           .ForMember(dest => dest.CommissionPercentage, options =>
             options.MapFrom(src => src.Product.Category.CategoryBrands.Select(c => new { c.BrandId, c.CommissionPercentage }).Single(c => c.BrandId == src.Product.BrandId).CommissionPercentage));
        ;


        this.CreateMap<AddEditDiscountViewModel, Entities.ProductVariant>()
            .ForMember(x => x.Price, opt => opt.Ignore())
            .ForMember(x => x.StartDateTime, opt => opt.Ignore())
            .ForMember(x => x.EndDateTime, opt => opt.Ignore())
            ;


        this.CreateMap<ProductVariant, AddEditDiscountViewModel>()

          .ForMember(dest => dest.MainPicture, options => options.MapFrom(src => src.Product.ProductMedia.First().FileName))
          .ForMember(dest => dest.ProductTitle, options => options.MapFrom(src => src.Product.PersianTitle))
          .ForMember(dest => dest.CategoryIsVariantColor, options => options.MapFrom(src => src.Product.Category.IsVariantColor))
          .ForMember(dest => dest.CommissionPercentage, options =>
            options.MapFrom(src => src.Product.Category.CategoryBrands.Select(c => new { c.BrandId, c.CommissionPercentage }).Single(c => c.BrandId == src.Product.BrandId).CommissionPercentage));
        ;



        this.CreateMap<Entities.Variant, ShowVariantInEditCategoryVariantViewModel>();
        this.CreateMap<Entities.Cart, ProductVariantInCartForProductInfoViewModel>();



        this.CreateMap<Cart, ShowCartInDropDownViewModel>()
            .ForMember(dest => dest.ProductPicture, options => options.MapFrom(src => src.ProductVariant.Product.ProductMedia.First().FileName))
            .ForMember(dest => dest.IsDiscountActive,
                options =>
                    options.MapFrom(src =>
                        src.ProductVariant.offPercentage != null && (src.ProductVariant.StartDateTime <= now && src.ProductVariant.EndDateTime >= now)))
            ;


        this.CreateMap<Cart, ShowCartInCartPageViewModel>()
            .ForMember(dest => dest.ProductPicture, options => options.MapFrom(src => src.ProductVariant.Product.ProductMedia.First().FileName))
            .ForMember(dest => dest.ProductVariantCount2,
                options =>
                    options.MapFrom(src =>
                        src.ProductVariant.Count > 5 ? (byte)0 : (byte)src.ProductVariant.Count
                    ))
            .ForMember(dest => dest.IsDiscountActive,
                options =>
                    options.MapFrom(src =>
                        src.ProductVariant.offPercentage != null && (src.ProductVariant.StartDateTime <= now && src.ProductVariant.EndDateTime >= now)))
            ;


        this.CreateMap<Cart, ShowCartInChackoutPageViewModel>()
            .ForMember(dest => dest.ProductPicture, options => options.MapFrom(src => src.ProductVariant.Product.ProductMedia.First().FileName))
            .ForMember(dest => dest.ProductVariantCount2,
                options =>
                    options.MapFrom(src =>
                        src.ProductVariant.Count > 5 ? (byte)0 : (byte)src.ProductVariant.Count
                    ))
            .ForMember(dest => dest.IsDiscountActive,
                options =>
                    options.MapFrom(src =>
                        src.ProductVariant.offPercentage != null && (src.ProductVariant.StartDateTime <= now && src.ProductVariant.EndDateTime >= now)))
            ;

        this.CreateMap<Cart, ShowCartInPeymentPageViewModel>()
            .ForMember(dest => dest.ProductPicture, options => options.MapFrom(src => src.ProductVariant.Product.ProductMedia.First().FileName))
            .ForMember(dest => dest.ProductVariantCount2,
                options =>
                    options.MapFrom(src =>
                        src.ProductVariant.Count > 5 ? (byte)0 : (byte)src.ProductVariant.Count
                    ))
            .ForMember(dest => dest.IsDiscountActive,
                options =>
                    options.MapFrom(src =>
                        src.ProductVariant.offPercentage != null && (src.ProductVariant.StartDateTime <= now && src.ProductVariant.EndDateTime >= now)))
            ;


        this.CreateMap<Cart, ShowCartForCreateOrderAndPayViewModel>()
            .ForMember(dest => dest.ProductPicture, options => options.MapFrom(src => src.ProductVariant.Product.ProductMedia.First().FileName))
            .ForMember(dest => dest.ProductVariantCount2,
                options =>
                    options.MapFrom(src =>
                        src.ProductVariant.Count > 5 ? (byte)0 : (byte)src.ProductVariant.Count
                    ))
            .ForMember(dest => dest.IsDiscountActive,
                options =>
                    options.MapFrom(src =>
                        src.ProductVariant.offPercentage != null && (src.ProductVariant.StartDateTime <= now && src.ProductVariant.EndDateTime >= now)))
            ;

        this.CreateMap<Entities.Address, AddressInCheckoutPageInViewModel>();


        this.CreateMap<Order, ShowOrder>()
            .ForMember(dest => dest.CreatedDateTime,
                options =>
                    options.MapFrom(src => src.CreatedDateTime.ToLongPersianDatewithHour()))


            .ForMember(dest => dest.Destination,
                options =>
                    options.MapFrom(src => src.Address.Province.Title + " - " + src.Address.City.Title))
            ;



        this.CreateMap<Order, OrderDetailsViewModel>()
            .ForMember(dest => dest.CreatedDateTime,
                options =>
                    options.MapFrom(src => src.CreatedDateTime.ToLongPersianDatewithHour()))



            ;


        this.CreateMap<Entities.ParcalPost, ParcelPostForOrderDetailsViewModel>();
        this.CreateMap<Entities.ParcelPostItem, ParcelPostItemForOrderDetailsViewModel>()
            .ForMember(dest => dest.ProductPicture, options
                => options.MapFrom(src => src.ProductVariant.Product.ProductMedia.First().FileName))
            ;

        this.CreateMap<Order, ShowOrderInDeliveryOrderViewModel>()
            .ForMember(dest => dest.CreatedDateTime,
                options =>
                    options.MapFrom(src => src.CreatedDateTime.ToLongPersianDatewithHour()))


            .ForMember(dest => dest.Destination,
                options =>
                    options.MapFrom(src => src.Address.Province.Title + " - " + src.Address.City.Title))

            .ForMember(dest => dest.ParcelPostsCountInPost,
                options =>
                    options.MapFrom(src => src.ParcalPosts.Count(c => c.ParcelPostStatus == ParcelPostStatus.DeliveredToPost || c.ParcelPostStatus == ParcelPostStatus.DeliveredToClient)))
            ;


        this.CreateMap<Entities.ParcalPost, ShowParcelPostInDeliveryOrdersViewModel>();

        this.CreateMap<Entities.Product, ShowProductInCompareViewModel>()

            .ForMember(dest => dest.MainPicure, options => options.MapFrom(src => src.ProductMedia.First().FileName))
            .ForMember(dest => dest.Price, options => options.MapFrom(src =>
                src.ProductStockStatustus == ProductStockStatus.Available ? src.ProductVariants.Any() ? src.ProductVariants.OrderBy(c => c.OffPrice ?? c.Price).First().FinalPrice : 0 : 0))
            .ForMember(dest => dest.Score,
                options =>
                    options.MapFrom(src => src.productComments.Any() ? src.productComments.Average(c => c.Score) : 0))
            ;

        this.CreateMap<Entities.ProductFeature, ShowFeatureInCompareViewModel>().ReverseMap();



        this.CreateMap<Entities.Product, ProductItemsForShowProductInComparePartialViewModel>()

            .ForMember(dest => dest.MainPicure, options => options.MapFrom(src => src.ProductMedia.First().FileName))

            .ForMember(dest => dest.Price,
                options =>
                    options.MapFrom(src =>
                        src.ProductStockStatustus == ProductStockStatus.Available
                            ? src.ProductVariants.Any()
                                ? src.ProductVariants.OrderBy(x => x.OffPrice ?? x.Price).First().FinalPrice
                                : 0
                            : 0
                    ))



            .ForMember(dest => dest.Score,
                options =>
                    options.MapFrom(src => src.productComments.Any() ? src.productComments.Average(c => c.Score) : 0))


            .ForMember(dest => dest.Count,
                options =>
                    options.MapFrom(src =>
                        src.ProductStockStatustus == ProductStockStatus.Available
                            ?
                            src.ProductVariants.Any()
                                ?
                                src.ProductVariants.OrderBy(x => x.OffPrice ?? x.Price).First().Count > 3
                                    ? 0
                                    :
                                    src.ProductVariants.OrderBy(x => x.OffPrice ?? x.Price).First().Count
                                :
                                0
                            :
                            0
                    ));


    }
}
