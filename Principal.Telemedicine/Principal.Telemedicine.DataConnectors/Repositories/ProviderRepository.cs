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
}

