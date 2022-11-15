using ProShop.Entities.Identity;

namespace ProShop.Entities;

public class ProductComment : EntityBase, IAuditableEntity
{
    #region Properties

    public long? UserId { get; set; }

    public long? SellerId { get; set; }

    public long ProductId { get; set; }

    public long? VariantId { get; set; }
    public long? SellerShopNameId { get; set; }

    public byte Score { get; set; }

   
    public string CommentTitle { get; set; }

   
    public string CommentText { get; set; }

    public bool? Suggest { get; set; }

    
    public string PositiveItems { get; set; }

    
    public string NegativeItems { get; set; }


    public DateTime CreatedDateTime { get; set; }

    public bool IsUnknown { get; set; }

    public bool IsBuyer { get; set; }

    public bool IsConfirmed { get; set; }

    #endregion

    #region Relations

    public User User { get; set; }

    public Seller Seller { get; set; }

    public Seller SellerShopName { get; set; }

    public Variant Variant { get; set; }

    #endregion
}