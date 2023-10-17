using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Enums;
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
    private readonly IRoleMemberRepository _roleMemberRepository;
    private readonly IConfiguration _configuration;
    private readonly IADB2CRepository _adb2cRepository;

    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    private readonly string _logName = "ProviderApiController";

    public ProviderApiController(ICustomerRepository customerRepository, IProviderRepository providerRepository, IEffectiveUserRepository effectiveUserRepository, ISubjectAllowedToOrganizationRepository subjectAllowedToOrganizationRepository,
        IRoleMemberRepository roleMemberRepository, IConfiguration configuration, IADB2CRepository adb2cRepository, DbContextApi dbContext, ILogger<UserApiController> logger, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _providerRepository = providerRepository;
        _effectiveUserRepository = effectiveUserRepository;
        _subjectAllowedToOrganizationRepository = subjectAllowedToOrganizationRepository;
        _roleMemberRepository = roleMemberRepository;
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


    /// <summary>
    /// Uloží poskytovatele
    /// </summary>
    /// <param name="globalId"> GUID uživatele</param>
    /// <param name="provider"> objekt Providera</param>
    /// <param name="permissions"> seznam objektů Permission</param>
    /// <returns></returns>
    [HttpPost(Name = "InsertProvider")]
    public async Task<IActionResult> InsertProvider(string globalId, [FromBody] Provider provider, [FromRoute] List<DataConnectors.Models.Shared.Permission> permissions)
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
        foreach (DataConnectors.Models.Shared.Permission permission in permissions)
        {
            int organizationId = currentUser.OrganizationId == null ? default(int) : currentUser.OrganizationId.Value;
            var subjectAllowedToOrganization = await _subjectAllowedToOrganizationRepository.GetSubjectAllowedToOrganizationsBySubjectAndOrganizationIdAsyncTask(permission.SubjectId, organizationId);

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
        SetProviderAdminUsers(currentUser.Id, provider);

        await _providerRepository.InsertProviderTaskAsync(provider);
        return Ok();
    }

    private void SetProviderAdminUsers(int userId, Provider provider)
    {
        var providerAdminsOfProvider = _effectiveUserRepository.GetEffectiveUsersByProviderId(provider.Id);
        //.ToList()
        providerAdminsOfProvider = providerAdminsOfProvider.Where(u => u.RoleMembers.Any(r => r.RoleId == (int)RoleEnum.ProviderAdmin && !r.Deleted));

        // všichni SP za danou organizaci
        var providerAdminsOfOrganization = _effectiveUserRepository.GetEffectiveUsersByOrganizationId(provider.OrganizationId);
        providerAdminsOfOrganization = providerAdminsOfOrganization.Where(u => u.RoleMembers.Any(r => r.RoleId == (int)RoleEnum.ProviderAdmin && !r.Deleted && r.Active));

        foreach (var effectiveAdminUser in provider.EffectiveUsers)
        {
            // EF uživatel stejného poskytovatele
            var existingEffectiveAdminUser = providerAdminsOfProvider
                .FirstOrDefault(u => u.UserId == effectiveAdminUser.UserId);

            if (existingEffectiveAdminUser == null)
            {
                // EF uživatel jiného poskytovatele, ale v roli SP
                existingEffectiveAdminUser = providerAdminsOfOrganization
                .FirstOrDefault(u => u.UserId == effectiveAdminUser.UserId);

                if (existingEffectiveAdminUser != null)
                {
                    // zkusíme, jestli máme EF uživatele pro daného Poskytovatele
                    EffectiveUser existingEffectiveProviderUser = _effectiveUserRepository.GetEffectiveUsersByProviderId( provider.Id).Where(w => w.UserId == effectiveAdminUser.UserId).FirstOrDefault();

                    if (existingEffectiveProviderUser == null)
                    {
                        // máme EF uživatele jiného poskytovatele, musíme si vytvořit svého EF uživatele
                        existingEffectiveProviderUser = new EffectiveUser()
                        {
                            Active = true,
                            CreatedByCustomerId = userId,
                            CreatedDateUtc = DateTime.UtcNow,
                            Deleted = false,
                            UserId = existingEffectiveAdminUser.UserId,
                            ProviderId = provider.Id,
                        };
                        // přidání EF
                        _effectiveUserRepository.InsertEffectiveUserTaskAsync(existingEffectiveProviderUser);
                    }
                    else
                    {
                        // EF uživatel je neaktivní
                        if (!existingEffectiveProviderUser.Active)
                        {
                            existingEffectiveProviderUser.Active = true;
                            existingEffectiveProviderUser.UpdatedByCustomerId = userId;
                            existingEffectiveProviderUser.UpdateDateUtc = DateTime.UtcNow;
                            _effectiveUserRepository.UpdateEffectiveUserTaskAsync(existingEffectiveProviderUser);
                        }
                    }

                    // přidání role Správce poskytovatele
                    _roleMemberRepository.InsertRoleMemberTaskAsync(new RoleMember
                    {
                        Active = true,
                        CreatedByCustomerId = userId,
                        CreatedDateUtc = DateTime.UtcNow,
                        Deleted = false,
                        EffectiveUserId = existingEffectiveProviderUser.Id,
                        RoleId = (int)RoleEnum.ProviderAdmin
                    });
                }
            }
            // aktualizace administrátora
            else
            {
                var roleMember = _roleMemberRepository.GetAllRoleMembers()
                    .FirstOrDefault(rm => rm.EffectiveUserId == effectiveAdminUser.Id
                                 && rm.RoleId == (int)RoleEnum.ProviderAdmin);

                if (roleMember != null)
                {
                    roleMember.UpdateDateUtc = DateTime.UtcNow;
                    roleMember.UpdatedByCustomerId = userId;
                }
            }
        }

        // promazání odebraných administrátorů
        var effectiveAdminUsersToDelete = providerAdminsOfProvider
            .Where(u => provider.EffectiveUsers.All(x => x.UserId != u.UserId) && !u.Deleted && u.Id > 0)
            .ToList();

        // projdeme existující nesmazané a smažeme je
        foreach (var effectiveAdminUserToDelete in effectiveAdminUsersToDelete)
        {
            var roleMembers = _roleMemberRepository.GetAllRoleMembers()
                .Where(rm => rm.EffectiveUserId == effectiveAdminUserToDelete.Id);

            // seznam rolí SP
            var adminRoleMembers = roleMembers.Where(rm => rm.RoleId == (int)RoleEnum.ProviderAdmin);

            // odebereme roli SP
            foreach (var roleMember in adminRoleMembers)
            {
                _roleMemberRepository.DeleteRoleMemberTaskAsync(roleMember);
            }

            // nemá jiné role u tohoto poskytovatele než roli SP, znaktivníme ho
            if (!roleMembers.Any(a => a.RoleId != (int)RoleEnum.ProviderAdmin))
            {
                effectiveAdminUserToDelete.Active = false;
                effectiveAdminUserToDelete.UpdatedByCustomerId = userId;
                effectiveAdminUserToDelete.UpdateDateUtc = DateTime.UtcNow;
                _effectiveUserRepository.UpdateEffectiveUserTaskAsync(effectiveAdminUserToDelete);
            }
        }
    }
}

