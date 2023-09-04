using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from Group.cs
/// </summary>
[DataContract]
public class GroupContract
{

    /// <summary>
    /// Primary identifier of a group
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a group is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a group is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a group
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of group creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a group
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of group update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a parent provider
    /// </summary>
    public int ProviderId { get; set; }

    /// <summary>
    /// Link to dbo.Group as a parent group of a group
    /// </summary>
    public int? ParentGroupId { get; set; }

    /// <summary>
    /// Link to dbo.GroupTagType as a tag type of a group
    /// </summary>
    public int? GroupTagTypeId { get; set; }

    /// <summary>
    /// Name of a group
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Picture as a photo of a group
    /// </summary>
    public int? PictureId { get; set; }

    /// <summary>
    /// Bit identifier if group is risk group
    /// </summary>
    public bool IsRiskGroup { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("GroupCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [InverseProperty("Group")]
    public virtual ICollection<GroupEffectiveMemberContract> GroupEffectiveMembers { get; set; } = new List<GroupEffectiveMemberContract>();

    [InverseProperty("Group")]
    public virtual ICollection<GroupPermissionContract> GroupPermissions { get; set; } = new List<GroupPermissionContract>();

    [InverseProperty("ParentGroup")]
    public virtual ICollection<GroupContract> InverseParentGroup { get; set; } = new List<GroupContract>();

    [ForeignKey("ParentGroupId")]
    [InverseProperty("InverseParentGroup")]
    public virtual Group? ParentGroup { get; set; }

    [ForeignKey("PictureId")]
    [InverseProperty("Groups")]
    public virtual PictureContract? Picture { get; set; }

    [ForeignKey("ProviderId")]
    [InverseProperty("Groups")]
    public virtual ProviderContract Provider { get; set; } = null!;

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("GroupUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
