using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.SharedApi.Controllers;

/// <summary>
/// API metody vztažené k poskytovateli
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class ProviderApiController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IEffectiveUserRepository _effectiveUserRepository;
    private readonly IConfiguration _configuration;
    private readonly IADB2CRepository _adb2cRepository;

    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    private readonly string _logName = "ProviderApiController";

    public ProviderApiController(ICustomerRepository customerRepository, IProviderRepository providerRepository, IEffectiveUserRepository effectiveUserRepository,
        IConfiguration configuration, IADB2CRepository adb2cRepository, DbContextApi dbContext, ILogger<UserApiController> logger, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _providerRepository = providerRepository;
        _effectiveUserRepository = effectiveUserRepository;
        _configuration = configuration;
        _adb2cRepository = adb2cRepository;
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Vrátí poskytovatele
    /// </summary>
    /// <param name="providerId"> Id požadovaného poskytovatele</param>
    /// <returns>Objekt uživatele</returns>
    [HttpGet(Name = "GetProvider")]
    public async Task<IActionResult> GetProvider(int providerId)
    {

        if (providerId <= 0)
        {
            return BadRequest();
        }

        try
        {
            var mappedProvider = new ProviderContract();

            var provider = await _providerRepository.GetProviderByIdTaskAsync(providerId);
            mappedProvider = _mapper.Map<ProviderContract>(provider);

            var effectiveUsers = await _effectiveUserRepository.GetEffectiveUsersByProviderIdTaskAsync(providerId);
            int countOfEU = effectiveUsers.Count();

            mappedProvider.AdminUsers = countOfEU;

            return Ok(mappedProvider);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
