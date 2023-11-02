using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Delta;
using Principal.Telemedicine.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of effective users who are members of groups
/// </summary>
[Table("GroupEffectiveMember")]
public partial class GroupEffectiveMember
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

    /// <summary>
    ///  Link to dbo.Customer as an user who creates a group effective member
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("GroupEffectiveMemberCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Link to dbo.EffectiveUser as an effective user (i.e. user who is member of a directory and not only of an organization) who is a member of group"
    /// </summary>
    [ForeignKey("EffectiveUserId")]
    [InverseProperty("GroupEffectiveMembers")]
    public virtual EffectiveUser EffectiveUser { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Group as a group of which user is a member
    /// </summary>
    [ForeignKey("GroupId")]
    [InverseProperty("GroupEffectiveMembers")]
    public virtual Group Group { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Customer as an user who updates a group effective member
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("GroupEffectiveMemberUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    /// <summary>
    /// Vrátí GroupEffectiveMemberContract z GroupEffectiveMember
    /// </summary>
    /// <param name="withGroup">Příznak, zda chceme vrátit i data Skupiny (default TRUE)</param>
    /// <param name="withProviderInGroup">Příznak, zda chceme vrátit se Skupinou i data Poskytovatele (default TRUE)</param>
    /// <param name="withGroupPermissions">Příznak, zda chceme vrátit se Skupinou i data Permissions (default TRUE)</param>
    /// <param name="withPermissionSubject">Příznak, zda chceme vrátit se Skupinou i data Subjektu v Permissions (default TRUE)</param>
    /// <param name="withRolesAndGroupsDetail">Příznak, zda chceme vrátit i podrobnější data jako kategorie nebo typ Role / Skupiny (default FALSE)</param>
    /// <returns>GroupEffectiveMemberContract</returns>
    public GroupEffectiveMemberContract ConvertToGroupEffectiveMemberContract(bool withGroup = true, bool withProviderInGroup = true, bool withGroupPermissions = true, bool withPermissionSubject = true, bool withRolesAndGroupsDetail = false)
    {
        GroupEffectiveMemberContract data = new GroupEffectiveMemberContract();

        data.Active = Active.GetValueOrDefault();
        data.CreatedByCustomerId = CreatedByCustomerId;
        data.CreatedDateUtc = CreatedDateUtc;
        data.Deleted = Deleted;
        data.EffectiveUserId = EffectiveUserId;
        data.GroupId = GroupId;
        data.Id = Id;
        data.UpdateDateUtc = UpdateDateUtc;
        data.UpdatedByCustomerId = UpdatedByCustomerId;

        if (withGroup && Group != null)
            data.Group = Group.ConvertToGroupContract(withProviderInGroup, withGroupPermissions, withPermissionSubject, withRolesAndGroupsDetail);

        return data;
    }
}
