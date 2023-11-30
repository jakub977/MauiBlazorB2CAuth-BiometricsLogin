using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.DataConnectors.Utils;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad objektem RoleMember.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Metoda vrací všechny role.
    /// </summary>
    /// <returns>Seznam členů rolí</returns>
    IQueryable<Role> ListOfAllRoles();

    /// <summary>
    /// Vrací seznam rolí pro grid
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="activeRolesOnly">Filtr - pouze aktivní role</param>
    /// <param name="searchText">Filtr - vyhledání v názvu role</param>
    /// <param name="filterRoleCategoryId">Filtr - pouze vybrané kategorie role</param>
    /// <param name="filterAvailability">Filtr - pouze role vybrané dostupnosti</param>
    /// <param name="showHidden">Zobrazit i smazané záznamy?</param>
    /// <param name="showSpecial">Příznak, že se jedná o uživatele v Roli Super admin nebo Správce organizace</param>
    /// <param name="order">Řazení (vyýčet: "created_asc", "created_desc", "updated_asc", "updated_desc"</param>
    /// <param name="page">Požadované číslo stránky</param>
    /// <param name="pageSize">Počet záznamů na stránce</param>
    /// <param name="providerId">Id Poskytovatele pod kterým hledáme</param>
    /// <param name="organizationId">Id Organizace</param>
    /// <returns>Jednu stránku seznamu</returns>
    Task<IEnumerable<Role>> GetRolesForGridTaskAsync(CompleteUserContract currentUser, bool activeRolesOnly, string? searchText, int? filterRoleCategoryId, int? filterAvailability, bool showHidden = false, bool showSpecial = false, string? order = "created_desc", int? providerId = null, int? organizationId = null);

    /// <summary>
    /// Vrací seznam rolí pro dropdown list
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="providerId">Id Poskytovatele pod kterým hledáme</param>
    /// <param name="roleIds">Seznam ID rolí, které chceme mít v seznamu bez ohledu na ostatní podmínky</param>
    /// <returns>Seznam rolí</returns>
    Task<IEnumerable<Role>> GetRolesForDropdownListTaskAsync(CompleteUserContract currentUser, int providerId, List<int>? roleIds);


}
