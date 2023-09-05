using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Models;
using System.Collections.ObjectModel;

namespace Principal.Telemedicine.SharedApi.Controllers;

/// <summary>
/// API metody vztažené k uživateli
/// </summary>
[Route("api/[controller]/[action]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger _logger; 
        private readonly IMapper _mapper;

        public UserApiController(ICustomerRepository customerRepository, ILogger<UserApiController> logger, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _mapper = mapper;
        }

    /// <summary>
    /// Vrátí základní údaje uživatele.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetUserInfo")]
        public async Task<IActionResult> GetUserInfo([FromHeader(Name = "x-api-key")] string apiKey, int userId)
        {

            if (userId <= 0)
            {
                return BadRequest();
            }

            try
            {
                var user = await _customerRepository.GetCustomerByIdTaskAsync(userId);
                var mappedUser = _mapper.Map<UserContract>(user);
                
                return Ok(mappedUser);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


    /// <summary>
    /// Vrátí údaje uživatele včetně rolí, efektivních uživatelů a oprávnění.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetUser")]
        public async Task<IActionResult> GetUser(int userId)
        {

            if (userId <= 0)
            {
                return BadRequest();
            }

            try
            {
                var user = await _customerRepository.GetCustomerByIdTaskAsync(userId);
                var mappedUser = _mapper.Map<CompleteUserContract>(user);

                return Ok(mappedUser);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
}

