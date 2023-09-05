using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from UserPermission.cs
/// </summary>
[DataContract]
public class UserPermissionContract
{
    /// <summary>
    /// Primary identifier of an user permission
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if user permission is active
    /// </summary>
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
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates user permission
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of user permission update, using coordinated universal time
    /// </summary>
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


    //public virtual UserContract CreatedByCustomer { get; set; } = null!;

    //public virtual PermissionContract Permission { get; set; } = null!;

    //public virtual ProviderContract? Provider { get; set; }

    //public virtual UserContract? UpdatedByCustomer { get; set; }

    //public virtual UserContract User { get; set; } = null!;
}
