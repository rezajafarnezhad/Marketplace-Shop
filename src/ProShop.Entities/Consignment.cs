using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProShop.Entities;

public class Consignment : EntityBase, IAuditableEntity
{

    public long sellerId { get; set; }
    public string Description { get; set; }
    public DateTime DeliveryDate { get; set; }
    public ConsignmentStatus ConsignmentStatus { get; set; }

    public Seller Seller { get; set; }
    public ICollection<ConsignmentItem> ConsignmentItems { get; set; } = new List<ConsignmentItem>();
    public ICollection<ProductStock> ProductStocks { get; set; }
}


public enum ConsignmentStatus : byte
{
    [Display(Name = "در انتظار تایید")]
    AwaitingApproval,

    [Display(Name = "تایید شده و در انتظار ارسال محموله")]
    ConfirmAndAwaitingForConsignment,

    [Display(Name = "دریافت شده")]
    Received,

    [Display(Name = "دریافت شده و موجودی افزایش یافته")]
    ReceivedAndAddStock,

    [Display(Name = "رد شده")]
    Rejected,

    [Display(Name = "لغو شده")]
    Canceled

}