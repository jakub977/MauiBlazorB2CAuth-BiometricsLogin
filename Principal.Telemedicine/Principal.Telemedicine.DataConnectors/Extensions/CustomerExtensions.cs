using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Enums;

namespace Principal.Telemedicine.DataConnectors.Extensions;

/// <summary>
/// Pomocná třída Customer
/// </summary>
public static class CustomerExtensions
{
    /// <summary>
    /// Je uživatel Správcem poskytovatele?
    /// </summary>
    /// <param name="customer">Uživatel</param>
    /// <returns>true / false</returns>
    public static bool IsProviderAdmin(this Customer customer)
    {
        bool ret = false;
        if (customer.RoleMemberDirectUsers.Any(a => a.Active.GetValueOrDefault() && !a.Deleted && (a.RoleId == (int)RoleMainEnum.ProviderAdmin || (a.Role.ParentRoleId.HasValue && a.Role.ParentRoleId.Value == (int)RoleMainEnum.ProviderAdmin))))
            ret = true;

        if (!ret)
            if (customer.EffectiveUserUsers.Any(a => a.Active.GetValueOrDefault() && !a.Deleted && a.RoleMembers.Any(w => w.Active.GetValueOrDefault() && !w.Deleted && (w.RoleId == (int)RoleMainEnum.ProviderAdmin || (w.Role.ParentRoleId.HasValue && w.Role.ParentRoleId.Value == (int)RoleMainEnum.ProviderAdmin)))))
                ret = true;
        return ret;
    }

    /// <summary>
    /// Je uživatel Správcem organizace?
    /// </summary>
    /// <param name="customer">Uživatel</param>
    /// <returns>true / false</returns>
    public static bool IsOrganizationAdmin(this Customer customer)
    {
        bool ret = false;
        if (customer.RoleMemberDirectUsers.Any(a => a.Active.GetValueOrDefault() && !a.Deleted && (a.RoleId == (int)RoleMainEnum.OrganizationAdmin || (a.Role.ParentRoleId.HasValue && a.Role.ParentRoleId.Value == (int)RoleMainEnum.OrganizationAdmin))))
            ret = true;

        if (!ret)
            if (customer.EffectiveUserUsers.Any(a => a.Active.GetValueOrDefault() && !a.Deleted && a.RoleMembers.Any(w => w.Active.GetValueOrDefault() && !w.Deleted && (w.RoleId == (int)RoleMainEnum.OrganizationAdmin || (w.Role.ParentRoleId.HasValue && w.Role.ParentRoleId.Value == (int)RoleMainEnum.OrganizationAdmin)))))
                ret = true;
        return ret;
    }

    /// <summary>
    /// Je uživatel globální administrátor?
    /// </summary>
    /// <param name="customer">Uživatel</param>
    /// <returns>true / false</returns>
    public static bool IsGlobalAdmin(this Customer customer)
    {
        bool ret = false;
        if (customer.RoleMemberDirectUsers.Any(a => a.Active.GetValueOrDefault() && !a.Deleted && (a.RoleId == (int)RoleMainEnum.SuperAdmin || (a.Role.ParentRoleId.HasValue && a.Role.ParentRoleId.Value == (int)RoleMainEnum.SuperAdmin))))
            ret = true;

        if (!ret)
            if (customer.EffectiveUserUsers.Any(a => a.Active.GetValueOrDefault() && !a.Deleted && a.RoleMembers.Any(w => w.Active.GetValueOrDefault() && !w.Deleted && (w.RoleId == (int)RoleMainEnum.SuperAdmin || (w.Role.ParentRoleId.HasValue && w.Role.ParentRoleId.Value == (int)RoleMainEnum.SuperAdmin)))))
                ret = true;
        return ret;
    }
}

