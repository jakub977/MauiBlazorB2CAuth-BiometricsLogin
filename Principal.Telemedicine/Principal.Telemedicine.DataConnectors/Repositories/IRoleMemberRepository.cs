using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad objektem RoleMember.
/// </summary>
public interface IRoleMemberRepository
{

    /// <summary>
    /// Metoda vrací všechny členy rolí.
    /// </summary>
    /// <returns>Seznam členů rolí</returns>
    IQueryable<RoleMember> GetAllRoleMembers();


    /// <summary>
    /// Metoda zakládá nového RoleMembera
    /// </summary>
    /// <param name="roleMember">člen role</param>
    /// <returns>true / false</returns>
    Task<bool> InsertRoleMemberTaskAsync(RoleMember roleMember);

    /// <summary>
    /// Metoda aktualizuje RoleMembera
    /// </summary>
    /// <param name="roleMember">člen role</param>
    /// <returns>true / false</returns>
    Task<bool> UpdateRoleMemberTaskAsync(RoleMember roleMember);

    /// <summary>
    /// Metoda odstraní RoleMembera
    /// </summary>
    /// <param name="roleMember">člen role</param>
    /// <returns>true / false</returns>
    Task<bool> DeleteRoleMemberTaskAsync(RoleMember roleMember);

}
