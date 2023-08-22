using Principal.Telemedicine.DataConnectors.Models;

namespace Principal.Telemedicine.DataConnectors.Repository;

    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomersTaskAsyncTask();
        Task<Customer?> GetCustomerByIdTaskAsync(int id);
    }

