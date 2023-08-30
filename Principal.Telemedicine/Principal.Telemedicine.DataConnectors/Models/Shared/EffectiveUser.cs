using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("EffectiveUserCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    [InverseProperty("EffectiveUser")]
    public virtual ICollection<GroupEffectiveMember> GroupEffectiveMembers { get; set; } = new List<GroupEffectiveMember>();

    [ForeignKey("ProviderId")]
    [InverseProperty("EffectiveUsers")]
    public virtual Provider Provider { get; set; } = null!;

    [InverseProperty("EffectiveUser")]
    public virtual ICollection<RoleMember> RoleMembers { get; set; } = new List<RoleMember>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("EffectiveUserUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("EffectiveUserUsers")]
    public virtual Customer User { get; set; } = null!;
}
