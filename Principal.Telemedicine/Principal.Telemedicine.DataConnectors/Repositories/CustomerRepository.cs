﻿using Microsoft.EntityFrameworkCore;
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
    private readonly string _logName = "ADB2CRepository";

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
    public async Task<Customer?> GetCustomerByIdTaskAsync(int id)
    {
        var customer = await _dbContext.Customers
            .Include(p => p.EffectiveUserUsers).ThenInclude(efus => efus.RoleMembers).DefaultIfEmpty()// efektivního uživatele mají jenom uživatelé, kteřé mají vyplněné ProviderId - do RoleMember vazba přes EffectiveUserId -- pacient, lékař atd.
            .Include(p => p.RoleMemberDirectUsers).DefaultIfEmpty() // uživatelé bez ProviderId mají vazbu do RoleMember přes DirectUserId -- administrativní role
            .Include(p => p.UserPermissionUsers).DefaultIfEmpty() //DeniedPermissions
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        return customer;
    }

    /// <inheritdoc/>
    public async Task<Customer?> GetCustomerByGlobalIdTaskAsync(string globalId)
    {
        var customer = await _dbContext.Customers
            .Include(p => p.EffectiveUserUsers).ThenInclude(efus => efus.RoleMembers).DefaultIfEmpty()// efektivního uživatele mají jenom uživatelé, kteřé mají vyplněné ProviderId - do RoleMember vazba přes EffectiveUserId -- pacient, lékař atd.
            .Include(p => p.RoleMemberDirectUsers).DefaultIfEmpty() // uživatelé bez ProviderId mají vazbu do RoleMember přes DirectUserId -- administrativní role
            .Include(p => p.UserPermissionUsers).DefaultIfEmpty() //DeniedPermissions
            .Where(p => p.GlobalId == globalId).FirstOrDefaultAsync();

        return customer;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateCustomerTaskAsync(Customer user, bool? ignoreADB2C = false)
    {
        bool ret = false;
        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            user.UpdateDateUtc = DateTime.UtcNow;

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
                    tran.Commit();
                    _logger.LogDebug("User '{0}', Email: '{1}', Id: {2} updated succesfully", user.FriendlyName, user.Email, user.Id);
                    return true;

                }
                else
                {
                    tran.Rollback();
                    _logger.LogWarning("User '{0}', Email: '{1}', Id: {2} was not updated", user.FriendlyName, user.Email, user.Id);
                    return false;
                }

            }
            else
            {
                if (result != 0)
                    ret = await _adb2cRepository.UpdateUserAsyncTask(user);

                if (ret)
                {
                    tran.Commit();
                    _logger.LogDebug("User '{0}', Email: '{1}', Id: {2} updated succesfully", user.FriendlyName, user.Email, user.Id);
                }
                else
                {
                    tran.Rollback();
                    _logger.LogWarning("User '{0}', Email: '{1}', Id: {2} was not updated", user.FriendlyName, user.Email, user.Id);
                }
            }
        }
        catch (Exception ex)
        {
            tran.Rollback();
            _logger.LogError("User '{0}', Email: '{1}', Id: {2} was not updated, Error: {3}", user.FriendlyName, user.Email, user.Id, ex.Message);
        }

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> InsertCustomerTaskAsync(Customer user)
    {
        bool ret = false;
        string logHeader = _logName + ".InsertCustomerTaskAsync:";
        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            user.CreatedDateUtc = DateTime.UtcNow;

            _dbContext.Customers.Update(user);

            int result = await _dbContext.SaveChangesAsync();
            if (result != 0)
                ret = await _adb2cRepository.InsertUserAsyncTask(user);

            if (ret)
            {
                tran.Commit();
                _logger.LogDebug("{0} User '{1}', Email: '{2}', Id: {3} created succesfully", logHeader, user.FriendlyName, user.Email, user.Id);
            }
            else
            {
                tran.Rollback();
                _logger.LogWarning("{0} User '{1}', Email: '{2}', Id: {3} was not created", logHeader, user.FriendlyName, user.Email, user.Id);
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

}

