using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Models;

namespace Principal.Telemedicine.DataConnectors.Repository;

    public class CustomerRepository : ICustomerRepository
    {

        private readonly ApiDbContext _dbContext;

        public CustomerRepository(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Customer>> GetCustomersTaskAsyncTask()
        {
            var listOfCustomers = await _dbContext.Customers.OrderBy(p => p.Id).ToListAsync();

            return listOfCustomers;
        }

        public async Task<Customer?> GetCustomerByIdTaskAsync(int id)
        {
             var customer = await _dbContext.Customers.Where(p => p.Id == id).FirstOrDefaultAsync();

             return customer;
        }
    }

