using ProShop.Entities.Identity;

namespace ProShop.Entities;

public class ProductQuestionAnswerScore : EntityBase, IAuditableEntity
{
    #region Properties

    /// <summary>
    /// آیدی جواب سوال
    /// </summary>
    public long AnswerId { get; set; }

    /// <summary>
    /// آیدی کاربری که پاسخ داده
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// آیا لایک انجام شده یا دیسلایک
    /// </summary>
    public bool IsLike { get; set; }

    #endregion

    #region Relations

    public User User { get; set; }

    public ProductQuestionAndAnswer Answer { get; set; }

    #endregion
}