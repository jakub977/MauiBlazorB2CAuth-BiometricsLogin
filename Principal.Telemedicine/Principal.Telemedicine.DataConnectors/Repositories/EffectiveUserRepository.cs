using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Graph.Models;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Enums;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class EffectiveUserRepository : IEffectiveUserRepository
{

    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly string _logName = "CustomerRepository";

    public EffectiveUserRepository(DbContextApi dbContext, ILogger<EffectiveUserRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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
    public async Task<bool> UpdateEffectiveUserTaskAsync(CompleteUserContract currentUser, EffectiveUser user)
    {
        bool ret = false;

        user.UpdateDateUtc = DateTime.UtcNow;
        user.UpdatedByCustomerId = currentUser.Id;

        bool tracking = _dbContext.ChangeTracker.Entries<EffectiveUser>().Any(x => x.Entity.Id == user.Id);
        if (!tracking)
        {
            _dbContext.EffectiveUsers.Update(user);
        }

        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public bool UpdateEffectiveUser(CompleteUserContract currentUser, EffectiveUser user)
    {
        bool ret = false;

        user.UpdateDateUtc = DateTime.UtcNow;
        user.UpdatedByCustomerId = currentUser.Id;

        bool tracking = _dbContext.ChangeTracker.Entries<EffectiveUser>().Any(x => x.Entity.Id == user.Id);
        if (!tracking)
        {
            _dbContext.EffectiveUsers.Update(user);
        }

        int result = _dbContext.SaveChanges();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> InsertEffectiveUserTaskAsync(CompleteUserContract currentUser, EffectiveUser user)
    {
        bool ret = false;

        user.Deleted = false;
        user.CreatedDateUtc = DateTime.UtcNow;
        user.CreatedByCustomerId = currentUser.Id;

        _dbContext.EffectiveUsers.Add(user);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public bool InsertEffectiveUser(CompleteUserContract currentUser, EffectiveUser user)
    {
        bool ret = false;

        user.Deleted = false;
        user.CreatedDateUtc = DateTime.UtcNow;
        user.CreatedByCustomerId = currentUser.Id;

        _dbContext.EffectiveUsers.Add(user);
        int result = _dbContext.SaveChanges();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteEffectiveUserTaskAsync(CompleteUserContract currentUser, EffectiveUser user)
    {
        bool ret = false;
        string logHeader = _logName + ".DeleteEffectiveUserTaskAsync:";

        try
        {
            user.UpdateDateUtc = DateTime.UtcNow;
            user.UpdatedByCustomerId = currentUser.Id;
            user.Deleted = true;

            bool tracking = _dbContext.ChangeTracker.Entries<EffectiveUser>().Any(x => x.Entity.Id == user.Id);
            if (!tracking)
            {
                _dbContext.EffectiveUsers.Update(user);
            }

            int result = await _dbContext.SaveChangesAsync();

            if (result != 0)
            {
                _logger.LogDebug("{0} EffectiveUser Id: {1} deleted succesfully", logHeader, user.Id);
                ret = true;
            }
            else
            {
                _logger.LogWarning("{0} EffectiveUser Id: {1} was not deleted", logHeader, user.Id);
            }

        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError("{0} EffectiveUser Id: {1} was not deleted, Error: {4}", logHeader, user.Id, errMessage);
        }

        return ret;
    }

    /// <inheritdoc/>
    public IQueryable<EffectiveUser> GetEffectiveUsersByOrganizationId(int organizationId)
    {
        var query = _dbContext.EffectiveUsers.Include(p => p.Provider).Include(p => p.RoleMembers).Include(p => p.GroupEffectiveMembers)
            .Where(p => p.Provider.OrganizationId == organizationId && !p.Deleted).OrderBy(p => p.Id);

        return query;
    }
}

