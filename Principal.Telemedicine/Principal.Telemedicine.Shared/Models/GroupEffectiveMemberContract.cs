using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from GroupEffectiveMember.cs
/// </summary>
[DataContract]
public class GroupEffectiveMemberContract
{

    /// <summary>
    /// Primary identifier of a group member
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a group member is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a group member is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a group member
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of group member creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a group member
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of group member update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.EffectiveUser as an effective user (i.e. user who is member of a directory and not only of an organization) who is a member of group
    /// </summary>
    public int EffectiveUserId { get; set; }

    /// <summary>
    /// Link to dbo.Group as a group of which user is a member
    /// </summary>
    public int GroupId { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("GroupEffectiveMemberCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [ForeignKey("EffectiveUserId")]
    [InverseProperty("GroupEffectiveMembers")]
    public virtual EffectiveUserContract EffectiveUser { get; set; } = null!;

    [ForeignKey("GroupId")]
    [InverseProperty("GroupEffectiveMembers")]
    public virtual GroupContract Group { get; set; } = null!;

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("GroupEffectiveMemberUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
