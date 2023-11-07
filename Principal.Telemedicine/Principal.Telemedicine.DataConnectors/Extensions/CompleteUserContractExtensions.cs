using Principal.Telemedicine.Shared.Enums;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.DataConnectors.Extensions;

/// <summary>
/// Pomocná třída CompleteUserContractExtensions
/// </summary>
public static class CompleteUserContractExtensions
{
    /// <summary>
    /// Je uživatel Správcem poskytovatele?
    /// </summary>
    /// <param name="customer">Uživatel</param>
    /// <returns>true / false</returns>
    public static bool IsProviderAdmin(this CompleteUserContract customer)
    {
        bool ret = false;
        if (customer.RoleMemberDirectUsers.Any(a => a.Active && !a.Deleted && (a.RoleId == (int)RoleMainEnum.ProviderAdmin || (a.Role.ParentRoleId.HasValue && a.Role.ParentRoleId.Value == (int)RoleMainEnum.ProviderAdmin))))
            ret = true;

        if (!ret)
            if (customer.EffectiveUserUsers.Any(a => a.Active && !a.Deleted && a.RoleMembers.Any(w => w.Active && !w.Deleted && (w.RoleId == (int)RoleMainEnum.ProviderAdmin || (w.Role.ParentRoleId.HasValue && w.Role.ParentRoleId.Value == (int)RoleMainEnum.ProviderAdmin)))))
                ret = true;
        return ret;
    }

    /// <summary>
    /// Jedná se o uživatele Výzkum?
    /// </summary>
    /// <param name="customer">Uživatel</param>
    /// <returns>true / false</returns>
    public static bool IsResearch(this CompleteUserContract customer)
    {
        bool ret = false;
        if (customer.RoleMemberDirectUsers.Any(a => a.Active && !a.Deleted && (a.RoleId == (int)RoleMainEnum.Research || (a.Role.ParentRoleId.HasValue && a.Role.ParentRoleId.Value == (int)RoleMainEnum.Research))))
            ret = true;

        if (!ret)
            if (customer.EffectiveUserUsers.Any(a => a.Active && !a.Deleted && a.RoleMembers.Any(w => w.Active && !w.Deleted && (w.RoleId == (int)RoleMainEnum.Research || (w.Role.ParentRoleId.HasValue && w.Role.ParentRoleId.Value == (int)RoleMainEnum.Research)))))
                ret = true;
        return ret;
    }

    /// <summary>
    /// Je uživatel Správcem organizace?
    /// </summary>
    /// <param name="customer">Uživatel</param>
    /// <returns>true / false</returns>
    public static bool IsOrganizationAdmin(this CompleteUserContract customer)
    {
        bool ret = false;
        if (customer.RoleMemberDirectUsers.Any(a => a.Active && !a.Deleted && (a.RoleId == (int)RoleMainEnum.OrganizationAdmin || (a.Role.ParentRoleId.HasValue && a.Role.ParentRoleId.Value == (int)RoleMainEnum.OrganizationAdmin))))
            ret = true;

        if (!ret)
            if (customer.EffectiveUserUsers.Any(a => a.Active && !a.Deleted && a.RoleMembers.Any(w => w.Active && !w.Deleted && (w.RoleId == (int)RoleMainEnum.OrganizationAdmin || (w.Role.ParentRoleId.HasValue && w.Role.ParentRoleId.Value == (int)RoleMainEnum.OrganizationAdmin)))))
                ret = true;
        return ret;
    }

    /// <summary>
    /// Je uživatel globální administrátor?
    /// </summary>
    /// <param name="customer">Uživatel</param>
    /// <returns>true / false</returns>
    public static bool IsGlobalAdmin(this CompleteUserContract customer)
    {
        bool ret = false;
        if (customer.RoleMemberDirectUsers.Any(a => a.Active && !a.Deleted && (a.RoleId == (int)RoleMainEnum.SuperAdmin || (a.Role.ParentRoleId.HasValue && a.Role.ParentRoleId.Value == (int)RoleMainEnum.SuperAdmin))))
            ret = true;

        if (!ret)
            if (customer.EffectiveUserUsers.Any(a => a.Active && !a.Deleted && a.RoleMembers.Any(w => w.Active && !w.Deleted && (w.RoleId == (int)RoleMainEnum.SuperAdmin || (w.Role.ParentRoleId.HasValue && w.Role.ParentRoleId.Value == (int)RoleMainEnum.SuperAdmin)))))
                ret = true;
        return ret;
    }
}

