﻿using Microsoft.AspNetCore.Identity;
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

    
    public string Avatar { get; set; }

    public DateTime SendSmsLastTime { get; set; }

    public virtual ICollection<UserClaim> UserClaims { get; set; }
    public virtual ICollection<UserLogin> UserLogins { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<UserToken> UserTokens { get; set; }
}