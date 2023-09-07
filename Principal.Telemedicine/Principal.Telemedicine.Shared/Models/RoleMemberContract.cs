using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from RoleMember.cs
/// </summary>
[DataContract]
public class RoleMemberContract
{
    /// <summary>
    /// Primary identifier of a role member
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a role member is active
    /// </summary>
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
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a role member
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of role member update, using coordinated universal time
    /// </summary>
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

   // public virtual UserContract? UpdatedByCustomer { get; set; }

    // public virtual UserContract? DirectUser { get; set; }


    //public virtual UserContract CreatedByCustomer { get; set; } = null!;

   // public virtual EffectiveUserContract? EffectiveUser { get; set; } //todo: zde odkomentovat

    //public virtual RoleContract Role { get; set; } = null!;


}
