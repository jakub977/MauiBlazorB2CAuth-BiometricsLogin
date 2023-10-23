﻿using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Slouží pro správu Poskytovatelů
/// </summary>
[DataContract]
public class EffectiveUserProviderContract
{
    /// <summary>
    /// Primary identifier of an effective user
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if effective user is active
    /// </summary>
    public bool Active { get; set; }

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
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates effective user
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of effective user update, using coordinated universal time
    /// </summary>
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who is effective user, i.e. member of provider
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a provider of which user is member
    /// </summary>
    public int ProviderId { get; set; }

    [JsonPropertyName("roles")]
    public virtual ICollection<RoleMemberProviderContract> RoleMembers { get; set; } = new List<RoleMemberProviderContract>();

    [JsonPropertyName("groups")]
    public virtual ICollection<GroupEffectiveMemberContract> GroupEffectiveMembers { get; set; } = new List<GroupEffectiveMemberContract>();

    [JsonPropertyName("providerObject")]
    public virtual ProviderContract? Provider { get; set; }

}
