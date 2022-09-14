using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Sellers;

namespace ProShop.web.Pages.AdminPanel.Seller;

public class IndexModel : PageBase
{

    private readonly ISellerService _sellerService;
    private readonly IHtmlSanitizer _htmlSanitizer;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadFileService _fileService;
    public IndexModel(ISellerService sellerService, IHtmlSanitizer htmlSanitizer, IUnitOfWork unitOfWork, IUploadFileService fileService)
    {
        _sellerService = sellerService;
        _htmlSanitizer = htmlSanitizer;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    [BindProperty(SupportsGet = true)]
    public ShowSellersViewModel Sellers { get; set; } = new();
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetGetDataTableAsync()
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        return Partial("_List", await _sellerService.GetSellers(Sellers));
    }


    public async Task<IActionResult> OnGetGetSellerDetails(long sellerId)
    {
        var data = await _sellerService.GetSellerDetails(sellerId);
        if (data is null) 
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
       
        return Partial("_SellerDetails",data);
    }

    public async Task<IActionResult> OnPostRejectSellerDocuments(SellerDetailsViewModel sellers)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, "لطفا دلایل رد مدارک فروشنده را وارد کنید"));
        }

        var _seller = await _sellerService.FindByIdAsync(sellers.Id);
        if(_seller is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        _seller.DocumentStatus = DocumentStatus.Rejected;
        _seller.RejectReason = _htmlSanitizer.Sanitize(sellers.RejectReason);
        await _unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "مدارک فروشنده مورد نظر باموفقیت رد شد"));
    }

    public async Task<IActionResult> OnPostConfirmSellerDocuments(long Id)
    {
        if (Id < 1)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var _seller = await _sellerService.FindByIdAsync(Id);       
        if (_seller is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        _seller.DocumentStatus = DocumentStatus.Confirmed;
        _seller.RejectReason = null;
        await _unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "مدارک فروشنده مورد نظر باموفقیت تایید شد"));

    }  
    
    public async Task<IActionResult> OnPostRemoveSeller(long Id)
    {
        if (Id < 1)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var _seller = await _sellerService.GetSellerToRemoveInManagingSeller(Id);       
        if (_seller is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        _seller.User.IsSeller = false;
        _sellerService.Remove(_seller);
         await _unitOfWork.SaveChangesAsync();
        _fileService.DeleteFile(_seller.IdCartPicture, "images", "seller-id-cart-pictures");
        _fileService.DeleteFile(_seller.Logo, "images", "seller-logos");
        return Json(new JsonResultOperation(true, "فروشنده مورد نظر باموفقیت حذف شد"));

    }
}