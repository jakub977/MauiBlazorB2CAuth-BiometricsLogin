using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Enums;

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
    public async Task<IEnumerable<EffectiveUser>> GetEffectiveUsersTaskAsyncTask()
    {
        var data = await _dbContext.EffectiveUsers.OrderBy(p => p.Id).ToListAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EffectiveUser>> GetEffectiveUsersTaskAsyncTask(int userId)
    {
        var data = await _dbContext.EffectiveUsers.Where(w => w.UserId == userId && !w.Deleted).OrderBy(p => p.Id).ToListAsync();

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
    public async Task<bool> UpdateEffectiveUserTaskAsync(EffectiveUser user)
    {
        bool ret = false;

        _dbContext.EffectiveUsers.Update(user);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> InsertEffectiveUserTaskAsync(EffectiveUser user)
    {
        bool ret = false;

        await _dbContext.EffectiveUsers.AddAsync(user);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<ICollection<EffectiveUser>> GetEffectiveUsersByProviderIdTaskAsync(int providerId)
    {
        var data = await _dbContext.EffectiveUsers
            .Where(p => p.ProviderId == providerId && !p.Deleted).OrderBy(p => p.Id).ToListAsync();

        return data;
    }
    /// <inheritdoc/>
    public IQueryable<EffectiveUser> GetEffectiveUsersByProviderId(int providerId)
    {
        var query =  _dbContext.EffectiveUsers.Include(p => p.Provider).Include(p => p.RoleMembers).Include(p => p.GroupEffectiveMembers)
            .Where(p => p.ProviderId == providerId && !p.Deleted).OrderBy(p => p.Id);

        return query;
    }

    public IQueryable<EffectiveUser> GetAdminUsersByProviderId(int providerId, int role)
    {
        var query = _dbContext.EffectiveUsers.Include(p => p.Provider).Include(p => p.RoleMembers).Include(p => p.GroupEffectiveMembers)
            .Where(p => p.ProviderId == providerId && !p.Deleted && p.RoleMembers.Any(r => r.RoleId == role && !r.Deleted)).OrderBy(p => p.Id);

        return query;
    }

    /// <inheritdoc/>
    public IQueryable<EffectiveUser> GetEffectiveUsersByOrganizationId(int organizationId)
    {
        var query = _dbContext.EffectiveUsers.Include(p => p.Provider).Include(p => p.RoleMembers).Include(p => p.GroupEffectiveMembers)
            .Where(p => p.Provider.OrganizationId == organizationId && !p.Deleted).OrderBy(p => p.Id);

        return query;
    }

    public IQueryable<EffectiveUser> GetAdminUsersByOrganizationId(int organizationId, int role)
    {
        var query = _dbContext.EffectiveUsers.Include(p => p.Provider).Include(p => p.RoleMembers).Include(p => p.GroupEffectiveMembers)
            .Where(p => p.Provider.OrganizationId == organizationId && !p.Deleted && p.Active && p.RoleMembers.Any(r => r.RoleId == role && !r.Deleted)).OrderBy(p => p.Id);

        return query;
    }
}

