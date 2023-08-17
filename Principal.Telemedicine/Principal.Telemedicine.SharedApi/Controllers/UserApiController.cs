using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.DataConnectors.Repository;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.SharedApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<UserApiController> _logger;
        private readonly IMapper _mapper;

        public UserApiController(ICustomerRepository customerRepository, ILogger<UserApiController> logger, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _mapper = mapper;

        }

        /// <summary>
        /// Vrátí základní údaje uživatele
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetUserInfo")]
        public async Task<IActionResult> GetUserInfo([FromHeader(Name = "x-api-key")] string apiKey, int userId)
        {
            try
            {
                var user = await _customerRepository.GetCustomerByIdTaskAsync(userId);
                var mappedUser = _mapper.Map<UserContract>(user);
                
                return Ok(mappedUser);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem();
            }
        }
    }
}
