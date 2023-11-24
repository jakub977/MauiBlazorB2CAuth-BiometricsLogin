using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Enums;

namespace Principal.Telemedicine.DataConnectors.Extensions;

/// <summary>
/// Pomocná třída EffectiveUser
/// </summary>
public static class EffectiveUserExtensions
{
    /// <summary>
    /// Je uživatel Správcem poskytovatele?
    /// </summary>
    /// <param name="ef">Uživatel</param>
    /// <returns>true / false</returns>
    public static bool IsProviderAdmin(this EffectiveUser ef)
    {
        bool ret = false;
            if (ef.RoleMembers.Any(w => w.Active.GetValueOrDefault() && !w.Deleted && (w.RoleId == (int)RoleMainEnum.ProviderAdmin || (w.Role.ParentRoleId.HasValue && w.Role.ParentRoleId.Value == (int)RoleMainEnum.ProviderAdmin))))
                ret = true;
        return ret;
    }

    /// <summary>
    /// Jedná se o uživatele Výzkum?
    /// </summary>
    /// <param name="ef">Uživatel</param>
    /// <returns>true / false</returns>
    public static bool IsResearch(this EffectiveUser ef)
    {
        bool ret = false;

            if (ef.RoleMembers.Any(w => w.Active.GetValueOrDefault() && !w.Deleted && (w.RoleId == (int)RoleMainEnum.Research || (w.Role.ParentRoleId.HasValue && w.Role.ParentRoleId.Value == (int)RoleMainEnum.Research))))
                ret = true;
        return ret;
    }
}

