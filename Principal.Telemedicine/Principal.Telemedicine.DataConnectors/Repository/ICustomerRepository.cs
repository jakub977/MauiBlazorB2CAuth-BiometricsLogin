using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Principal.Telemedicine.DataConnectors.Models;
//todo bĚŽNÁ úprava
namespace Principal.Telemedicine.DataConnectors.Repository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomersTaskAsyncTask();
        Task<Customer?> GetCustomerByIdTaskAsync(int id);
    }
}
