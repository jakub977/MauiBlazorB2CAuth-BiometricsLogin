using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Enums;
using Principal.Telemedicine.Shared.Models;

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
    public async Task<IEnumerable<Provider>> GetProvidersTaskAsync(bool fullData = true, int? organizationId = null)
    {

        if (fullData)
            if (organizationId.HasValue && organizationId.Value > 0)
                return await _dbContext.Providers.Where(w => !w.Deleted && organizationId == organizationId.Value).Include(i => i.EffectiveUsers.Where(w => !w.Deleted)).ThenInclude(t => t.RoleMembers).ThenInclude(t => t.Role).Include(i => i.City).Include(i => i.Picture).Include(t => t.Organization).Include(i => i.EffectiveUsers.Where(w => !w.Deleted)).ThenInclude(t => t.User).OrderBy(p => p.Id).ToListAsync();
            else
                return await _dbContext.Providers.Where(w => !w.Deleted).Include(i => i.EffectiveUsers.Where(w => !w.Deleted)).ThenInclude(t => t.RoleMembers).ThenInclude(t => t.Role).Include(i => i.City).Include(i => i.Picture).Include(t => t.Organization).Include(i => i.EffectiveUsers.Where(w => !w.Deleted)).ThenInclude(t => t.User).OrderBy(p => p.Id).ToListAsync();
        else
            if (organizationId.HasValue && organizationId.Value > 0)
                return await _dbContext.Providers.Where(w => !w.Deleted && organizationId == organizationId.Value).Include(i => i.City).Include(t => t.Organization).OrderBy(p => p.Id).ToListAsync();
            else
                return await _dbContext.Providers.Where(w => !w.Deleted).Include(i => i.City).Include(t => t.Organization).OrderBy(p => p.Id).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Provider?> GetProviderByIdTaskAsync(int id)
    {
        var data = await _dbContext.Providers.Include(i => i.EffectiveUsers.Where(w => !w.Deleted)).ThenInclude(t => t.RoleMembers).ThenInclude(t => t.Role)
            .Include(i => i.EffectiveUsers.Where(w => !w.Deleted)).ThenInclude(t => t.User).ThenInclude(t => t.Picture)
            .Include(t => t.City)
            .Include(t => t.Picture)
            .Include(t => t.Organization)
            .Include(t => t.SubjectAllowedToProviders.Where(w => !w.Deleted)).ThenInclude(t => t.SubjectAllowedToOrganization).ThenInclude(t => t.Subject).ThenInclude(t => t.ParentSubject)
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<Provider?> GetProviderListDetailByIdTaskAsync(int id)
    {
        var data = await _dbContext.Providers
            .Include(i => i.EffectiveUsers.Where(w => !w.Deleted && w.RoleMembers.Any(a => a.RoleId == (int)RoleEnum.ProviderAdmin))).ThenInclude(t => t.RoleMembers)
            .Include(i => i.EffectiveUsers.Where(w => !w.Deleted && w.RoleMembers.Any(a => a.RoleId == (int)RoleEnum.ProviderAdmin))).ThenInclude(t => t.User)
            .Include(t => t.City)
            .Include(t => t.Picture)
            .Include(t => t.Organization)
            .Include(t => t.SubjectAllowedToProviders.Where(w => !w.Deleted)).ThenInclude(t => t.SubjectAllowedToOrganization).ThenInclude(t => t.Subject).ThenInclude(t => t.ParentSubject)
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateProviderTaskAsync(CompleteUserContract currentUser, Provider provider)
    {
        bool ret = false;
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".UpdateProviderTaskAsync:";

        try
        {
            provider.UpdateDateUtc = DateTime.UtcNow;
            provider.UpdatedByCustomerId = currentUser.Id;

            bool tracking = _dbContext.ChangeTracker.Entries<Provider>().Any(x => x.Entity.Id == provider.Id);
            if (!tracking)
            {
                _dbContext.Providers.Update(provider);
            }

            int result = await _dbContext.SaveChangesAsync();
            TimeSpan timeEnd = DateTime.Now - startTime;


            if (result != 0)
            {
                _logger.LogDebug($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} updated succesfully, duration: {timeEnd}");
                return true;
            }
            else
            {
                _logger.LogWarning($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} was not updated, duration: {timeEnd}");
                return false;
            }
        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} was not updated, Error: {errMessage}");
        }

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> InsertProviderTaskAsync(CompleteUserContract currentUser, Provider provider)
    {
        bool ret = false;

        provider.CreatedByCustomerId = currentUser.Id;

        _dbContext.Providers.Add(provider);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteProviderTaskAsync(CompleteUserContract currentUser, Provider provider)
    {
        bool ret = false;
        string logHeader = _logName + ".DeleteProviderTaskAsync:";
        DateTime startTime = DateTime.Now;

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
                _logger.LogDebug($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} deleted succesfully, duration: {timeEnd}");
                ret = true;
            }
            else
            {
                _logger.LogWarning($"{logHeader} Provider '{provider.Name}', Id: {provider.Id} was not deleted, duration: {timeEnd}");
                ret = false;
            }
        }
        catch (Exception ex)
        {
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
            .Include(c => c.SubjectAllowedToProviders).DefaultIfEmpty()
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

