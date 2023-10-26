using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;

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
    public async Task<IEnumerable<Customer>> GetCustomersTaskAsyncTask()
    {
        var listOfCustomers = await _dbContext.Customers.OrderBy(p => p.Id).ToListAsync();

        return listOfCustomers;
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
            .Include(p => p.EffectiveUserUsers).ThenInclude(efus => efus.RoleMembers).ThenInclude(efus => efus.Role).DefaultIfEmpty()// efektivního uživatele mají jenom uživatelé, kteřé mají vyplněné ProviderId - do RoleMember vazba přes EffectiveUserId -- pacient, lékař atd.
            .Include(p => p.RoleMemberDirectUsers).ThenInclude(efus => efus.Role).DefaultIfEmpty() // uživatelé bez ProviderId mají vazbu do RoleMember přes DirectUserId -- administrativní role
            .Include(p => p.UserPermissionUsers).DefaultIfEmpty() //DeniedPermissions
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        return customer;
    }

    /// <inheritdoc/>
    public async Task<Customer?> GetCustomerByGlobalIdTaskAsync(string globalId)
    {
        var customer = await _dbContext.Customers
            .Include(p => p.EffectiveUserUsers).ThenInclude(efus => efus.RoleMembers).ThenInclude(efus => efus.Role).DefaultIfEmpty()// efektivního uživatele mají jenom uživatelé, kteřé mají vyplněné ProviderId - do RoleMember vazba přes EffectiveUserId -- pacient, lékař atd.
            .Include(p => p.RoleMemberDirectUsers).ThenInclude(efus => efus.Role).DefaultIfEmpty() // uživatelé bez ProviderId mají vazbu do RoleMember přes DirectUserId -- administrativní role
            .Include(p => p.UserPermissionUsers).DefaultIfEmpty() //DeniedPermissions
            .Where(p => p.GlobalId == globalId).FirstOrDefaultAsync();

        return customer;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateCustomerTaskAsync(Customer currentUser, Customer user, bool? ignoreADB2C = false, IDbContextTransaction? tran = null, bool dontManageTran = false)
    {
        bool ret = false;
        string logHeader = _logName + ".InsertCustomerTaskAsync:";

        if (tran == null && !dontManageTran)
            tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            user.UpdateDateUtc = DateTime.UtcNow;
            user.UpdatedByCustomerId = currentUser.Id;

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
                    return true;
                }
                else
                {
                    if (!dontManageTran)
                        tran.Rollback();
                    _logger.LogWarning("{0} User '{1}', Email: '{2}', Id: {3} was not updated", logHeader, user.FriendlyName, user.Email, user.Id);
                    return false;
                }
            }
            else
            {
                if (result != 0)
                    ret = await _adb2cRepository.UpdateUserAsyncTask(user);

                if (ret)
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
    public async Task<bool> InsertCustomerTaskAsync(Customer currentUser, Customer user)
    {
        bool ret = false;
        string logHeader = _logName + ".InsertCustomerTaskAsync:";
        DateTime startTime = DateTime.Now;

        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            user.CreatedDateUtc = DateTime.UtcNow;
            user.CreatedByCustomerId = currentUser.Id;

            _dbContext.Customers.Update(user);

            int result = await _dbContext.SaveChangesAsync();
            TimeSpan end1 = DateTime.Now - startTime;
            _logger.LogInformation("{0} Saved to DB: {1}", logHeader, end1);
            if (result != 0)
                ret = await _adb2cRepository.InsertUserAsyncTask(user);

            TimeSpan timeEnd = DateTime.Now - startTime;

            if (ret)
            {
                tran.Commit();
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
    public async Task<bool> DeleteCustomerTaskAsync(Customer currentUser, Customer user, bool? ignoreADB2C = false)
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
    public async Task<int> CheckIfUserExists(Customer currentUser, Customer user)
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

