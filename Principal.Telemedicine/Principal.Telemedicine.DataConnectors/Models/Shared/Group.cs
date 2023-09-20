using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of groups, third level of organization hierarchy
/// </summary>
[Table("Group")]
public partial class Group
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

    /// <summary>
    /// Link to dbo.Customer as an user who creates a group
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("GroupCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Inverse colllection of GroupEffectiveMembers (effective users who are members of groups)
    /// </summary>
    [InverseProperty("Group")]
    public virtual ICollection<GroupEffectiveMember> GroupEffectiveMembers { get; set; } = new List<GroupEffectiveMember>();

    /// <summary>
    /// Inverse collection of GroupPermissions
    /// </summary>
    [InverseProperty("Group")]
    public virtual ICollection<GroupPermission> GroupPermissions { get; set; } = new List<GroupPermission>();

    /// <summary>
    /// Inverse collection of Group as a inverse parent group
    /// </summary>
    [InverseProperty("ParentGroup")]
    public virtual ICollection<Group> InverseParentGroup { get; set; } = new List<Group>();

    /// <summary>
    /// Link to dbo.ParentGroupt as a parent group of group
    /// </summary>
    [ForeignKey("ParentGroupId")]
    [InverseProperty("InverseParentGroup")]
    public virtual Group? ParentGroup { get; set; }

    /// <summary>
    /// Link to dbo.Picture as a picture of group
    /// </summary>
    [ForeignKey("PictureId")]
    [InverseProperty("Groups")]
    public virtual Picture? Picture { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a parent provider
    /// </summary>
    [ForeignKey("ProviderId")]
    [InverseProperty("Groups")]
    public virtual Provider Provider { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Customer as an user who updates a group
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("GroupUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
