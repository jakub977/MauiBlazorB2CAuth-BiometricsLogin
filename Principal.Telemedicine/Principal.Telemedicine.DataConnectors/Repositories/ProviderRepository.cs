using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class ProviderRepository : IProviderRepository
{

    private readonly DbContextApi _dbContext;

    public ProviderRepository(DbContextApi dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Provider>> GetProvidersTaskAsyncTask()
    {
        var data = await _dbContext.Providers.OrderBy(p => p.Id).ToListAsync();

        return data;
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

    /// <inheritdoc/>
    public async Task<Provider?> GetProviderByIdTaskAsync(int id)
    {
        var data = await _dbContext.Providers
            .Where(p => p.Id == id).FirstOrDefaultAsync(); //Include(i => i.EffectiveUsers)

        return data;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateProviderTaskAsync(Provider provider)
    {
        bool ret = false;

        _dbContext.Providers.Update(provider);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

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

    public IQueryable<Provider> ListOfAllProviders()
    {

        return _dbContext.Providers.Include(c => c.CreatedByCustomer)
            .Include(c => c.UpdatedByCustomer)
            .Include(c => c.Organization)
            .Include(c => c.Picture).DefaultIfEmpty()
            .Include(c => c.City).DefaultIfEmpty();
    }
}

