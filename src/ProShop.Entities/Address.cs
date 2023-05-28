using ProShop.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProShop.Entities;

public class Address : EntityBase, IAuditableEntity
{
    public long UserId { get; set; }
    public long ProvinceId { get; set; }
    public long CityId { get; set; }
    public string AddressLine { get; set; }

    /// <summary>
    /// پلاک
    /// </summary>
    public int No { get; set; }

    /// <summary>
    /// واحد
    /// </summary>
    public short Unit { get; set; }

    [MaxLength(10)]
    public string Pob { get; set; }

    [MaxLength(200)]
    public string FirstName { get; set; }

    [MaxLength(200)]
    public string LastName { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    [MaxLength(11)]
    public string PhoneNumber { get; set; }
    public User User { get; set; }
    public ProvinceAndCity Province { get; set; }
    public ProvinceAndCity City { get; set; }
}