namespace ProShop.Entities;


/// <summary>
/// مرسوله
/// هر سفارش میتواند شامل چندین مرسوله باشد
/// </summary>
public class ParcalPost : EntityBase, IAuditableEntity
{
    public long OrderId { get; set; }
    public ProductDimensions Dimensions { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string PostTrackingCode { get; set; }
    /// <summary>
    /// هزینه پرداخت شده برای  پست کردن این مرسوله
    /// برای مثال در حال حاضر هزینه پستی 30 هزار است، اما بعد از شش
    /// ماه به 40 هزار تغییر میکند، ما باید بدونیم که 6 ماه پیش که این مرسوله ارسال
    /// شده است، چه هزینه ایی بابت پست پرداخت شده است
    /// </summary>
    public int ShippingPrice { get; set; }
    public Order Order { get; set; }
    public ICollection<ParcelPostItem> ParcelPostItems { get; set; }
}