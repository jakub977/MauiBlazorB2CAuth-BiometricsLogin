using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Extensions;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.DataConnectors.Utils;
using Principal.Telemedicine.Shared.Enums;
using Principal.Telemedicine.Shared.Models;
using System.Data;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class CustomerRepository : ICustomerRepository
{

    private readonly DbContextApi _dbContext;
    private readonly IADB2CRepository _adb2cRepository;
    private readonly ILogger _logger;
    private readonly string _logName = "CustomerRepository";

    public CustomerRepository(DbContextApi dbContext, IADB2CRepository adb2cRepository, ILogger<CustomerRepository> logger)
    {
        _dbContext = dbContext;
        _adb2cRepository = adb2cRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Customer>> GetCustomersTaskAsyncTask(int? providerId = null)
    {
        if (providerId.HasValue)
            return await _dbContext.Customers.Where(w => w.CreatedByProviderId == providerId.Value).OrderBy(p => p.Id).ToListAsync();
        else
            return await _dbContext.Customers.OrderBy(p => p.Id).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Customer?> GetCustomerInfoByIdTaskAsync(int id)
    {
        var customer = await _dbContext.Customers
            .Include(p => p.Picture).DefaultIfEmpty()
            .Include(p => p.Organization).DefaultIfEmpty()
            .Include(p => p.GenderType).DefaultIfEmpty()
            .Include(p => p.City).DefaultIfEmpty()
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        return customer;
    }

    /// <inheritdoc/>
    public IQueryable<Customer> ListOfAllCustomers()
    {
      return _dbContext.Customers.OrderBy(c => c.Id);
    }

    /// <inheritdoc/>
    public async Task<Customer?> GetCustomerByIdTaskAsync(int id)
    {
        var customer = await _dbContext.Customers
            .Include(p => p.EffectiveUserUsers).ThenInclude(efus => efus.RoleMembers.Where(w => !w.Deleted)).ThenInclude(efus => efus.Role).ThenInclude(p => p.RolePermissions).ThenInclude(p => p.Permission).ThenInclude(p => p.Subject).ThenInclude(p => p.ParentSubject).DefaultIfEmpty()// efektivního uživatele mají jenom uživatelé, kteřé mají vyplněné ProviderId - do RoleMember vazba přes EffectiveUserId -- pacient, lékař atd.
            .Include(p => p.EffectiveUserUsers).ThenInclude(efus => efus.RoleMembers.Where(w => !w.Deleted)).ThenInclude(efus => efus.Role).ThenInclude(i => i.RoleCategoryCombination).ThenInclude(i => i.RoleCategory).DefaultIfEmpty()
            .Include(p => p.EffectiveUserUsers).ThenInclude(i => i.GroupEffectiveMembers.Where(w => !w.Deleted)).ThenInclude(i => i.Group).ThenInclude(p => p.GroupPermissions).ThenInclude(p => p.Permission).ThenInclude(p => p.Subject).ThenInclude(p => p.ParentSubject).DefaultIfEmpty()
            .Include(p => p.RoleMemberDirectUsers.Where(w => !w.Deleted)).ThenInclude(efus => efus.Role).ThenInclude(p => p.RolePermissions).ThenInclude(p => p.Permission).ThenInclude(p => p.Subject).ThenInclude(p => p.ParentSubject).DefaultIfEmpty() // uživatelé bez ProviderId mají vazbu do RoleMember přes DirectUserId -- administrativní role
            .Include(p => p.RoleMemberDirectUsers.Where(w => !w.Deleted)).ThenInclude(efus => efus.Role).ThenInclude(i => i.RoleCategoryCombination).ThenInclude(i => i.RoleCategory).DefaultIfEmpty()
            .Include(p => p.UserPermissionUsers).ThenInclude(p => p.Permission).ThenInclude(p => p.Subject).ThenInclude(p => p.ParentSubject).DefaultIfEmpty() //DeniedPermissions
            .Include(p => p.Picture).DefaultIfEmpty()
            .Include(p => p.Organization).DefaultIfEmpty()
            .Include(p => p.GenderType).DefaultIfEmpty()
            .Include(p => p.City).DefaultIfEmpty()
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        return customer;
    }

    /// <inheritdoc/>
    public async Task<Customer?> GetCustomerByGlobalIdTaskAsync(string globalId)
    {
        var customer = await _dbContext.Customers
            .Include(p => p.EffectiveUserUsers.Where(w => !w.Deleted)).ThenInclude(efus => efus.RoleMembers.Where(w => !w.Deleted)).ThenInclude(efus => efus.Role).ThenInclude(p => p.RolePermissions.Where(w => !w.Deleted)).ThenInclude(p => p.Permission).ThenInclude(p => p.Subject).ThenInclude(p => p.ParentSubject).DefaultIfEmpty()// efektivního uživatele mají jenom uživatelé, kteřé mají vyplněné ProviderId - do RoleMember vazba přes EffectiveUserId -- pacient, lékař atd.
            .Include(p => p.EffectiveUserUsers.Where(w => !w.Deleted)).ThenInclude(efus => efus.RoleMembers.Where(w => !w.Deleted)).ThenInclude(efus => efus.Role).ThenInclude(i => i.RoleCategoryCombination).ThenInclude(i => i.RoleCategory).DefaultIfEmpty()
            .Include(p => p.EffectiveUserUsers.Where(w => !w.Deleted)).ThenInclude(i => i.GroupEffectiveMembers.Where(w => !w.Deleted)).ThenInclude(i => i.Group).ThenInclude(p => p.GroupPermissions.Where(w => !w.Deleted)).ThenInclude(p => p.Permission).ThenInclude(p => p.Subject).ThenInclude(p => p.ParentSubject).DefaultIfEmpty()
            .Include(p => p.RoleMemberDirectUsers.Where(w => !w.Deleted)).ThenInclude(efus => efus.Role).ThenInclude(p => p.RolePermissions.Where(w => !w.Deleted)).ThenInclude(p => p.Permission).ThenInclude(p => p.Subject).ThenInclude(p => p.ParentSubject).DefaultIfEmpty() // uživatelé bez ProviderId mají vazbu do RoleMember přes DirectUserId -- administrativní role
            .Include(p => p.RoleMemberDirectUsers.Where(w => !w.Deleted)).ThenInclude(efus => efus.Role).ThenInclude(i => i.RoleCategoryCombination).ThenInclude(i => i.RoleCategory).DefaultIfEmpty()
            .Include(p => p.UserPermissionUsers.Where(w => !w.Deleted)).ThenInclude(p => p.Permission).ThenInclude(p => p.Subject).ThenInclude(p => p.ParentSubject).DefaultIfEmpty() //DeniedPermissions
            .Include(p => p.Organization).DefaultIfEmpty()
            .Include(p => p.GenderType).DefaultIfEmpty()
            .Include(p => p.City).DefaultIfEmpty()
            .Where(p => p.GlobalId == globalId).FirstOrDefaultAsync();

        return customer;
    }

    /// <inheritdoc/>
    public async Task<Customer?> GetCustomerByIdOnlyForUpdateTaskAsync(int id)
    {
        var customer = await _dbContext.Customers
            .Include(p => p.EffectiveUserUsers).ThenInclude(efus => efus.RoleMembers).DefaultIfEmpty()// efektivního uživatele mají jenom uživatelé, kteřé mají vyplněné ProviderId - do RoleMember vazba přes EffectiveUserId -- pacient, lékař atd.
            .Include(p => p.EffectiveUserUsers).ThenInclude(i => i.GroupEffectiveMembers).DefaultIfEmpty()
            .Include(p => p.RoleMemberDirectUsers).DefaultIfEmpty() // uživatelé bez ProviderId mají vazbu do RoleMember přes DirectUserId -- administrativní role
            .Include(p => p.UserPermissionUsers).DefaultIfEmpty() //DeniedPermissions
            .Include(p => p.Picture).DefaultIfEmpty()
            .Include(p => p.Organization).DefaultIfEmpty()
            .Include(p => p.GenderType).DefaultIfEmpty()
            .Include(p => p.City).DefaultIfEmpty()
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        return customer;
    }

    /// <inheritdoc/>
    public async Task<PaginatedListData<Customer>> GetCustomersTaskAsync(CompleteUserContract currentUser, bool activeUsersOnly, int? filterRole, int? filteGroup, string? searchText, string? order = "created_desc", int? page = 1, int? pageSize = 20, int? providerId = null)
    {
        IQueryable<Customer> query = _dbContext.Customers
            .Include(p => p.EffectiveUserUsers).ThenInclude(efus => efus.RoleMembers.Where(w => !w.Deleted)).ThenInclude(efus => efus.Role).DefaultIfEmpty()// efektivního uživatele mají jenom uživatelé, kteřé mají vyplněné ProviderId - do RoleMember vazba přes EffectiveUserId -- pacient, lékař atd.
            .Include(p => p.EffectiveUserUsers).ThenInclude(i => i.GroupEffectiveMembers.Where(w => !w.Deleted)).ThenInclude(i => i.Group).DefaultIfEmpty()
            .Include(p => p.RoleMemberDirectUsers.Where(w => !w.Deleted)).ThenInclude(efus => efus.Role).DefaultIfEmpty() // uživatelé bez ProviderId mají vazbu do RoleMember přes DirectUserId -- administrativní role
            .Include(p => p.Picture).DefaultIfEmpty()
            .Include(p => p.City).DefaultIfEmpty().AsQueryable();


        // filtrování podle role
        if (filterRole != null)
        {
            if (filterRole > 0)
                query = activeUsersOnly
        ? query.Where(w => w.RoleMemberDirectUsers.Any(a => a.RoleId == filterRole && a.Active.Value && !a.Deleted) || (w.EffectiveUserUsers.Count > 0 && w.EffectiveUserUsers.Any(a => !a.Deleted && a.Active.Value && a.RoleMembers.Any(g => g.Active.Value && !g.Deleted && g.RoleId == filterRole))))
        : query.Where(w => w.RoleMemberDirectUsers.Any(a => a.RoleId == filterRole && a.Active.Value && !a.Deleted) || (w.EffectiveUserUsers.Count > 0 && w.EffectiveUserUsers.Any(a => !a.Deleted && a.RoleMembers.Any(g => g.Active.Value && !g.Deleted && g.RoleId == filterRole))));
        }
        // filtrování podle skupiny
        if (filteGroup != null)
        {
            if (filteGroup > 0)
                query = activeUsersOnly
                ? query = query.Where(w => w.EffectiveUserUsers.Count > 0 && w.EffectiveUserUsers.Any(a => !a.Deleted && a.Active.Value && a.GroupEffectiveMembers.Any(g => g.Active.Value && !g.Deleted && g.GroupId == filteGroup)))
                : query = query.Where(w => w.EffectiveUserUsers.Count > 0 && w.EffectiveUserUsers.Any(a => !a.Deleted && a.GroupEffectiveMembers.Any(g => g.Active.Value && !g.Deleted && g.GroupId == filteGroup)));
        }
        // filtrování podle jména a adresy
        if (searchText != null)
        {
            if (!string.IsNullOrEmpty(searchText))
                query = query.Where(w => w.FriendlyName.Contains(searchText) || (w.AddressLine == null ? false : w.AddressLine.Contains(searchText)));
        }

        if (currentUser.IsGlobalAdmin())
        {
            // globální admin vidí i uživatele bez přiřazeného poskytovatele -> Správce organizace a ostatní super adminy
            if (providerId.HasValue)
            {
                query = activeUsersOnly
                    ? query.Where(w => w.Active.Value && !w.Deleted && ((w.EffectiveUserUsers.Count > 0 && w.EffectiveUserUsers.Any(a => a.ProviderId == providerId && a.Active.Value && !a.Deleted)) // efektivní uživatel
                    || w.IsOrganizationAdminAccount || w.IsSuperAdminAccount // správce organizace a super admin podle příznaku
                    || w.RoleMemberDirectUsers.Any(r => r.Active.Value && !r.Deleted && (r.RoleId == (int)RoleMainEnum.SuperAdmin || r.RoleId == (int)RoleMainEnum.OrganizationAdmin || r.RoleId == (int)RoleMainEnum.Research || (r.Role.ParentRoleId.HasValue && r.Role.ParentRole.Active.Value && !r.Role.ParentRole.Deleted && (r.Role.ParentRoleId.Value == (int)RoleMainEnum.SuperAdmin || r.RoleId == (int)RoleMainEnum.OrganizationAdmin || r.RoleId == (int)RoleMainEnum.Research)))))) // správce organizace a super admin a výzkum podle role

                    : query.Where(w => !w.Deleted && (
                    (w.EffectiveUserUsers.Count > 0 && w.EffectiveUserUsers.Any(a => a.ProviderId == providerId && !a.Deleted))  // efektivní uživatel
                    || w.IsOrganizationAdminAccount || w.IsSuperAdminAccount // správce organizace a super admin podle příznaku
                    || (!w.RoleMemberDirectUsers.Any(r => r.Active.Value && !r.Deleted) && !w.EffectiveUserUsers.Any(a => !a.Deleted) && w.CreatedByProviderId.HasValue && w.CreatedByProviderId == providerId) // uživatel bez role
                    || w.RoleMemberDirectUsers.Any(r => r.Active.Value && !r.Deleted && (r.RoleId == (int)RoleMainEnum.SuperAdmin || r.RoleId == (int)RoleMainEnum.OrganizationAdmin || r.RoleId == (int)RoleMainEnum.Research || (r.Role.ParentRoleId.HasValue && r.Role.ParentRole.Active.Value && !r.Role.ParentRole.Deleted && (r.Role.ParentRoleId.Value == (int)RoleMainEnum.SuperAdmin || r.RoleId == (int)RoleMainEnum.OrganizationAdmin || r.RoleId == (int)RoleMainEnum.Research)))))); // správce organizace a super admin a výzkum podle role
            }
            else
            {
                query = activeUsersOnly
                    ? query.Where(w => w.Active.Value && !w.Deleted && (w.IsOrganizationAdminAccount || w.IsSuperAdminAccount // správce organizace a super admin podle příznaku
                    || w.RoleMemberDirectUsers.Any(r => r.Active.Value && !r.Deleted && (r.RoleId == (int)RoleMainEnum.SuperAdmin || r.RoleId == (int)RoleMainEnum.OrganizationAdmin || r.RoleId == (int)RoleMainEnum.Research || (r.Role.ParentRoleId.HasValue && (r.Role.ParentRoleId.Value == (int)RoleMainEnum.SuperAdmin || r.RoleId == (int)RoleMainEnum.OrganizationAdmin || r.RoleId == (int)RoleMainEnum.Research)))))) // správce organizace a super admin a výzkum podle role

                    : query.Where(w => !w.Deleted && (w.IsOrganizationAdminAccount || w.IsSuperAdminAccount // správce organizace a super admin podle příznaku
                    || (!w.RoleMemberDirectUsers.Any(r => r.Active.Value && !r.Deleted) && !w.EffectiveUserUsers.Any(a => !a.Deleted)) // uživatel bez role
                    || w.RoleMemberDirectUsers.Any(r => r.Active.Value && !r.Deleted && (r.RoleId == (int)RoleMainEnum.SuperAdmin || r.RoleId == (int)RoleMainEnum.OrganizationAdmin || r.RoleId == (int)RoleMainEnum.Research || (r.Role.ParentRoleId.HasValue && (r.Role.ParentRoleId.Value == (int)RoleMainEnum.SuperAdmin || r.RoleId == (int)RoleMainEnum.OrganizationAdmin || r.RoleId == (int)RoleMainEnum.Research)))))); // správce organizace a super admin a výzkum podle role
            }
        }
        else if (currentUser.IsOrganizationAdmin())
        {
            // Správce organizace vidí Správce poskytovatel a sebe
            query = activeUsersOnly
                ? query.Where(w => w.Active.Value && !w.Deleted && (w.EffectiveUserUsers.Count > 0 && w.EffectiveUserUsers.Any(a => a.Active.Value && !a.Deleted && a.RoleMembers.Any(r => r.Active.Value && !r.Deleted && (r.RoleId == (int)RoleMainEnum.ProviderAdmin || (r.Role.ParentRoleId.HasValue && r.Role.ParentRoleId.Value == (int)RoleMainEnum.ProviderAdmin && r.Role.ParentRole != null && r.Role.ParentRole.Active.Value && !r.Role.ParentRole.Deleted))))) || (w.Id == currentUser.Id && w.Active.Value))
                : query.Where(w => (!w.Deleted && w.EffectiveUserUsers.Count > 0 && w.EffectiveUserUsers.Any(a => !a.Deleted && a.RoleMembers.Any(r => r.Active.Value && !r.Deleted && (r.RoleId == (int)RoleMainEnum.ProviderAdmin || (r.Role.ParentRoleId.HasValue && r.Role.ParentRoleId.Value == (int)RoleMainEnum.ProviderAdmin && r.Role.ParentRole != null && r.Role.ParentRole.Active.Value && !r.Role.ParentRole.Deleted))))) || w.Id == currentUser.Id);
        }
        else if (currentUser.IsResearch())
        {
            // Výzkum vidí všechny ve stejné roli
            query = activeUsersOnly
                ? query.Where(w => w.Active.Value && !w.Deleted && w.RoleMemberDirectUsers.Any(r => r.Active.Value && !r.Deleted && (r.RoleId == (int)RoleMainEnum.Research && r.Role.Active.Value && !r.Role.Deleted || (r.Role.ParentRoleId.HasValue && r.Role.ParentRoleId.Value == (int)RoleMainEnum.Research && r.Role.ParentRole.Active.Value && !r.Role.ParentRole.Deleted))))
                : query.Where(w => !w.Deleted && w.RoleMemberDirectUsers.Any(r => r.Active.Value && !r.Deleted && (r.RoleId == (int)RoleMainEnum.Research && r.Role.Active.Value && !r.Role.Deleted || (r.Role.ParentRoleId.HasValue && r.Role.ParentRoleId.Value == (int)RoleMainEnum.Research && r.Role.ParentRole.Active.Value && !r.Role.ParentRole.Deleted))));
        }
        else
        {
            // pouze uživatelé kteří mají přiděleného poskytovatele a nejsou to Admini
            if (providerId.HasValue)
            {
                query = activeUsersOnly
                    ? query.Where(w => !w.RoleMemberDirectUsers.Any(a => a.Active.Value && !a.Deleted) && w.EffectiveUserUsers.Count > 0 && w.EffectiveUserUsers.Any(a => a.ProviderId == providerId && a.Active.Value && !a.Deleted))
                    : query.Where(w => (!w.RoleMemberDirectUsers.Any(a => a.Active.Value && !a.Deleted) && w.EffectiveUserUsers.Count > 0 && w.EffectiveUserUsers.Any(a => a.ProviderId == providerId && !a.Deleted))
                    || (!w.RoleMemberDirectUsers.Any(r => r.Active.Value && !r.Deleted) && !w.EffectiveUserUsers.Any(a => !a.Deleted) && w.CreatedByProviderId.HasValue && w.CreatedByProviderId == providerId) // uživatel bez role
                    );
            }
            else
            {
                // může vidět jen sebe
                query = activeUsersOnly
                    ? query.Where(w => w.Active.Value && w.Id == currentUser.Id)
                    : query.Where(w => w.Id == currentUser.Id);
            }
        }

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

        return await PaginatedListData<Customer>.CreateAsync(query, page ?? 1, pageSize ?? 20);
    }

    /// <inheritdoc/>
    public async Task<int> UpdateCustomerTaskAsync(CompleteUserContract currentUser, Customer user, bool? ignoreADB2C = false, IDbContextTransaction? tran = null, bool dontManageTran = false)
    {
        int ret = -1;
        string logHeader = _logName + ".InsertCustomerTaskAsync:";

        if (tran == null && !dontManageTran)
            tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            user.UpdateDateUtc = DateTime.UtcNow;
            user.UpdatedByCustomerId = currentUser.Id;

            // kontrola, pokud stávající uživatel nemá GlobalId ve formátu UPN
            if (!user.GlobalId.ToLower().EndsWith(_adb2cRepository.GetApplicationDomain().ToLower()))
                user.GlobalId = _adb2cRepository.CreateUPN(user.Email);

            bool tracking = _dbContext.ChangeTracker.Entries<Customer>().Any(x => x.Entity.Id == user.Id);
            if (!tracking)
            {
                _dbContext.Customers.Update(user);
            }

            int result = await _dbContext.SaveChangesAsync();

            if (ignoreADB2C == true)
            {
                if (result != 0)
                {
                    if (!dontManageTran)
                        tran.Commit();
                    _logger.LogDebug("{0} User '{1}', Email: '{2}', Id: {3} updated succesfully", logHeader, user.FriendlyName, user.Email, user.Id);
                    return 1;
                }
                else
                {
                    if (!dontManageTran)
                        tran.Rollback();
                    _logger.LogWarning("{0} User '{1}', Email: '{2}', Id: {3} was not updated", logHeader, user.FriendlyName, user.Email, user.Id);
                    return -14;
                }
            }
            else
            {
                if (result != 0)
                    ret = await _adb2cRepository.UpdateUserAsyncTask(user);

                if (ret == 1)
                {
                    if (!dontManageTran)
                        tran.Commit();
                    _logger.LogDebug("{0} User '{1}', Email: '{2}', Id: {3} updated succesfully", logHeader, user.FriendlyName, user.Email, user.Id);
                }
                else
                {
                    if (!dontManageTran)
                        tran.Rollback();
                    _logger.LogWarning("{0} User '{1}', Email: '{2}', Id: {3} was not updated", logHeader, user.FriendlyName, user.Email, user.Id);
                }
            }

        }
        catch (Exception ex)
        {
            if (!dontManageTran)
                tran.Rollback();

            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError("{0} User '{1}', Email: '{2}', Id: {3} was not updated, Error: {4}", logHeader, user.FriendlyName, user.Email, user.Id, errMessage);
        }

        if (!dontManageTran)
            tran.Dispose();

        return ret;
    }

    /// <inheritdoc/>
    public async Task<int> InsertCustomerTaskAsync(CompleteUserContract currentUser, Customer user)
    {
        int ret = -1;
        string logHeader = _logName + ".InsertCustomerTaskAsync:";
        DateTime startTime = DateTime.Now;

        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            user.CreatedDateUtc = DateTime.UtcNow;
            user.CreatedByCustomerId = currentUser.Id;
            user.GlobalId = _adb2cRepository.CreateUPN(user.Email);
            user.AdminComment = user.Email; // sem si uloženíme uživatelské přihlašovací jméno "do zálohy", pokud si uživatel změní email (slouží jen jako informace, přihlašování obsluhuje AD B2C)

            _dbContext.Customers.Update(user);

            int result = await _dbContext.SaveChangesAsync();
            
            if (result != 0)
                ret = await _adb2cRepository.InsertUserAsyncTask(user);
            else
                ret = -6;

            TimeSpan timeEnd = DateTime.Now - startTime;

            if (ret == 1)
            {
                tran.Commit();
                ret = user.Id;
                _logger.LogInformation("{0} User '{1}', Email: '{2}', Id: {3} created succesfully, duration: {4}", logHeader, user.FriendlyName, user.Email, user.Id, timeEnd);
            }
            else
            {
                tran.Rollback();
                _logger.LogWarning("{0} User '{1}', Email: '{2}', Id: {3} was not created, duration: {4}", logHeader, user.FriendlyName, user.Email, user.Id, timeEnd);
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
            _logger.LogError("{0} User '{1}', Email: '{2}', Id: {3} was not created, Error: {4}", logHeader, user.FriendlyName, user.Email, user.Id, errMessage);
        }

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteCustomerTaskAsync(CompleteUserContract currentUser, Customer user, bool? ignoreADB2C = false)
    {
        bool ret = false;
        string logHeader = _logName + ".DeleteCustomerTaskAsync:";
        DateTime startTime = DateTime.Now;
        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            user.UpdateDateUtc = DateTime.UtcNow;
            user.UpdatedByCustomerId = currentUser.Id;
            user.Deleted = true;

            bool tracking = _dbContext.ChangeTracker.Entries<Customer>().Any(x => x.Entity.Id == user.Id);
            if (!tracking)
            {
                _dbContext.Customers.Update(user);
            }

            int result = await _dbContext.SaveChangesAsync();

            if (ignoreADB2C == true)
            {
                TimeSpan timeEnd = DateTime.Now - startTime;
                if (result != 0)
                {
                    tran.Commit();
                    _logger.LogDebug("{0} User '{1}', Email: '{2}', Id: {3} deleted succesfully, duration: {4}", logHeader, user.FriendlyName, user.Email, user.Id, timeEnd);
                    ret = true;
                }
                else
                {
                    tran.Rollback();
                    _logger.LogWarning("{0} User '{1}', Email: '{2}', Id: {3} was not deleted, duration: {4}", logHeader, user.FriendlyName, user.Email, user.Id, timeEnd);
                }
            }
            else
            {

                if (result != 0)
                    ret = await _adb2cRepository.DeleteUserAsyncTask(user);

                TimeSpan timeEnd = DateTime.Now - startTime;
                if (ret)
                {
                    tran.Commit();
                    _logger.LogDebug("{0} User '{1}', Email: '{2}', Id: {3} deleted succesfully from ADB2C, duration: {4}", logHeader, user.FriendlyName, user.Email, user.Id, timeEnd);
                }
                else
                {
                    tran.Rollback();
                    _logger.LogWarning("{0} User '{1}', Email: '{2}', Id: {3} was not deleted from ADB2C, duration: {4}", logHeader, user.FriendlyName, user.Email, user.Id, timeEnd);
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
            _logger.LogError("{0} User '{1}', Email: '{2}', Id: {3} was not deleted, Error: {4}", logHeader, user.FriendlyName, user.Email, user.Id, errMessage);
        }

        return ret;
    }

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
    public async Task<int> CheckIfUserExists(CompleteUserContract currentUser, Customer user)
    {
        string logHeader = _logName + ".CheckIfUserExists:";
        string logData = string.Format("Customer: ({0}) {1}, Email: {2}, CurrentUser: ({3}) {4}", user.Id, user.FriendlyName, user.Email, currentUser.Id, currentUser.FriendlyName);

        // KONTROLY
        // seznam exitujících uživatelů, kteří mají stejné klíčové hodnoty
        List<Customer> checkedUsers = await _dbContext.Customers.Where(w => w.Id != user.Id && (
        w.Email == user.Email || w.TelephoneNumber == user.TelephoneNumber || w.TelephoneNumber2 == user.TelephoneNumber || (!string.IsNullOrEmpty(user.GlobalId) && user.GlobalId == w.GlobalId)
        || (!string.IsNullOrEmpty(user.TelephoneNumber2) && user.TelephoneNumber2 == w.TelephoneNumber))
        ).ToListAsync();

        foreach (Customer item in checkedUsers)
        {
            if (!item.Deleted)
            {
                if (item.Email == user.Email)
                {
                    _logger.LogWarning("{0} Customer with same Email exists -> existing Customer: ({2}) {3}, {1}", logHeader, logData, item.Id, item.FriendlyName);
                    return -10;
                }

                if (item.TelephoneNumber == user.TelephoneNumber)
                {
                    _logger.LogWarning("{0} Customer with same TelephoneNumber {4} exists -> existing Customer: ({2}) {3}, {1}", logHeader, logData, item.Id, item.FriendlyName, item.TelephoneNumber);
                    return -11;
                }

                if (item.TelephoneNumber2 == user.TelephoneNumber)
                {
                    _logger.LogWarning("{0} Customer with same TelephoneNumber (in TelephoneNumber2) {4} exists -> existing Customer: ({2}) {3}, {1}", logHeader, logData, item.Id, item.FriendlyName, item.TelephoneNumber);
                    return -11;
                }

                if (!string.IsNullOrEmpty(user.TelephoneNumber2) && item.TelephoneNumber == user.TelephoneNumber2)
                {
                    _logger.LogWarning("{0} Customer with same TelephoneNumber2 (in TelephoneNumber) {4} exists -> existing Customer: ({2}) {3}, {1}", logHeader, logData, item.Id, item.FriendlyName, item.TelephoneNumber2);
                    return -11;
                }

                if (item.PersonalIdentificationNumber == user.PersonalIdentificationNumber)
                {
                    _logger.LogWarning("{0} Customer with same PersonalIdentificationNumber exists -> existing Customer: ({2}) {3}, {1}", logHeader, logData, item.Id, item.FriendlyName);
                    return -12;
                }

                if (!string.IsNullOrEmpty(user.GlobalId) && user.GlobalId == item.GlobalId)
                {
                    _logger.LogWarning("{0} Customer with same GlobalId {4} exists -> existing Customer: ({2}) {3}, {1}", logHeader, logData, item.Id, item.FriendlyName, item.GlobalId);
                    return -13;
                }
            }
        }

        return 0;
    }
}

