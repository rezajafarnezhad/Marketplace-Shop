using ProShop.Entities.Identity;

namespace ProShop.Entities;

public class CommentScore : EntityBase, IAuditableEntity
{
    public long ProductCommentId { get; set; }
    public long UserId { get; set; }
    public bool IsLike { get; set; }


    public User User { get; set; }
    public ProductComment ProductComment{ get; set; }    
}