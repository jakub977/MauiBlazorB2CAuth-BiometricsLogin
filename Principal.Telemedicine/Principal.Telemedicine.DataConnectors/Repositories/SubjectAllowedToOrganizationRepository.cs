using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class SubjectAllowedToOrganizationRepository : ISubjectAllowedToOrganizationRepository
{
    private readonly DbContextApi _dbContext;

    public SubjectAllowedToOrganizationRepository(DbContextApi dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<SubjectAllowedToOrganization>> GetSubjectsAllowedToOrganizationsAsyncTask()
    {
        var data = await _dbContext.SubjectAllowedToOrganizations.OrderBy(p => p.Id).ToListAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<SubjectAllowedToOrganization>> GetSubjectsAllowedToOrganizationsByOrganizationIdAsyncTask(int organizationId)
    {
        var data = await _dbContext.SubjectAllowedToOrganizations.Where(w => w.OrganizationId == organizationId).OrderBy(p => p.Id).ToListAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<SubjectAllowedToOrganization?> GetSubjectAllowedToOrganizationsBySubjectAndOrganizationIdAsyncTask(int subjectId, int organizationId)
    {
        var data = await _dbContext.SubjectAllowedToOrganizations.Where(p => p.SubjectId == subjectId
                                  && p.OrganizationId == organizationId).FirstOrDefaultAsync();

        return data;
    }
}
