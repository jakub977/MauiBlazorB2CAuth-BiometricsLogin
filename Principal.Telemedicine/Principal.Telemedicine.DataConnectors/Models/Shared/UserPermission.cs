using Principal.Telemedicine.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of permissions which are explicitly granted or denied to users beyond permissions already granted based on assigned roles or memberships in any of organizational units
/// </summary>
[Table("UserPermission")]
public partial class UserPermission
{
    /// <summary>
    /// Primary identifier of an user permission
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if user permission is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if user permission is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates user permission
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of user permission creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates user permission
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of user permission update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user to whom permission is granted or denied
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a provider for which exception is valid
    /// </summary>
    public int? ProviderId { get; set; }

    /// <summary>
    /// Link to dbo.Permission as a permission which is granted or denied to user
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// Bit identifier if permission is denied (1) or granted (0) to user beyond the granted permission based on assigned role or membership in an organizational unit
    /// </summary>
    public bool IsDeniedPermission { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a user permission
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("UserPermissionCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Permission as a permission which is granted or denied to user
    /// </summary>
    [ForeignKey("PermissionId")]
    [InverseProperty("UserPermissions")]
    public virtual Permission Permission { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Provider as a provider for which exception is valid
    /// </summary>
    [ForeignKey("ProviderId")]
    [InverseProperty("UserPermissions")]
    public virtual Provider? Provider { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a user permission
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("UserPermissionUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user to whom permission is granted or denied
    /// </summary>
    [ForeignKey("UserId")]
    [InverseProperty("UserPermissionUsers")]
    public virtual Customer User { get; set; } = null!;

    /// <summary>
    /// Vrátí UserPermissionContract z UserPermission
    /// </summary>
    /// <param name="withSubject">Příznak, zda chceme vrátit v Permission i Subject (default TRUE)</param>
    /// <returns>UserPermissionContract</returns>
    public UserPermissionContract ConvertToUserPermissionContract(bool withSubject = true)
    {
        UserPermissionContract data = new UserPermissionContract();

        data.Active = Active;
        data.CreatedByCustomerId = CreatedByCustomerId;
        data.CreatedDateUtc = CreatedDateUtc;
        data.Deleted = Deleted;
        data.Id = Id;
        data.IsDeniedPermission = IsDeniedPermission;
        data.PermissionId = PermissionId;
        
        if (Permission != null)
            data.Permission = Permission.ConvertToPermissionContract(withSubject);

        data.ProviderId = ProviderId;
        data.UpdateDateUtc = UpdateDateUtc;
        data.UpdatedByCustomerId = UpdatedByCustomerId;
        data.UserId = UserId;

        return data;
    }
}
