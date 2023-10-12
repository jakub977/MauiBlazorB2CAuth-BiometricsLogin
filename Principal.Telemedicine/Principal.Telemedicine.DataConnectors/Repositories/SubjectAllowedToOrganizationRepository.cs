using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
