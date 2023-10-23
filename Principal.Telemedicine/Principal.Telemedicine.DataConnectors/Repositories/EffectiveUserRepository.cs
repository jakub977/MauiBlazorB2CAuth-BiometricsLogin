using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class EffectiveUserRepository : IEffectiveUserRepository
{

    private readonly DbContextApi _dbContext;

    public EffectiveUserRepository(DbContextApi dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EffectiveUser>> GetEffectiveUsersTaskAsync()
    {
        var data = await _dbContext.EffectiveUsers.OrderBy(p => p.Id).ToListAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EffectiveUser>> GetEffectiveUsersTaskAsync(int userId)
    {
        var data = await _dbContext.EffectiveUsers.Where(w => w.UserId == userId && !w.Deleted).OrderBy(p => p.Id).ToListAsync();

        return data;
    }

    public async Task<IEnumerable<EffectiveUser>> GetEffectiveUsersByProviderIdTaskAsync(int providerId)
    {
        var data = await _dbContext.EffectiveUsers.Include(i => i.RoleMembers).Include(i => i.GroupEffectiveMembers).Where(w => w.ProviderId == providerId && !w.Deleted).OrderBy(p => p.Id).ToListAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<EffectiveUser?> GetEffectiveUserByIdTaskAsync(int id)
    {
        var data = await _dbContext.EffectiveUsers
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateEffectiveUserTaskAsync(Customer currentUser, EffectiveUser user)
    {
        bool ret = false;

        user.UpdateDateUtc = DateTime.UtcNow;
        user.UpdatedByCustomerId = currentUser.Id;

        _dbContext.EffectiveUsers.Update(user);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> InsertEffectiveUserTaskAsync(Customer currentUser, EffectiveUser user)
    {
        bool ret = false;

        user.Deleted = false;
        user.CreatedDateUtc = DateTime.UtcNow;
        user.CreatedByCustomerId = currentUser.Id;

        await _dbContext.EffectiveUsers.AddAsync(user);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteEffectiveUserTaskAsync(Customer currentUser, EffectiveUser user)
    {
        bool ret = false;

        user.Deleted = true;
        user.UpdateDateUtc = DateTime.UtcNow;
        user.UpdatedByCustomerId = currentUser.Id;

        _dbContext.EffectiveUsers.Update(user);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }
}

