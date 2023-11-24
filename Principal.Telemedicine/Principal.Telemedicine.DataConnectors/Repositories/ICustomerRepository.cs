using Microsoft.EntityFrameworkCore.Storage;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.DataConnectors.Utils;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad objektem Customer.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Metoda vrací všechny nesmazané uživatele včetně Efektivní uživatelů.
    /// </summary>
    /// <param name="providerId">Id Poskytovatele, který uživatele vytvořil</param>
    /// <returns>Seznam uživatelů</returns>
    Task<IEnumerable<Customer>> GetCustomersTaskAsyncTask(int? providerId = null);

    /// <summary>
    /// Metoda vrací všechny uživatele.
    /// </summary>
    /// <returns> Seznam uživatelů </returns>
    public IQueryable<Customer> ListOfAllCustomers();

    /// <summary>
    /// Metoda vrací konkrétního uživatele na základě id.
    /// </summary>
    /// <param name="id">Id uživatele</param>
    /// <returns>Konkrétní uživatel</returns>
    Task<Customer?> GetCustomerInfoByIdTaskAsync(int id);

    /// <summary>
    /// Metoda vrací konkrétního uživatele na základě id se všemi Rolemi, Skupinami a Permissions včetně Subjektů.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Konkrétní uživatel</returns>
    Task<Customer?> GetCustomerByIdTaskAsync(int id);

    /// <summary>
    /// Metoda vrací konkrétního uživatele na základě GlobalId se všemi Rolemi, Skupinami a Permissions včetně Subjektů.
    /// </summary>
    /// <param name="globalId"></param>
    /// <returns>Konkrétní uživatel</returns>
    Task<Customer?> GetCustomerByGlobalIdTaskAsync(string globalId);

    /// <summary>
    /// Metoda vrací potřebná data uživatele určené pouze pro Update uživatele.
    /// Neobsahuje Role, Skupiny a Permissions a Subject protože s těmi se při updatu uživatele nepracuje.
    /// Zato obsahuje i smazané vazby na Role a Skupiny, protože se s nimi při updatu pracuje.
    /// </summary>
    /// <param name="id">Id uživatele</param>
    /// <returns>Konkrétní uživatel</returns>
    Task<Customer?> GetCustomerByIdOnlyForUpdateTaskAsync(int id);

    /// <summary>
    /// Vrací seznam uživatelů pro grid
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="activeUsersOnly">Filtr - pouze aktivní uživatelé</param>
    /// <param name="filterRole">Filtr - pouze uživatelé vybrané role</param>
    /// <param name="filteGroup">Filtr - pouze uživatelé vybrané skupiny</param>
    /// <param name="searchText">Filtr - vyhledání v názvu uživatele a adresy</param>
    /// <param name="order">Řazení (vyýčet: "created_asc", "created_desc", "updated_asc", "updated_desc"</param>
    /// <param name="page">Požadované číslo stránky</param>
    /// <param name="pageSize">Počet záznamů na stránce</param>
    /// <param name="providerId">Id Poskytovatele pod kterým hledáme</param>
    /// <returns>Jednu stránku seznamu</returns>
    Task<PaginatedListData<Customer>> GetCustomersTaskAsync(CompleteUserContract currentUser, bool activeUsersOnly, int? filterRole, int? filteGroup, string? searchText, string? order = "created_desc", int? page = 1, int? pageSize = 20, int? providerId = null);

    /// <summary>
    /// Metoda aktualizuje Customera včetně aktualizace v ADB2C
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="user">Customer</param>
    /// <param name="ignoreADB2C">Příznak, že se má ignorovat volání ADB2C (default FALSE)</param>
    /// <param name="tran">Aktuální DB transakce (default NULL)</param>
    /// <param name="dontManageTran">Příznak, zda se v metodě mají ignorovat transakční příkazy (default FALSE)</param>
    /// <returns>1 - update se povedl nebo:
    /// -1 = globální chyba
    /// -14 = uživatele se nepodařilo uložit
    /// -15 = uživatele se nepodařilo uložit v AD B2C
    /// -16 = uživatel nenalezen v AD B2C
    /// -17 = existuje více shodných uživatelů v AD B2C</returns>
    Task<int> UpdateCustomerTaskAsync(CompleteUserContract currentUser, Customer user, bool? ignoreADB2C = false, IDbContextTransaction? tran = null, bool dontManageTran = false);

    /// <summary>
    /// Metoda založí nového Customera včetně založení v ADB2C
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="user">Customer</param>
    /// <returns>1 - update se povedl nebo:
    /// -1 = globální chyba
    /// -6 = uživatele se nepodařilo založit v DB
    /// -18 = uživatel již existuje v AD B2C
    /// -19 = uživatel se stejným emailem již existuje v AD B2C</returns>
    Task<int> InsertCustomerTaskAsync(CompleteUserContract currentUser, Customer user);

    /// <summary>
    /// Označí užvatele za smazaného a smaže ho z ADB2C
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="user">Customer</param>
    /// <param name="ignoreADB2C">Nemezat v ADB2C?</param>
    /// <returns>true / false</returns>
    Task<bool> DeleteCustomerTaskAsync(CompleteUserContract currentUser, Customer user, bool? ignoreADB2C = false);

    /// <summary>
    /// Zkontroluje, zda uživatel (nesmazaný) již existuje v dedikované DB podel Emailu, tel. čísla 1 a tel. čísla 2, GlobalId nebo PersonalIdentificationNumber
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="user">Uživatel ke kontrole</param>
    /// <returns>0 jako že se shodný uživatel nenalezl nebo:
    /// -10 = uživatel se stejným emailem existuje
    /// -11 = uživatel se stejným tel. číslem existuje
    /// -12 = uživatel se stejným PersonalIdentificationNumber existuje
    /// -13 = uživatel se stejným GlobalID existuje
    /// </returns>
    Task<int> CheckIfUserExists(CompleteUserContract currentUser, Customer user);
}

