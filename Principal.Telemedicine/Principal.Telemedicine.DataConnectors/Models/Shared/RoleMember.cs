using Principal.Telemedicine.Shared.Models;
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

    /// <summary>
    /// Převede RoleMember na RoleMemberProviderContract využívaný pro správu Poskytovatelů
    /// </summary>
    /// <param name="role">Role RoleProviderContract</param>
    /// <returns>RoleMemberProviderContract</returns>
    public RoleMemberProviderContract ConvertToRoleMemberProviderContract(RoleProviderContract? role = null)
    {
        RoleMemberProviderContract data = new RoleMemberProviderContract();
        data.Id = this.Id;
        data.Active = this.Active.GetValueOrDefault();
        data.Deleted = this.Deleted;
        data.CreatedByCustomerId = this.CreatedByCustomerId;
        data.CreatedDateUtc = this.CreatedDateUtc;
        data.UpdateDateUtc = this.UpdateDateUtc;
        data.UpdatedByCustomerId = this.UpdatedByCustomerId;
        data.DirectUserId = this.DirectUserId;
        data.EffectiveUserId = this.EffectiveUserId;
        data.RoleId = this.RoleId;
        data.Role = role;

        return data;
    }

    /// <summary>
    /// Vrátí RoleMemberContract z RoleMember
    /// </summary>
    /// <param name="withRole">Příznak, zda chceme vrátit i data Role (default TRUE)</param>
    /// <param name="withProviderInRole">Příznak, zda chceme vrátit s Rolí i data Poskytovatele (default TRUE)</param>
    /// <param name="withRolePermissions">Příznak, zda chceme vrátit s Rolí i data Permissions (default TRUE)</param>
    /// <param name="withPermissionSubject">Příznak, zda chceme vrátit s Rolí i data Subjektu v Permissions (default TRUE)</param>
    /// <param name="withRolesAndGroupsDetail">Příznak, zda chceme vrátit i podrobnější data jako kategorie nebo typ Role / Skupiny (default FALSE)</param>
    /// <returns>RoleMemberContract</returns>
    public RoleMemberContract ConvertToRoleMemberContract(bool withRole = true, bool withProviderInRole = true, bool withRolePermissions = true, bool withPermissionSubject = true, bool withRolesAndGroupsDetail = false)
    {
        RoleMemberContract data = new RoleMemberContract();

        data.Active = Active.GetValueOrDefault();
        data.CreatedByCustomerId = CreatedByCustomerId;
        data.CreatedDateUtc = CreatedDateUtc;
        data.Deleted = Deleted;
        data.DirectUserId = DirectUserId;
        data.EffectiveUserId = EffectiveUserId;
        data.Id = Id;
        data.RoleId = RoleId;

        if (withRole && Role != null)
            data.Role = Role.ConvertToRoleContract(withProviderInRole, withRolePermissions, withPermissionSubject, withRolesAndGroupsDetail);

        data.UpdateDateUtc = UpdateDateUtc;
        data.UpdatedByCustomerId = UpdatedByCustomerId;

        return data;
    }
}
