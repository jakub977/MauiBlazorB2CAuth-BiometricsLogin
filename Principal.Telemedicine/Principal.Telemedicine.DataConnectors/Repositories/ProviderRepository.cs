using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;

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

    /// <inheritdoc/>
    public async Task<Provider?> GetProviderByIdTaskAsync(int id)
    {
        var data = await _dbContext.Providers
            .Where(p => p.Id == id).FirstOrDefaultAsync();

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
}

