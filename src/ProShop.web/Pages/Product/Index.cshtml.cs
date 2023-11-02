using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Mvc;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using ProShop.ViewModels.Product;
using ProShop.ViewModels.Product.ProductComments;

namespace ProShop.web.Pages.Product;

public class IndexModel : PageBase
{

    private readonly IProductService _productService;
    private readonly IUserProductFavoriteService _userFavoriteService;
    private readonly IProductVariantService _productVariantService;
    private readonly ICartService _cartService;
    private readonly IUnitOfWork unitOfWork;
    private readonly IViewRenderService _viewRendererService;
    private readonly ICommentsReportsService _commentsReportsService;
    private readonly IProductCommentService _productCommentService;
    private readonly ICommentScoreService _commentScoreService;
    public IndexModel(IProductService productService, IUserProductFavoriteService userFavoriteService, IUnitOfWork unitOfWork, IProductVariantService productVariantService, ICartService cartService, IViewRenderService viewRendererService, ICommentsReportsService commentsReportsService, IProductCommentService productCommentService, ICommentScoreService commentScoreService)
    {
        _productService = productService;
        _userFavoriteService = userFavoriteService;
        this.unitOfWork = unitOfWork;
        _productVariantService = productVariantService;
        _cartService = cartService;
        _viewRendererService = viewRendererService;
        _commentsReportsService = commentsReportsService;
        _productCommentService = productCommentService;
        _commentScoreService = commentScoreService;
    }

    public ShowProductInfoViewModel ProductInfo { get; set; }

    public async Task<IActionResult> OnGet(int productCode, string slug)
    {

        ProductInfo = await _productService.GetProductInfo(productCode);
        if (ProductInfo is null)
            return RedirectToPage(PublicConstantStrings.Error404PageName);

        if (ProductInfo.Slug != slug)
            return RedirectToPage("Index", new { productCode, slug = ProductInfo.Slug });

        ProductInfo.ProductsPageCount =(int)Math.Ceiling((decimal)ProductInfo.productCommentsCount/1);


        var userid = User.Identity.GetLoggedUserId();
        var ProductVariantsIds = ProductInfo.ProductVariants.Select(c => c.Id).ToList();
        ProductInfo.ProductVariantInCart = await _cartService.GetProductVariantsInCart(ProductVariantsIds, userid);
        var commentIds = ProductInfo.productComments.Select(c=>c.Id).ToArray();
        ProductInfo.LikedCommentByUser = await _commentScoreService.GetLikedCommentByUser(userid,commentIds);
        return Page();

    }
    public async Task<IActionResult> OnPostAddOrRemoveFavorite(long Id, bool addFavorite)
    {
        if (!User.Identity.IsAuthenticated)
            return Json(new JsonResultOperation(false, "ابتدا به حساب کاربری خود وارد شوید"));

        if (!await _productService.IsExistsBy(nameof(Entities.Product.Id), Id))
            return Json(new JsonResultOperation(false));


        var userId = User.Identity.GetLoggedUserId();
        var userProductFavorite = await _userFavoriteService.FindAsync(userId, Id);
        if (userProductFavorite is null && addFavorite)
        {
            await _userFavoriteService.AddAsync(new UserProductFavorite()
            {
                ProductId = Id,
                UserId = userId
            });
        }
        else if (userProductFavorite != null && !addFavorite)
        {
            _userFavoriteService.Remove(userProductFavorite);
        }

        await unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true));
    }

    //Id is ProductVariantId
    public async Task<IActionResult> OnPostAddProductVariantToCart(long productVariantId, bool isIncrease)
    {
        var ProductVariant = await _productVariantService.FindByIdAsync(productVariantId);
        if (ProductVariant is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var userId = User.Identity.GetLoggedUserId();

        var cart = await _cartService.FindAsync(userId, productVariantId);
        if (cart is null)
        {
            var CartToAdd = new Cart()
            {
                ProductVaraintId = productVariantId,
                UserId = userId,
                Count = 1
            };


            await _cartService.AddAsync(CartToAdd);
        }
        else if (isIncrease)
        {
            cart.Count++;
            if (cart.Count > ProductVariant.MaxCountInCart)
                cart.Count = ProductVariant.MaxCountInCart;

            //ProductVariant.Count موجودی انبار
            if (cart.Count > ProductVariant.Count)
                cart.Count = (short)ProductVariant.Count;
        }
        else
        {
            cart.Count--;
            if (cart.Count == 0)
                _cartService.Remove(cart);
        }




        await unitOfWork.SaveChangesAsync();


        // اگر کاونت سبد خرید برابر با مکس تعیین شده توسط فروشنده بود
        // یا مساوی تعداد موجودی داخل انبار، این متغیر ترو میشود

        var IsCartFull = ProductVariant.MaxCountInCart == (cart?.Count ?? 1) || (cart?.Count ?? 1) == ProductVariant.Count;


        var carts = await _cartService.GetCartForDropDown(userId);

        return Json(new JsonResultOperation(true, "محصول مورد نظر به ثبت خرید شما اضافه شد")
        {

            Data = new
            {
                count = cart?.Count ?? 1,
                productvariantid = productVariantId,
                iscartfull = IsCartFull,
                cartsDetails = await _viewRendererService.RenderViewToStringAsync("~/Pages/Shared/_Cart.cshtml", carts)
            }

        });

    }

    public async Task<IActionResult> OnPostAddCommentReports(long commentId)
    {
        if (commentId < 0)
            return RecordNotFound();

        var userid = ProShop.Common.IdentityToolkit.IdentityExtensions.GetUserId(User.Identity);

        if (userid is null)
            return RecordNotFound();

        if (!await _productCommentService.IsExistsBy(nameof(Entities.ProductComment.Id), commentId))
            return RecordNotFound();


        if (await _commentsReportsService.IsExistsBy(nameof(Entities.CommentsReports.UserId),
                nameof(Entities.CommentsReports.ProductCommentId), userid, commentId))
        {
            return Json(new JsonResultOperation(false, "شما برای این کامنت قبلا گزارش ثبت کرده اید."));

        }


        var _comment = new CommentsReports()
        {
            UserId = userid.Value,
            ProductCommentId = commentId
        };


        await _commentsReportsService.AddAsync(_comment);
        await unitOfWork.SaveChangesAsync();

        return Json(new JsonResultOperation(true, "گزارش شما با موفقیت ثبت شد با تشکر از همکاری شما"));
    }

    public async Task<IActionResult> OnGetShowCommentsByPagination(long productId,int pageNumber,int commentPagesCount, CommentSorting sortBy, SortingOrder orderBy)
    {
        if(!await _productService.IsExistsBy(nameof(Entities.Product.Id),productId))
        {
            return jsonBadRequest();
        }

        var comment = await _productCommentService.GetCommentsByPagination(productId, pageNumber,sortBy,orderBy);

        var userid = User.Identity.GetLoggedUserId();
        var commentIds = comment.Select(c => c.Id).ToArray();
        var model = new CommentForCommentPartialViewModel
        {
            ProductComments = comment,
            CommentPageCount = commentPagesCount,
            CurrentPage = pageNumber,
            LikedCommentByUser = await _commentScoreService.GetLikedCommentByUser(userid, commentIds)
        };
        return Partial("_CommentPartial", model);
    }


    public async Task<IActionResult> OnPostCommentScore(long commentId, bool isLike)
    {
        var userid = ProShop.Common.IdentityToolkit.IdentityExtensions.GetUserId(User.Identity);
        if (userid is null)
            return RecordNotFound();

        if (!await _productCommentService.IsExistsBy(nameof(Entities.ProductComment.Id), commentId))
            return RecordNotFound();

        var commentScore = await _commentScoreService.FindAsync(userid.Value,commentId);
        var operation = string.Empty;
        if (commentScore == null)
        {
            operation = "Add";
            await _commentScoreService.AddAsync(new CommentScore()
            {
                ProductCommentId = commentId,
                UserId = userid.Value,
                IsLike = isLike
            });
        }
        else if (commentScore.IsLike && isLike || !commentScore.IsLike && !isLike)
        {
            operation = "Subtract";
            _commentScoreService.Remove(commentScore);
        }
        else
        {
            operation = "AddAndSubtract";
            commentScore.IsLike = !commentScore.IsLike;
        }

        await unitOfWork.SaveChangesAsync();

        return Json(new JsonResultOperation(true, string.Empty)
        {
            Data = operation,
        });
    }
}
