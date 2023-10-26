using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class RoleMemberRepository : IRoleMemberRepository
{
    private readonly DbContextApi _dbContext;

    public RoleMemberRepository(DbContextApi dbContext)
    {
        _dbContext = dbContext;
    }
    /// <inheritdoc/>
    public async Task<IEnumerable<RoleMember>> GetRoleMembersAsyncTask()
    {
        var data = await _dbContext.RoleMembers.OrderBy(p => p.Id).ToListAsync();

        return data;
    }

    /// <inheritdoc/>
    public IQueryable<RoleMember> GetAllRoleMembers()
    {
        var data = _dbContext.RoleMembers.Include(c => c.EffectiveUser).Include(c => c.DirectUser).Include(c => c.Role).Where(a => !a.Deleted).OrderBy(p => p.Id);

        return data;
    }

    /// <inheritdoc/>
    public async Task<bool> InsertRoleMemberTaskAsync(RoleMember roleMember)
    {
        bool ret = false;

        _dbContext.RoleMembers.Add(roleMember);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateRoleMemberTaskAsync(RoleMember roleMember)
    {
        bool ret = false;

        _dbContext.RoleMembers.Update(roleMember);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteRoleMemberTaskAsync(RoleMember roleMember)
    {
        bool ret = false;

        _dbContext.RoleMembers.Remove(roleMember);
        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }
}
