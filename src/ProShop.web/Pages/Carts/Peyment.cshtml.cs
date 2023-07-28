using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parbad;
using Parbad.AspNetCore;
using Parbad.Gateway.ZarinPal;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Cart;

namespace ProShop.web.Pages.Carts
{
    [Authorize]
    public class PeymentModel : PageBase
    {
        private readonly ICartService _cartService;
        private readonly IAddressService _addressService;
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOnlinePayment _onlinePayment;

        public PeymentModel(ICartService cartService, IAddressService addressService, IOrderService orderService, IUnitOfWork unitOfWork, IOnlinePayment onlinePayment)
        {
            _cartService = cartService;
            _addressService = addressService;
            _orderService = orderService;
            _unitOfWork = unitOfWork;
            _onlinePayment = onlinePayment;
        }

        public PeymentViewModel PeymentPage { get; set; } = new PeymentViewModel();


        [BindProperty]
        public CreateOrderAndPayViewModel CreateOrderAndPayModel { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var userId = User.Identity.GetLoggedUserId();
            PeymentPage.CartItems = await _cartService.GetCartsForPeymentPage(userId);
            if (PeymentPage.CartItems.Count < 1)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (CreateOrderAndPayModel.PayFromWallet)
            {
                //par order Price from wallet
            }

            var userId = User.Identity.GetLoggedUserId();
            var address = await _addressService.GetAddressForCreateOrderAndPay(userId);
            if (!address.HasUserAddress)
                return RedirectToPage("CheckOut");

            var orderToAdd = new Order()
            {
                UserId = userId,
                AddressId = address.AddressId,
                PayFromWallet = false,

            };


            var CartItems = await _cartService.GetCartsForCreateOrderAndPay(userId);

            if(CartItems.Count <1)
                return RedirectToPage("Index");


            var normalProducts = CartItems.Where(c => c.ProductVariantProductDimensions == ProShop.Entities.ProductDimensions.Normal)
                .ToList();

            var HeavyProducts = CartItems.Where(c => c.ProductVariantProductDimensions == ProShop.Entities.ProductDimensions.Heavy)
                .ToList();

            var UltraHeavyProducts = CartItems.Where(c => c.ProductVariantProductDimensions == ProShop.Entities.ProductDimensions.UltraHeavy)
                .ToList();

            var sumPriceOfNormalProducts = normalProducts.Sum(x => (x.IsDiscountActive ? x.ProductVariantOffPrice.Value : x.ProductVariantPrice) * x.Count);
            var sumPriceOfHeavyProducts = HeavyProducts.Sum(x => (x.IsDiscountActive ? x.ProductVariantOffPrice.Value : x.ProductVariantPrice) * x.Count);


            //noraml Products
            if (normalProducts.Count > 0)
            {
                var parcelPostToAdd = new ParcalPost()
                {
                    Dimensions = ProductDimensions.Normal,
                    OrderStatus = OrderStatus.WaitingForPaying,
                    ShippingPrice = sumPriceOfNormalProducts < 500000 ? 30000 : 0,
                };

                foreach (var normalProduct in normalProducts)
                {
                    var parcelPostItemsToAdd = new Entities.ParcelPostItem()
                    {
                        Count = normalProduct.Count,
                        ProductVariantId = normalProduct.ProductVariantId,
                        GaranteeId = normalProduct.ProductVariantGaranteeId,
                        Score = normalProduct.Score,
                        Price = normalProduct.ProductVariantPrice,

                    };
                    if (normalProduct.IsDiscountActive)
                    {
                        parcelPostItemsToAdd.DiscountPrice =
                            normalProduct.ProductVariantPrice - normalProduct.ProductVariantOffPrice.Value;
                    }

                    parcelPostToAdd.ParcelPostItems.Add(parcelPostItemsToAdd);
                }
                orderToAdd.ParcalPosts.Add(parcelPostToAdd);

            }


            //Heavy Products
            if (HeavyProducts.Count > 0)
            {
                var parcelPostToAdd = new ParcalPost()
                {
                    Dimensions = ProductDimensions.Heavy,
                    OrderStatus = OrderStatus.WaitingForPaying,
                    ShippingPrice = sumPriceOfHeavyProducts < 500000 ? 45000 : 0,
                };

                foreach (var heavyProduct in HeavyProducts)
                {
                    var parcelPostItemsToAdd = new Entities.ParcelPostItem()
                    {
                        Count = heavyProduct.Count,
                        ProductVariantId = heavyProduct.ProductVariantId,
                        GaranteeId = heavyProduct.ProductVariantGaranteeId,
                        Score = heavyProduct.Score,
                        Price = heavyProduct.ProductVariantPrice,

                    };
                    if (heavyProduct.IsDiscountActive)
                    {
                        parcelPostItemsToAdd.DiscountPrice =
                            heavyProduct.ProductVariantPrice - heavyProduct.ProductVariantOffPrice.Value;
                    }

                    parcelPostToAdd.ParcelPostItems.Add(parcelPostItemsToAdd);
                }
                orderToAdd.ParcalPosts.Add(parcelPostToAdd);

            }


            //UltraHeavy Products
            if (UltraHeavyProducts.Count > 0)
            {
                var parcelPostToAdd = new ParcalPost()
                {
                    Dimensions = ProductDimensions.UltraHeavy,
                    OrderStatus = OrderStatus.WaitingForPaying,
                    ShippingPrice = 0,
                };

                foreach (var ultraHeavyProduct in UltraHeavyProducts)
                {
                    var parcelPostItemsToAdd = new Entities.ParcelPostItem()
                    {
                        Count = ultraHeavyProduct.Count,
                        ProductVariantId = ultraHeavyProduct.ProductVariantId,
                        GaranteeId = ultraHeavyProduct.ProductVariantGaranteeId,
                        Score = ultraHeavyProduct.Score,
                        Price = ultraHeavyProduct.ProductVariantPrice,

                    };
                    if (ultraHeavyProduct.IsDiscountActive)
                    {
                        parcelPostItemsToAdd.DiscountPrice =
                            ultraHeavyProduct.ProductVariantPrice - ultraHeavyProduct.ProductVariantOffPrice.Value;
                    }

                    parcelPostToAdd.ParcelPostItems.Add(parcelPostItemsToAdd);
                }
                orderToAdd.ParcalPosts.Add(parcelPostToAdd);

            }


            //// remove Cart 
            var _productVariantIds = CartItems.Select(c => c.ProductVariantId).ToList();
            var cartItems = await _cartService.GetCartsForRemove(userId, _productVariantIds);

            _cartService.RemoveRange(cartItems);




            //قیمت قابل پرداخت
            var TotalPrice = CartItems
                .Sum(x => (x.IsDiscountActive ? x.ProductVariantOffPrice.Value : x.ProductVariantPrice)
                          *
                          x.Count
                );

            var sumPriceOfShipping = 0;
            if (sumPriceOfNormalProducts < 500000 && normalProducts.Count > 0)
            {
                sumPriceOfShipping += 30000;
            }
            if (sumPriceOfHeavyProducts < 500000 && HeavyProducts.Count > 0)
            {
                sumPriceOfShipping += 45000;

            }

            var FinalPrice = TotalPrice + sumPriceOfShipping;


            //Payment
            var CallbackUrl = Url.PageLink("VerifyPayment", null, null, Request.Scheme);
            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice
                    .SetAmount(FinalPrice)
                    .SetCallbackUrl(CallbackUrl)
                    .SetGateway(CreateOrderAndPayModel.PaymentGateway.ToString())
                    .UseAutoIncrementTrackingNumber();
                if (CreateOrderAndPayModel.PaymentGateway == PaymentGateway.Zarinpal)
                {
                    invoice.SetZarinPalData(new ZarinPalInvoice("پرداخت آنلاین پرو شاپ"));
                }

            });

            orderToAdd.OrderNumber = result.TrackingNumber;
            orderToAdd.PaymentGateway = CreateOrderAndPayModel.PaymentGateway;
            if (result.IsSucceed)
            {
                await _orderService.AddAsync(orderToAdd);
                await _unitOfWork.SaveChangesAsync();
                return result.GatewayTransporter.TransportToGateway();
            }
            else
            {
                return RedirectToPage(PublicConstantStrings.Error500PageName);

            }
            //end payment
        }

    }
}
