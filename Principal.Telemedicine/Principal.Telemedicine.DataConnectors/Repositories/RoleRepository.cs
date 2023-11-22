using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.DataConnectors.Repositories;


public class RoleRepository : IRoleRepository
{
    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly string _logName = "RoleRepository";

    public RoleRepository(DbContextApi dbContext, ILogger<RoleRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IQueryable<Role> ListOfAllRoles()
    {
        return _dbContext.Roles.Include(c => c.CreatedByCustomer)
            .Include(c => c.UpdatedByCustomer)
            .Include(c => c.Organization)
            .Include(c => c.Provider)
            .Include(c => c.RoleCategoryCombination)
            .Include(c => c.RoleMembers).ThenInclude(m => m.EffectiveUser).ThenInclude(e => e.User)
            .DefaultIfEmpty();
    }
}
