using ProShop.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProShop.Entities;

public class Order : EntityBase, IAuditableEntity
{
    public long UserId { get; set; }
    public long AddressId { get; set; }
    public int OrderNumber { get; set; }
    public DateTime CreatedDateTime { get; set; }
    /// <summary>
    /// آیا این سفارش توسط مقدار داخل کیف پول کاربر پرداخت شده است ؟
    /// اگر فالس باشد یعنی توسط درگاه اینترنتی پرداخت شده است
    /// اگر هم ترو باشد، یعنی توسط کیف پول پرداخت شده است
    /// </summary>
    public bool PayFromWallet { get; set; }
    public User User { get; set; }
    public Address Address { get; set; }
    public ICollection<ParcalPost> ParcalPosts { get; set; }
}

public enum OrderStatus : byte
{
    [Display(Name = "در انتظار پرداخت")]
    WaitingForPaying,

    [Display(Name = "در حال پردازش")]
    Processing,

    [Display(Name = "پردازش انبار")]
    InventoryProcessing,

    [Display(Name = "تحویل به پست")]
    DeliveredToPost,

    [Display(Name = "تحویل شده")]
    DeliveredToClient
}