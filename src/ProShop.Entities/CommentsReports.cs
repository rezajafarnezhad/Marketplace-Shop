using ProShop.Entities.Identity;

namespace ProShop.Entities;

/// <summary>
/// گزارش کامنت ها
/// هر کاربر میتواند یک
///گزارش برای کامنت درج کند
/// </summary>
public class CommentsReports : EntityBase, IAuditableEntity
{
    public long ProductCommentId { get; set; }
    public long UserId { get; set; }
    public bool IsLike { get; set; }


    public User User { get; set; }
    public ProductComment ProductComment { get; set; }

}