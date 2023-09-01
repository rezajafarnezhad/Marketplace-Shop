using ProShop.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProShop.Entities;

public class Order : EntityBase, IAuditableEntity
{
    public long UserId { get; set; }
    public long AddressId { get; set; }
    public long OrderNumber { get; set; }
    public string BankTransactionCode { get; set; }
    public int TotalPrice { get; set; }
    public int? DiscountPrice { get; set; }
    public byte TotalScore { get; set; }
    public byte ShippingCount { get; set; }
    public int FinalPrice { get; set; }
    public DateTime CreatedDateTime { get; set; }
    /// <summary>
    /// آیا این سفارش توسط مقدار داخل کیف پول کاربر پرداخت شده است ؟
    /// اگر فالس باشد یعنی توسط درگاه اینترنتی پرداخت شده است
    /// اگر هم ترو باشد، یعنی توسط کیف پول پرداخت شده است
    /// </summary>
    public bool PayFromWallet { get; set; }
    public bool IsPay { get; set; }
    /// <summary>
    /// از کدام درگاه، پرداختی انجام شده است
    /// </summary>
    public PaymentGateway? PaymentGateway { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public User User { get; set; }
    public Address Address { get; set; }
    public ICollection<ParcalPost> ParcalPosts { get; set; } = new List<ParcalPost>();
}

public enum OrderStatus : byte
{
    [Display(Name = "در انتظار پرداخت")]
    WaitingForPaying,

    [Display(Name = "در حال پردازش")]
    Processing,

    [Display(Name = "پردازش انبار")]
    InventoryProcessing,

    [Display(Name = "بخشی از مرسوله ها در پست")]
    SomeParcelsDeliveredToPost,
    [Display(Name = "تمام مرسوله ها در پست")]

    CompletelyParcelsDeliveredToPost,
    [Display(Name = "تحویل شده")]
    DeliveredToClient,
}

public enum PaymentGateway : byte
{
    [Display(Name = "زرین پال")]
    Zarinpal,

    [Display(Name = "به پرداخت ملت")]
    Mellat,

    [Display(Name = "درگاه مجازی تست")]
    ParbadVirtual,
}