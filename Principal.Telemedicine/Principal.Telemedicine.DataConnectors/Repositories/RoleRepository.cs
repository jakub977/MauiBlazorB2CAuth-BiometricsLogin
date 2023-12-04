using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Extensions;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Enums;
using Principal.Telemedicine.Shared.Models;
using System.Data.Common;
using System.Data;

namespace Principal.Telemedicine.DataConnectors.Repositories;


public class RoleRepository : IRoleRepository
{
    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly string _logName = "RoleRepository";

    public RoleRepository(DbContextApi dbContext, ILogger<RoleRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IQueryable<Role> ListOfAllRoles()
    {
        return _dbContext.Roles.Include(c => c.CreatedByCustomer)
            .Include(c => c.UpdatedByCustomer)
            .Include(c => c.Organization)
            .Include(c => c.Provider)
            .Include(c => c.RoleCategoryCombination)
            .Include(c => c.RoleMembers).ThenInclude(m => m.EffectiveUser).ThenInclude(e => e.User)
            .DefaultIfEmpty();
    }

    /// <inheritdoc/>
    public async Task<Role?> GetRoleByIdTaskAsync(int id)
    {
        var data = await _dbContext.Roles.Include(c => c.CreatedByCustomer)
            .Include(c => c.UpdatedByCustomer)
            .Include(c => c.Organization)
            .Include(c => c.Provider)
            .Include(c => c.RoleCategoryCombination)
            .Include(c => c.RoleMembers).ThenInclude(m => m.EffectiveUser).ThenInclude(e => e.User)
            .Where(c => c.Id == id).FirstOrDefaultAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Role>> GetRolesForGridTaskAsync(CompleteUserContract currentUser, bool activeRolesOnly, string? searchText, int? filterRoleCategoryId, int? filterAvailability, bool showHidden = false, bool showSpecial = false, string? order = "created_desc", int? providerId = null, int? organizationId = null)
    {
        DateTime startTime = DateTime.Now;
        var query = _dbContext.Roles.Include(c => c.ParentRole)
            .Include(c => c.Organization)
            .Include(c => c.Provider)
            .Include(c => c.RolePermissions.Where(w => !w.Deleted)).ThenInclude(rp => rp.Permission).ThenInclude(p => p.Subject).ThenInclude(p => p.ParentSubject)
            .Include(c => c.RoleCategoryCombination)
            .Include(c => c.RoleMembers.Where(w => !w.Deleted)).AsSplitQuery().DefaultIfEmpty().AsQueryable();

        TimeSpan timeMiddle = DateTime.Now - startTime;
        // filtrování smazaných záznamů
        if (!showHidden)
            query = query.Where(a => !a.Deleted);

        // filtrování podle stavu Active role
        if (activeRolesOnly)
        {
            query = query.Where(w => w.Active == activeRolesOnly);
        }

        // filtrování podle dostupnosti role
        if (filterAvailability != null)
        {
            if (filterAvailability > 0)
                query = query.Where(w => w.IsGlobal == (filterAvailability == 1));
        }

        // filtrování podle kategorie role
        if (filterRoleCategoryId != null)
        {
            if (filterRoleCategoryId > 0)
                query = query.Where(w => w.IsGlobal == (filterRoleCategoryId == 1));
        }
        
        // filtrování podle názvu role
        if (searchText != null)
        {
            if (!string.IsNullOrEmpty(searchText))
                query = query.Where(w => w.Name.Contains(searchText));
        }

        query = GetQueryAccordingUserRole(query, currentUser, organizationId, providerId, showSpecial);

        // řazení
        switch (order)
        {
            case "created_desc":
                query = query.OrderByDescending(o => o.CreatedDateUtc);
                break;
            case "created_asc":
                query = query.OrderBy(o => o.CreatedDateUtc);
                break;
            case "updated_desc":
                query = query.OrderByDescending(o => o.UpdateDateUtc).ThenByDescending(o => o.CreatedDateUtc);
                break;
            case "updated_asc":
                query = query.OrderBy(o => o.UpdateDateUtc).ThenBy(o => o.CreatedDateUtc);
                break;
            default:
                break;
        }

        TimeSpan timeEnd = DateTime.Now - startTime;

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Role>> GetRolesForDropdownListTaskAsync(CompleteUserContract currentUser, int providerId, List<int>? roleIds)
    {
        DateTime startTime = DateTime.Now;
        var query = _dbContext.Roles.Include(c => c.ParentRole)
            .Include(c => c.Organization)
            .Include(c => c.Provider)
            .Include(c => c.ParentRole)
            .Include(c => c.RoleCategoryCombination)
            .AsSplitQuery().DefaultIfEmpty().AsQueryable();

        query = query.Where(c => !c.Deleted);
        query = GetQueryAccordingUserRole(query, currentUser, currentUser.OrganizationId, providerId);

        if (roleIds.Count > 0 && roleIds != null)
        {
            var otherRoles = ListOfAllRoles().Where(w => roleIds.Contains(w.Id));
            query = query.Concat(otherRoles);
        }

        return await query.OrderBy(o => o.Name).ToListAsync();
    }
    /// <summary>
    /// Poskládá výsledný dotaz s ohledem na zařazení aktuálního uživatele do rolí
    /// </summary>
    /// <param name="query">Dotaz</param>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="organizationId">Id Organizace</param>
    /// <param name="providerId">Id Podkytovatele</param>
    /// <param name="showSpecial">Příznak, že uživatel v Roli Super admin nebo Správce organizace upravuje uživatele v Roli Super admin nebo Správce organizace a vracíme mu role, na které má právo</param>
    /// <returns></returns>
    private IQueryable<Role> GetQueryAccordingUserRole(IQueryable<Role> query, CompleteUserContract currentUser, int? organizationId = null, int? providerId = null, bool showSpecial = false)
    {
        // Role, které může vidět Super Admin
        if (currentUser.IsGlobalAdmin())
        {
            if (!showSpecial)
                query = query.Where(w => (w.IsGlobal && !w.OrganizationId.HasValue) // globální bez organizace
                || (w.IsGlobal && w.OrganizationId.HasValue && w.OrganizationId.Value == currentUser.OrganizationId.Value) // globální s naší organizací
                || (w.ProviderId.HasValue && w.ProviderId.Value == providerId) // aktuálního poskytovatele
                );
            else
                // globální, bez vazby na poskytovatele
                query = query.Where(w => w.IsGlobal && !w.ProviderId.HasValue && (!w.OrganizationId.HasValue || (w.OrganizationId.HasValue && w.OrganizationId.Value == currentUser.OrganizationId.Value)));
        }
        // Role, které může vidět Správce organizace
        else if (currentUser.IsOrganizationAdmin())
        {
            // vše pod svou organizací
            query = query.Where(w => w.OrganizationId.HasValue && w.OrganizationId.Value == currentUser.OrganizationId.Value);
            // nemůže vidět role Super Admina
            query = query.Where(w => w.Id != (int)RoleMainEnum.SuperAdmin && w.ParentRoleId != (int)RoleMainEnum.SuperAdmin);
            // vidí jen nepřiřazené Role, tj. jen ty jím vytvořené
            query = query.Where(w => !w.ProviderId.HasValue);
            // nemůže vidět role Správce Organizace
            if (!showSpecial)
                query = query.Where(w => w.Id != (int)RoleMainEnum.OrganizationAdmin && w.ParentRoleId != (int)RoleMainEnum.OrganizationAdmin);
        }
        // Role, které může vidět Výzkum
        else if (currentUser.IsResearch())
        {
            // vše pod svou organizací a pouze roli výzkum
            query = query.Where(w => w.OrganizationId.HasValue && w.OrganizationId.Value == currentUser.OrganizationId.Value && (w.Id == (int)RoleMainEnum.Research || w.ParentRoleId == (int)RoleMainEnum.Research));
        }
        // Role, které může vidět Správce Poskytovatele
        else if (currentUser.IsProviderAdmin())
        {
            // jen své organizace
            query = query.Where(w => w.OrganizationId.HasValue && w.OrganizationId.Value == currentUser.OrganizationId.Value);
            // jen svého poskytovatele nebo všechny globální (myšleny ne-admin globální role) 
            query = query.Where(w => (!w.IsGlobal && w.ProviderId == providerId) || w.IsGlobal);
            // nemůže vidět role Super Admina
            query = query.Where(w => w.Id != (int)RoleMainEnum.SuperAdmin && w.ParentRoleId != (int)RoleMainEnum.SuperAdmin);
            // nemůže vidět role Správce Organizace
            query = query.Where(w => w.Id != (int)RoleMainEnum.OrganizationAdmin && w.ParentRoleId != (int)RoleMainEnum.OrganizationAdmin);
            // nemůže vidět role Správce poskytovatele
            if (!showSpecial)
                query = query.Where(w => w.Id != (int)RoleMainEnum.ProviderAdmin && w.ParentRoleId != (int)RoleMainEnum.ProviderAdmin);

        }
        // normální uživatel
        else
        {
            // jen své organizace
            query = query.Where(w => w.OrganizationId.HasValue && w.OrganizationId.Value == currentUser.OrganizationId.Value);
            // jen svého poskytovatele nebo všechny globální (myšleny ne-admin globální role) 
            query = query.Where(w => (!w.IsGlobal && w.ProviderId == providerId) || w.IsGlobal);
            // nemůže vidět role Super Admina
            query = query.Where(w => w.Id != (int)RoleMainEnum.SuperAdmin && w.ParentRoleId != (int)RoleMainEnum.SuperAdmin);
            // nemůže vidět role Správce Organizace
            query = query.Where(w => w.Id != (int)RoleMainEnum.OrganizationAdmin && w.ParentRoleId != (int)RoleMainEnum.OrganizationAdmin);
            // nemůže vidět role Správce poskytovatele
            query = query.Where(w => w.Id != (int)RoleMainEnum.ProviderAdmin && w.ParentRoleId != (int)RoleMainEnum.ProviderAdmin);
        }

        return query;
    }

    /// <inheritdoc/>
    public async Task<int> InsertRoleTaskAsync(CompleteUserContract currentUser, Role role, bool cloneRole)
    {
        int ret = -1;
        string logHeader = _logName + ".InsertRoleTaskAsync:";
        DateTime startTime = DateTime.Now;
        Role actualRole = null;

        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            actualRole = role;
            actualRole.Active = true;
            actualRole.Deleted = false;
            actualRole.CreatedByCustomerId = currentUser.Id;
            actualRole.CreatedDateUtc = DateTime.UtcNow;

            foreach (RolePermission item in actualRole.RolePermissions)
            {
                // nový záznam
                item.CreatedByCustomerId = currentUser.Id;
                item.CreatedDateUtc = DateTime.UtcNow;
                item.Deleted = false;
                item.Active = true;
            }
           
            _dbContext.Roles.Add(actualRole);

            int result = await _dbContext.SaveChangesAsync();

            TimeSpan timeEnd = DateTime.Now - startTime;

            if (result == 1)
            {
                tran.Commit();
                ret = actualRole.Id;
                _logger.LogInformation($"{logHeader} Role '{actualRole.Name}', Id: {actualRole.Id} created succesfully, duration: {timeEnd}");
            }
            else
            {
                tran.Rollback();
                ret = -6;
                _logger.LogWarning($"{logHeader} Role '{actualRole.Name}', Id: {actualRole.Id} was not created, duration: {timeEnd}");
            }

            if (cloneRole)
            {
                bool cloneSuccess = await CloneRole(actualRole.Id);
                if (!cloneSuccess)
                {
                    tran.Rollback();
                    ret = -7;
                    _logger.LogWarning($"{logHeader} Role '{actualRole.Name}', Id: {actualRole.Id} was not cloned, duration: {timeEnd}");
                }
            }
        }
        catch (Exception ex)
        {
            tran.Rollback();
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError($"{logHeader} Role '{actualRole.Name}', Id: {actualRole.Id} was not created, Error: {errMessage}");
        }

        return ret;
    }

    /// <inheritdoc/>
    public async Task<int> UpdateRoleTaskAsync(CompleteUserContract currentUser, Role dbRole, Role editedRole, bool cloneRole)
    {
        int ret = -1;
        string logHeader = _logName + ".UpdateRoleTaskAsync:";
        DateTime startTime = DateTime.Now;

        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            dbRole.Active = editedRole.Active;
            dbRole.Name = editedRole.Name;
            dbRole.ProviderId = editedRole.ProviderId;
            dbRole.RoleCategoryCombinationId = editedRole.RoleCategoryCombinationId;
            dbRole.ParentRoleId = editedRole.ParentRoleId;
            dbRole.UpdatedByCustomerId = currentUser.Id;
            dbRole.UpdateDateUtc = DateTime.UtcNow;

            // nová a existující práva
            foreach (RolePermission item in editedRole.RolePermissions)
            {
                // dohledáme existující oprávnění
                var existingPermission = dbRole.RolePermissions.Where(w => w.PermissionId == item.PermissionId).FirstOrDefault();
                if (existingPermission != null)
                {
                    existingPermission.UpdatedByCustomerId = currentUser.Id;
                    existingPermission.UpdateDateUtc = DateTime.UtcNow;
                    existingPermission.Deleted = false;
                    existingPermission.Active = true;
                }
                else
                {
                    // nové oprávnění
                    item.CreatedByCustomerId = currentUser.Id;
                    item.CreatedDateUtc = DateTime.UtcNow;
                    item.Deleted = false;
                    item.Active = true;
                    dbRole.RolePermissions.Add(item);
                }
            }

            // odebraná oprávnění označíme jako smazaná
            foreach (RolePermission item in dbRole.RolePermissions)
            {
                if (editedRole.RolePermissions.Any(a => a.PermissionId == item.PermissionId))
                    continue;

                item.Deleted = true;
                item.UpdatedByCustomerId = currentUser.Id;
                item.UpdateDateUtc = DateTime.UtcNow;
            }

            _dbContext.Roles.Add(dbRole);

            int result = await _dbContext.SaveChangesAsync();

            TimeSpan timeEnd = DateTime.Now - startTime;

            if (result == 1)
            {
                tran.Commit();
                ret = dbRole.Id;
                _logger.LogInformation($"{logHeader} Role '{dbRole.Name}', Id: {dbRole.Id} updated succesfully, duration: {timeEnd}");
            }
            else
            {
                tran.Rollback();
                ret = -6;
                _logger.LogWarning($"{logHeader} Role '{dbRole.Name}', Id: {dbRole.Id} was not updated, duration: {timeEnd}");
            }

            if (cloneRole)
            {
                bool cloneSuccess = await CloneRole(dbRole.Id);
                if (!cloneSuccess)
                {
                    tran.Rollback();
                    ret = -7;
                    _logger.LogWarning($"{logHeader} Role '{dbRole.Name}', Id: {dbRole.Id} was not cloned while updating, duration: {timeEnd}");
                }
            }
        }
        catch (Exception ex)
        {
            tran.Rollback();
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError($"{logHeader} Role '{dbRole.Name}', Id: {dbRole.Id} was not updated, Error: {errMessage}");
        }

        return ret;
    }

    /// <summary>
    /// Založí klony role procedurou v DB
    /// </summary>
    /// <param name="Id">Id nové role</param>
    private async Task<bool> CloneRole(int Id)
    {
        bool result = false;
        string logHeader = _logName + ".CloneRole:";

        try
        {
            object[] parameters = new object[2];
            DbParameter parId = new SqlParameter("roleId", SqlDbType.Int);
            parId.IsNullable = true;
            parId.Value = Id;
            parId.Direction = ParameterDirection.Input;

            DbParameter parProviderId = new SqlParameter("providerId", SqlDbType.Int);
            parProviderId.IsNullable = true;
            parProviderId.Value = DBNull.Value;
            parProviderId.Direction = ParameterDirection.Input;

            parameters[0] = parId;
            parameters[1] = parProviderId;

            await _dbContext.Database.ExecuteSqlRawAsync($"dbo.sp_CloneRole @roleId, @providerId", parameters);
            result = true;
        }
        
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogWarning($"{logHeader} Stored procedure dbo.sp_CloneRole has failed. Error: {errMessage}");
        }

        return result;
    }
}
