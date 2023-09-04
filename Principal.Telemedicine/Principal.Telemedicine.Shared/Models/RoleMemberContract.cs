using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from RoleMember.cs
/// </summary>
[DataContract]
public class RoleMemberContract
{
    /// <summary>
    /// Primary identifier of a role member
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a role member is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a role member is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a role member
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of role member creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a role member
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of role member update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.EffectiveUser as an effective user (i.e. user who is member of a directory and not only of an organization) who is a member of a role
    /// </summary>
    public int? EffectiveUserId { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an direct user (i.e. user who is member only of an organization and not of a directory) who is a member of a role
    /// </summary>
    public int? DirectUserId { get; set; }

    /// <summary>
    /// Link to dbo.Role as a role which is grant to a (direct or effective) user
    /// </summary>
    public int RoleId { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("RoleMemberCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [ForeignKey("DirectUserId")]
    [InverseProperty("RoleMemberDirectUsers")]
    public virtual UserContract? DirectUser { get; set; }

    [ForeignKey("EffectiveUserId")]
    [InverseProperty("RoleMembers")]
    public virtual EffectiveUserContract? EffectiveUser { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("RoleMembers")]
    public virtual RoleContract Role { get; set; } = null!;

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RoleMemberUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
