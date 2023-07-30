namespace ProShop.Entities;

/// <summary>
/// محصولات داخل مرسوله
/// </summary>
public class ParcelPostItem : EntityBase, IAuditableEntity
{
    public long ParcalPostId { get; set; }
    public long ProductVariantId { get; set; }
    public long GaranteeId { get; set; }
    public int Price { get; set; }

    public int? DiscountPrice { get; set; }

    public int Count { get; set; }

    public byte Score { get; set; }


    public ParcalPost ParcalPost { get; set; }
    public ProductVariant ProductVariant { get; set; }
    public Garantee Garantee { get; set; }
}

