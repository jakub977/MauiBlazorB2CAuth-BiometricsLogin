using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of (effective and direct) users who are members of roles
/// </summary>
[Table("RoleMember")]
public partial class RoleMember
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

    /// <summary>
    /// Link to dbo.Customer as an user who creates a role member
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("RoleMemberCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Customer as an direct user (i.e. user who is member only of an organization and not of a directory) who is a member of a role
    /// </summary>
    [ForeignKey("DirectUserId")]
    [InverseProperty("RoleMemberDirectUsers")]
    public virtual Customer? DirectUser { get; set; }

    /// <summary>
    /// Link to dbo.EffectiveUser as an effective user (i.e. user who is member of a directory and not only of an organization) who is a member of a role
    /// </summary>
    [ForeignKey("EffectiveUserId")]
    [InverseProperty("RoleMembers")]
    public virtual EffectiveUser? EffectiveUser { get; set; }

    /// <summary>
    /// Link to dbo.Role as a role which is grant to a (direct or effective) user
    /// </summary>
    [ForeignKey("RoleId")]
    [InverseProperty("RoleMembers")]
    public virtual Role Role { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Customer as an user who updates a role member
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RoleMemberUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
