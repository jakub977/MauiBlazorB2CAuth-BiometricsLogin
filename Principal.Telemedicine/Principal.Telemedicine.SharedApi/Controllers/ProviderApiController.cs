using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
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
    private readonly ISubjectAllowedToOrganizationRepository _subjectAllowedToOrganizationRepository;
    private readonly IConfiguration _configuration;
    private readonly IADB2CRepository _adb2cRepository;

    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    private readonly string _logName = "ProviderApiController";

    public ProviderApiController(ICustomerRepository customerRepository, IProviderRepository providerRepository, IEffectiveUserRepository effectiveUserRepository, ISubjectAllowedToOrganizationRepository subjectAllowedToOrganizationRepository,
        IConfiguration configuration, IADB2CRepository adb2cRepository, DbContextApi dbContext, ILogger<UserApiController> logger, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _providerRepository = providerRepository;
        _effectiveUserRepository = effectiveUserRepository;
        _subjectAllowedToOrganizationRepository = subjectAllowedToOrganizationRepository;
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

    public async Task<IActionResult> InsertProvider(string globalId, Provider provider, List<Permission> permissions)
    {
        Customer? currentUser = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
        if (currentUser == null)
        {
            _logger.LogWarning("Current User not found");
            return BadRequest();
        }

        provider.CreatedByCustomerId = currentUser.Id;
        provider.CreatedDateUtc = DateTime.UtcNow;

        if (provider.Picture != null && provider.Picture.IsNew)
        {
            provider.Picture.CreatedDateUtc = DateTime.UtcNow;
            provider.Picture.CreatedByCustomerId = currentUser.Id;
            provider.Picture.Active = true;
        }

       // povolené moduly
        foreach (Permission permission in permissions)
        {
            var subjectAllowedToOrganization = await _subjectAllowedToOrganizationRepository.GetSubjectsAllowedToOrganizationsAsyncTask();
            subjectAllowedToOrganization = subjectAllowedToOrganization.Where(x => x.SubjectId == permission.SubjectId && x.OrganizationId == currentUser.OrganizationId).ToList();
            //.Get(x => x.SubjectId == permission.SubjectId
            //          && x.OrganizationId == currentUser.OrganizationId)
            //.FirstOrDefault();

            if (subjectAllowedToOrganization != null)
            {
                SubjectAllowedToProvider subjectAllowedToProvider = new SubjectAllowedToProvider
                {
                    CreatedByCustomerId = currentUser.Id,
                    CreatedDateUtc = DateTime.UtcNow,
                    Active = true,
                    SubjectAllowedToOrganizationId = subjectAllowedToOrganization.Id,
                    ProviderId = provider.Id
                };

                provider.SubjectAllowedToProviders.Add(subjectAllowedToProvider);
            }
        }

        var adminUsers = provider.EffectiveUsers;
        provider.EffectiveUsers = new List<EffectiveUser>();

        // uložení administrátorů
        provider.EffectiveUsers = adminUsers;
        // SetProviderAdminUsers(currentUser, provider);

        await _providerRepository.InsertProviderTaskAsync(provider);
        return Ok();
    }
}

