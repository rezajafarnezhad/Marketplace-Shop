using ProShop.Entities.Identity;

namespace ProShop.Entities;

public class ProductQuestionAndAnswer : EntityBase, IAuditableEntity
{
    /// <summary>
    /// متن سوال یا حواب    
    /// </summary>
    public string Body { get; set; }
    public bool IsConfirmed { get; set; }
    
    /// <summary>
    /// آیا کاربر میخاهد سوال یا جوابش به صورت ناشناش ثبت شود
    /// </summary>
    public bool IsUnknown { get; set; }

    public bool IsBuyer { get; set; }

    /// <summary>
    /// اگر سوال باشد null است
    /// اگر جواب باشد آیدی سوال قرار میگیرد
    /// </summary>
    public long? IsParent { get; set; }

    /// <summary>
    /// کدام فروشنده پاسخ داده است ؟
    /// فروشندگان قابلیت پرسش سوال را ندارند
    /// و اگر بخواهند پرسشی داشته باشند باید توسط یوزر خود، سوال بپرسند
    /// پس اگر این رکورد، سوال باشد این پراپرتی نال میشود
    /// </summary>
    public long? SellerId { get; set; }

    /// <summary>
    /// کدام کاربر این سوال یا پرسش را مطرح کرده است ؟
    /// اگر فروشگاه پاسخ ثبت کرده باشد این مورد نال میشود
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// برای کدام محصول سوال ثبت شده است ؟
    /// </summary>
    /// 
    public long? ProductId { get; set; }

    public Seller Seller { get; set; }
    public User User { get; set; }
    public Product Product { get; set; }

    public ProductQuestionAndAnswer Parent { get; set; }
    public ICollection<ProductQuestionAndAnswer> Answers { get; set; }
    public virtual ICollection<ProductQuestionAnswerScore> ProductQuestionAnswerScore { get; set; }


}