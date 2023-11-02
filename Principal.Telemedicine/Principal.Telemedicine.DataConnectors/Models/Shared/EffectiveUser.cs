using Principal.Telemedicine.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of effective users, i.e. members of provider. We also distinguish direct users, who are members of an organization only and not of a provider (these are users in dbo.Customer without row in this table).
/// </summary>
[Table("EffectiveUser")]
public partial class EffectiveUser
{
    /// <summary>
    /// Primary identifier of an effective user
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if effective user is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if effective user is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates effective user
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of effective user creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates effective user
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of effective user update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who is effective user, i.e. member of provider
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a provider of which user is member
    /// </summary>
    public int ProviderId { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who created effective user
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("EffectiveUserCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Inverse collection of effective users GroupEffectiveMembers
    /// </summary>
    [InverseProperty("EffectiveUser")]
    public virtual ICollection<GroupEffectiveMember> GroupEffectiveMembers { get; set; } = new List<GroupEffectiveMember>();

    /// <summary>
    /// Link to dbo.Provider as a provider of which user is member
    /// </summary>
    [ForeignKey("ProviderId")]
    [InverseProperty("EffectiveUsers")]
    public virtual Provider Provider { get; set; } = null!;

    /// <summary>
    /// Inverse collection of effective users RoleMembers
    /// </summary>
    [InverseProperty("EffectiveUser")]
    public virtual ICollection<RoleMember> RoleMembers { get; set; } = new List<RoleMember>();

    /// <summary>
    /// Link to dbo.Customer as an user who updates effective user
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("EffectiveUserUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an users EffectiveUser
    /// </summary>
    [ForeignKey("UserId")]
    [InverseProperty("EffectiveUserUsers")]
    public virtual Customer User { get; set; } = null!;

    /// <summary>
    /// Převede EffectiveUser na EffectiveUserProviderContract který se využívá pro sekci Poskytovatelů.
    /// Skupiny ve kterých je EffectiveUser nás u Poskytovatelů nezajímají, pouze Role
    /// </summary>
    /// <param name="roles">Seznam RoleMemberProviderContract</param>
    /// <returns>EffectiveUserProviderContract</returns>
    public EffectiveUserProviderContract ConvertToEffectiveUserProviderContract(ICollection<RoleMemberProviderContract>? roles = null)
    {
        EffectiveUserProviderContract data = new EffectiveUserProviderContract();
        data.UserId = this.UserId;
        data.ProviderId = this.ProviderId;
        data.Active = this.Active.GetValueOrDefault();
        data.Id = this.Id;
        data.CreatedByCustomerId = this.CreatedByCustomerId;
        data.CreatedDateUtc = this.CreatedDateUtc;
        data.Deleted = this.Deleted;   
        data.UpdateDateUtc = this.UpdateDateUtc;
        data.UpdatedByCustomerId = this.UpdatedByCustomerId;
        if (roles != null)
            data.RoleMembers = roles;

        return data;
    }

    /// <summary>
    /// Vrátí EffectiveUserContract z EffectiveUser
    /// </summary>
    /// <param name="withProvider">Příznak, zda chceme vrátit i data Poskytovatele a to i v Rolích (default TRUE)</param>
    /// <param name="withRolesAndGroups">Příznak, zda chceme vrátit i Role a Skupiny (default TRUE)</param>
    /// <param name="withPermissions">Příznak, zda chceme vrátit Role nebo Skupiny i s Permissions (default TRUE)</param>
    /// <param name="withPermissionSubject">Příznak, zda chceme vrátit v Permissions i data Subjektu (default TRUE)</param>
    /// <param name="withRolesAndGroupsDetail">Příznak, zda chceme vrátit i podrobnější data jako kategorie nebo typ Role / Skupiny (default FALSE)</param>
    /// <returns></returns>
    public EffectiveUserContract ConvertToEffectiveUserContract(bool withProvider = true, bool withRolesAndGroups = true, bool withPermissions = true, bool withPermissionSubject = true, bool withRolesAndGroupsDetail = false)
    {
        EffectiveUserContract data = new EffectiveUserContract();
        data.UserId = this.UserId;
        data.ProviderId = this.ProviderId;

        if (withProvider && Provider != null)
            data.Provider = Provider.ConvertToProviderContract(false, false);

        data.Active = this.Active.GetValueOrDefault();
        data.Id = this.Id;
        data.CreatedByCustomerId = this.CreatedByCustomerId;
        data.CreatedDateUtc = this.CreatedDateUtc;
        data.Deleted = this.Deleted;
        data.UpdateDateUtc = this.UpdateDateUtc;
        data.UpdatedByCustomerId = this.UpdatedByCustomerId;

        if (withRolesAndGroups && RoleMembers != null && RoleMembers.Count > 0)
            foreach (var roleMember in RoleMembers)
                data.RoleMembers.Add(roleMember.ConvertToRoleMemberContract(withRolesAndGroups, withProvider, withPermissions, withPermissionSubject, withRolesAndGroupsDetail));

        if (withRolesAndGroups && GroupEffectiveMembers != null && GroupEffectiveMembers.Count > 0)
            foreach (var groupMember in GroupEffectiveMembers)
                data.GroupEffectiveMembers.Add(groupMember.ConvertToGroupEffectiveMemberContract(withRolesAndGroups, withProvider, withPermissions, withPermissionSubject, withRolesAndGroupsDetail));

        return data;
    }
}
