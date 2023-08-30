﻿using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

    /// <inheritdoc/>
    public class CustomerRepository : ICustomerRepository
    {

        private readonly DbContextApi _dbContext;

        public CustomerRepository(DbContextApi dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Customer>> GetCustomersTaskAsyncTask()
        {
            var listOfCustomers = await _dbContext.Customers.OrderBy(p => p.Id).ToListAsync();

            return listOfCustomers;
        }

        /// <inheritdoc/>
        public async Task<Customer?> GetCustomerByIdTaskAsync(int id)
        {
             var customer = await _dbContext.Customers
                 .Include(p => p.EffectiveUserUsers).DefaultIfEmpty() // efektivního uživatele mají jenom uživatelé, kteřé mají vyplněné ProviderId - do RoleMember vazba přes EffectiveUserId
                 .Include (p => p.RoleMemberDirectUsers).DefaultIfEmpty() // uživatelé bez ProviderId mají vazbu do RoleMember přes DirectUserId
                 .Include( p => p.UserPermissionUsers).DefaultIfEmpty() //DeniedPermissions
                 .Include(p=> p.RoleMemberCreatedByCustomers)
                 .Where(p => p.Id == id).FirstOrDefaultAsync();

             return customer;
        }
}

