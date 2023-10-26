using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class ProviderRepository : IProviderRepository
{

    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;

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
    public async Task<bool> UpdateProviderTaskAsync(Provider provider)
    {
        bool ret = false;
        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            provider.UpdateDateUtc = DateTime.UtcNow;

            bool tracking = _dbContext.ChangeTracker.Entries<Provider>().Any(x => x.Entity.Id == provider.Id);
            if (!tracking)
            {
                _dbContext.Providers.Update(provider);
            }

            int result = await _dbContext.SaveChangesAsync();

           
                if (result != 0)
                {
                    tran.Commit();
                    _logger.LogDebug("Provider '{0}', Id: {1} updated succesfully", provider.Name, provider.Id);
                    return true;

                }
                else
                {
                    tran.Rollback();
                    _logger.LogWarning("Provider '{0}', Id: {1}", provider.Name, provider.Id);
                    return false;
                }
            
        }

        catch (Exception ex)
        {
            tran.Rollback();
            _logger.LogError("Provider '{0}', Id: {1} was not updated, Error: {2}", provider.Name, provider.Id, ex.Message);
        }

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

    public Provider GetProviderById(int providerId)
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
}

