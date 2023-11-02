using Principal.Telemedicine.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of user roles
/// </summary>
[Table("Role")]
public partial class Role
{
    /// <summary>
    /// Primary identifier of a role
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if role is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if role is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates role
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of role creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates role
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of role update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Organization as a parent organization
    /// </summary>
    public int? OrganizationId { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a parent provider
    /// </summary>
    public int? ProviderId { get; set; }

    /// <summary>
    /// Bit identifier if role is global or custom (0 = global, 1 = custom). Global roles are created by super admins.
    /// </summary>
    public bool IsGlobal { get; set; }

    /// <summary>
    /// Name of a role
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a role
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Link to dbo.Role as a parent role, i.e. reference to original role
    /// </summary>
    public int? ParentRoleId { get; set; }

    /// <summary>
    /// Link to dbo.RoleCategoryCombination as a combination of role category and its subcategory
    /// </summary>
    public int RoleCategoryCombinationId { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a role
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("RoleCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Inverse collection of Role as a inverse parent role
    /// </summary>
    [InverseProperty("ParentRole")]
    public virtual ICollection<Role> InverseParentRole { get; set; } = new List<Role>();

    /// <summary>
    /// Link to dbo.Organization as a parent organization
    /// </summary>
    [ForeignKey("OrganizationId")]
    [InverseProperty("Roles")]
    public virtual Organization? Organization { get; set; }

    /// <summary>
    /// Link to dbo.Role as a parent role, i.e. reference to original role
    /// </summary>
    [ForeignKey("ParentRoleId")]
    [InverseProperty("InverseParentRole")]
    public virtual Role? ParentRole { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a provider to whom role relates
    /// </summary>
    [ForeignKey("ProviderId")]
    [InverseProperty("Roles")]
    public virtual Provider? Provider { get; set; }

    /// <summary>
    /// Link to dbo.RoleCategoryCombination as a combination of role category and its subcategory
    /// </summary>
    [ForeignKey("RoleCategoryCombinationId")]
    [InverseProperty("Roles")]
    public virtual RoleCategoryCombination RoleCategoryCombination { get; set; } = null!;

    /// <summary>
    /// Inverse collection of RoleMembers to whom is a role related"
    /// </summary>
    [InverseProperty("Role")]
    public virtual ICollection<RoleMember> RoleMembers { get; set; } = new List<RoleMember>();

    /// <summary>
    /// Inverse collection of RolePermissions to which is a role related
    /// </summary>
    [InverseProperty("Role")]
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    /// <summary>
    /// Link to dbo.Customer as an user who updates a role
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RoleUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    /// <summary>
    /// Převede Role na RoleProviderContract pro správu Poskytovatelů
    /// </summary>
    /// <param name="parent">RoleProviderContract rodiče</param>
    /// <returns>RoleProviderContract</returns>
    public RoleProviderContract ConvertToRoleProviderContract(RoleProviderContract? parent = null)
    {
        RoleProviderContract data = new RoleProviderContract();
        data.IsGlobal = this.IsGlobal;
        data.Active = this.Active;
        data.Id = this.Id;
        data.CreatedByCustomerId = this.CreatedByCustomerId;
        data.CreatedDateUtc = this.CreatedDateUtc;
        data.Deleted = this.Deleted;
        data.UpdateDateUtc = this.UpdateDateUtc;
        data.UpdatedByCustomerId = this.UpdatedByCustomerId;
        data.OrganizationId = this.OrganizationId;
        data.ParentRoleId = this.ParentRoleId;
        data.Name = this.Name;
        data.Description = this.Description;
       
        if (parent != null)
            data.ParentRoleContract = parent;

        return data;
    }

    /// <summary>
    /// Vrátí RoleContract z Role
    /// </summary>
    /// <param name="withProvider">Příznak, zda chceme vrátit i data Poskytovatele (default TRUE)</param>
    /// <param name="withPermissions">Příznak, zda chceme vrátit i data Permissions (default TRUE)</param>
    /// <param name="withSubject">Příznak, zda chceme vrátit i data Subjektu (default TRUE)</param>
    /// <param name="withRolesAndGroupsDetail">Příznak, zda chceme vrátit i podrobnější data jako kategorie nebo typ Role / Skupiny (default FALSE)</param>
    /// <returns>RoleContract</returns>
    public RoleContract ConvertToRoleContract(bool withProvider = true, bool withPermissions = true, bool withSubject = true, bool withRolesAndGroupsDetail = false)
    {
        RoleContract data = new RoleContract();

        data.Active = Active.GetValueOrDefault();
        data.CreatedByCustomerId = CreatedByCustomerId; 
        data.CreatedDateUtc = CreatedDateUtc;
        data.Deleted = Deleted;
        data.Description = Description;
        data.Id = Id;
        data.IsGlobal = IsGlobal;
        data.Name = Name;
        data.OrganizationId = OrganizationId;

        data.ParentRoleId = ParentRoleId;

        if (ParentRole != null)
            data.ParentRole = ParentRole.ConvertToRoleContract(withProvider, withPermissions, withSubject, withRolesAndGroupsDetail);

        data.ProviderId = ProviderId;

        if (withRolesAndGroupsDetail && withProvider && Provider != null)
            data.Provider = Provider.ConvertToProviderContract(false, false);

        data.RoleCategoryCombinationId = RoleCategoryCombinationId;
        
        if (withRolesAndGroupsDetail && RoleCategoryCombination != null)
            data.RoleCategoryCombination = RoleCategoryCombination.ConvertToRoleCategoryCombinationContract();

        if (withPermissions && RolePermissions != null && RolePermissions.Count > 0)
            foreach(RolePermission permission in RolePermissions)
                data.RolePermissions.Add(permission.ConvertToRolePermissionContract(withSubject));

        data.UpdateDateUtc = UpdateDateUtc;
        data.UpdatedByCustomerId = UpdatedByCustomerId;

        return data;
    }
}
