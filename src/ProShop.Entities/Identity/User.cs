using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ProShop.Entities.Identity;

public class User : IdentityUser<long>, IAuditableEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    public bool IsActive { get; set; }

    public DateTime CreatedDateTime { get; set; }
    public DateTime? BirthDate { get; set; }
    public bool IsSeller { get; set; }

    
    public string Avatar { get; set; }
    public string NationalCode { get; set; }
    public Gender? Gender { get; set; }
    public DateTime SendSmsLastTime { get; set; }

    public virtual ICollection<UserClaim> UserClaims { get; set; }
    public virtual ICollection<UserLogin> UserLogins { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<UserToken> UserTokens { get; set; }
    public virtual ICollection<UserProductFavorite> UserProductFavorites { get; set; }
    public virtual ICollection<Cart> Carts{ get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<CommentScore> CommentScores { get; set; }
    public virtual ICollection<CommentsReports> CommentsReports { get; set; }
    public virtual ICollection<ProductQuestionAnswerScore> ProductQuestionAnswerScore { get; set; }
    public Seller Seller { get; set; }
}

public enum Gender
{
    [Display(Name = "آقا")]
    Man,

    [Display(Name = "خانم")]

    Woman
}