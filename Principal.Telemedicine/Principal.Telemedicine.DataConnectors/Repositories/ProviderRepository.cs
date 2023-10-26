using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class ProviderRepository : IProviderRepository
{

    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly string _logName = "ProviderRepository";

    public ProviderRepository(DbContextApi dbContext, ILogger<ProviderRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Provider>> GetProvidersTaskAsync()
    {
        var data = await _dbContext.Providers.Include(i => i.EffectiveUsers).Include(i => i.City).OrderBy(p => p.Id).ToListAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<Provider?> GetProviderByIdTaskAsync(int id)
    {
        var data = await _dbContext.Providers.Include(i => i.EffectiveUsers).ThenInclude(t => t.RoleMembers)
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateProviderTaskAsync(Provider provider, IDbContextTransaction? tran = null, bool dontManageTran = false)
    {
        bool ret = false;
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".UpdateProviderTaskAsync:";
        if (tran == null && !dontManageTran)
        {
            tran = await _dbContext.Database.BeginTransactionAsync();
        }

        try
        {
            provider.UpdateDateUtc = DateTime.UtcNow;

            bool tracking = _dbContext.ChangeTracker.Entries<Provider>().Any(x => x.Entity.Id == provider.Id);
            if (!tracking)
            {
                _dbContext.Providers.Update(provider);
            }

            int result = await _dbContext.SaveChangesAsync();
            TimeSpan timeEnd = DateTime.Now - startTime;


            if (result != 0)
                {
                    if (!dontManageTran)
                        tran.Commit();
                    _logger.LogDebug($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} updated succesfully, duration: {timeEnd}");
                    return true;
                }
                else
                {
                    if (!dontManageTran)
                        tran.Rollback();
                    _logger.LogWarning($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} was not updated, duration: {timeEnd}");
                    return false;
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
            _logger.LogError($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} was not updated, Error: {errMessage}");
        }
        if (!dontManageTran)
            tran.Dispose();
        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> InsertProviderTaskAsync(Provider provider)
    {
        bool ret = false;

        _dbContext.Providers.Add(provider);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteProviderTaskAsync(Customer currentUser, Provider provider)
    {
        bool ret = false;
        string logHeader = _logName + ".DeleteProviderTaskAsync:";
        DateTime startTime = DateTime.Now;
        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            provider.UpdateDateUtc = DateTime.UtcNow;
            provider.UpdatedByCustomerId = currentUser.Id;
            provider.Deleted = true;

            bool tracking = _dbContext.ChangeTracker.Entries<Provider>().Any(x => x.Entity.Id == provider.Id);
            if (!tracking)
            {
                _dbContext.Providers.Update(provider);
            }

            int result = await _dbContext.SaveChangesAsync();


            TimeSpan timeEnd = DateTime.Now - startTime;
            if (result != 0)
            {
                tran.Commit();
                _logger.LogDebug($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} deleted succesfully, duration: {timeEnd}");
                ret = true;
            }
            else
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} was not deleted, duration: {timeEnd}");
                ret = false;
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
            _logger.LogError($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} was not deleted, Error: {errMessage}");
        }

        return ret;
    }
    public Provider? GetProviderById(int providerId)
    {
        if (providerId < 1)
            return null;
        return _dbContext.Providers.Include(c => c.CreatedByCustomer)
        .Include(c => c.UpdatedByCustomer)
            .Include(c => c.Organization)
            .Include(c => c.Picture).DefaultIfEmpty()
            .Include(c => c.City).DefaultIfEmpty()
            .FirstOrDefault(d => d.Id == providerId);
    }

    public IQueryable<Provider> ListOfAllProviders()
    {
        return _dbContext.Providers.Include(c => c.CreatedByCustomer)
            .Include(c => c.UpdatedByCustomer)
            .Include(c => c.Organization)
            .Include(c => c.Picture).DefaultIfEmpty()
            .Include(c => c.City).DefaultIfEmpty();
    }
}

