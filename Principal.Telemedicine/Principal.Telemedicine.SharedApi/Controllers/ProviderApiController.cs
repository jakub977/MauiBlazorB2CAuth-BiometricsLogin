using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using Newtonsoft.Json.Linq;
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

            var effectiveUsers = await _effectiveUserRepository.GetEffectiveUsersByProviderIdTaskAsync(providerId);
            provider.EffectiveUsers = effectiveUsers;

            mappedProvider = _mapper.Map<ProviderContract>(provider);

            // mappedProvider.AdminUsers = countOfEU;

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
    [HttpPost(Name = "InsertProvider")]             ///[FromBody]JObject data
    public async Task<IActionResult> InsertProvider(string globalId, [FromBody] ProviderContract? providerContract)
    {
        var mappedProvider = new Provider();
        mappedProvider = _mapper.Map<Provider>(providerContract);

        Customer? currentUser = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);//providerRequest.globalId
        if (currentUser == null)
        {
            _logger.LogWarning("Current User not found");
            return BadRequest();
        }

        mappedProvider.CreatedByCustomerId = currentUser.Id;
        mappedProvider.CreatedDateUtc = DateTime.UtcNow;

        if (mappedProvider.Picture != null && mappedProvider.Picture.IsNew)
        {
            mappedProvider.Picture.CreatedDateUtc = DateTime.UtcNow;
            mappedProvider.Picture.CreatedByCustomerId = currentUser.Id;
            mappedProvider.Picture.Active = true;
        }

        //povolené moduly
        foreach (var permission in providerContract.Permission)
        {
            int organizationId = currentUser.OrganizationId == null ? default(int) : currentUser.OrganizationId.Value;
            var subjectAllowedToOrganization = await _subjectAllowedToOrganizationRepository.GetSubjectAllowedToOrganizationsBySubjectAndOrganizationIdAsyncTask(permission.SubjectId, organizationId);

            if (subjectAllowedToOrganization != null)
            {
                SubjectAllowedToProviderContract subjectAllowedToProvider = new SubjectAllowedToProviderContract
                {
                    CreatedByCustomerId = currentUser.Id,
                    CreatedDateUtc = DateTime.UtcNow,
                    Active = true,
                    SubjectAllowedToOrganizationId = subjectAllowedToOrganization.Id,
                    ProviderId = mappedProvider.Id,
                };

                var mappedSubjects = new SubjectAllowedToProvider();
                mappedSubjects = _mapper.Map<SubjectAllowedToProvider>(subjectAllowedToProvider);
                mappedProvider.SubjectAllowedToProviders.Add(mappedSubjects);
            }
        }

        await _providerRepository.InsertProviderTaskAsync(mappedProvider);
        return Ok();
    }

    /// <summary>
    /// Updatuje poskytovatele
    /// </summary>
    /// <param name="globalId"> GUID uživatele</param>
    /// <param name="provider"> objekt Providera</param>
    /// <param name="permissions"> seznam objektů Permission</param>
    /// <returns></returns>
    [HttpPost(Name = "UpdateProvider")]            
    public async Task<IActionResult> UpdateProvider(string globalId, [FromBody] ProviderContract? providerContract)
    {
        var mappedProvider = new Provider();
        mappedProvider = _mapper.Map<Provider>(providerContract);

        Customer? currentUser = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
        if (currentUser == null)
        {
            _logger.LogWarning("Current User not found");
            return BadRequest();
        }

        var dbProvider = _providerRepository.GetProviderById(mappedProvider.Id);
        if (dbProvider == null)
        {
            _logger.LogWarning("Provider not found");
            return BadRequest();;
        }

        if (_providerRepository.ListOfAllProviders().Any(i => i.Id == mappedProvider.Id))
        { 
                dbProvider.Active = mappedProvider.Active;
                dbProvider.Deleted = false;
                dbProvider.Name = mappedProvider.Name;
                dbProvider.AddressLine = mappedProvider.AddressLine;
                dbProvider.PostalCode = mappedProvider.PostalCode;
                dbProvider.CityId = mappedProvider.CityId;
                dbProvider.IdentificationNumber = mappedProvider.IdentificationNumber;
                dbProvider.TaxIdentificationNumber = mappedProvider.TaxIdentificationNumber;
                dbProvider.UpdatedByCustomerId = currentUser.Id;
                dbProvider.UpdateDateUtc = DateTime.UtcNow;

                if (mappedProvider.Picture != null && mappedProvider.Picture.MediaStorage != null)
                {
                    // aktualizace obsahu
                    if (dbProvider.Picture != null && dbProvider.Picture.Id > 0 && dbProvider.Picture.Id == mappedProvider.Picture.Id)
                    {
                        if (mappedProvider.Picture.IsNew)
                        {
                            dbProvider.Picture.MediaStorage.Data = mappedProvider.Picture.MediaStorage.Data;
                            dbProvider.Picture.UpdatedByCustomerId = currentUser.Id;
                            dbProvider.Picture.UpdatedOnUtc = DateTime.UtcNow;
                            dbProvider.Picture.Active = true;
                        }
                    }
                    else
                    {
                        dbProvider.Picture = mappedProvider.Picture;
                        dbProvider.Picture.CreatedByCustomerId = currentUser.Id;
                        dbProvider.Picture.CreatedDateUtc = DateTime.UtcNow;
                        dbProvider.Picture.Active = true;
                    }
                }
                else
                    dbProvider.Picture = null;


                List<int> existingSubjectIds = new List<int>();
                List<int> selectedSubjectIds = providerContract.Permission.GroupBy(x => x.SubjectId)
                    .Select(grp => grp.Key).ToList();

                foreach (var selectedSubjectId in selectedSubjectIds)
                {
                    int organizationId = currentUser.OrganizationId == null ? default(int) : currentUser.OrganizationId.Value;

                    var subjectAllowedToOrganization = await _subjectAllowedToOrganizationRepository.GetSubjectAllowedToOrganizationsBySubjectAndOrganizationIdAsyncTask(selectedSubjectId, organizationId);
                    
                    // TODO dočasný kód - založení subjektů pro organizaci
                    if (subjectAllowedToOrganization == null)
                    {
                        _logger.LogWarning("SubjectAllowedToOrganization not found");
                        return BadRequest(); 
                    }

                    // kontrola na existenci přiřazeného modulu/subjektu
                    var dbSubjectAllowedToProvider = dbProvider.SubjectAllowedToProviders
                        .FirstOrDefault(w => w.SubjectAllowedToOrganizationId == subjectAllowedToOrganization.Id
                                             && w.ProviderId == mappedProvider.Id);
                    // modul existuje, aktualizuji si Id pro pozdější mazání a pokračuji dál
                    if (dbSubjectAllowedToProvider != null)
                    {
                        existingSubjectIds.Add(dbSubjectAllowedToProvider.Id);
                        // dříve smazané právo, obnovuji ho
                        if (dbSubjectAllowedToProvider.Deleted)
                        {
                            dbSubjectAllowedToProvider.Deleted = false;
                            dbSubjectAllowedToProvider.Active = true;
                            dbSubjectAllowedToProvider.UpdateDateUtc = DateTime.UtcNow;
                            dbSubjectAllowedToProvider.UpdatedByCustomerId = currentUser.Id;
                        }

                        continue;
                    }

                    // nový záznam
                    var subjectAllowedToProvider = new SubjectAllowedToProvider
                    {
                        CreatedByCustomerId = currentUser.Id,
                        CreatedDateUtc = DateTime.UtcNow,
                        Active = true,
                        SubjectAllowedToOrganizationId = subjectAllowedToOrganization.Id,
                        ProviderId = mappedProvider.Id
                    };
                    dbProvider.SubjectAllowedToProviders.Add(subjectAllowedToProvider);
                }

                // promažeme odebraná práva
                var allowedSubjectsToDelete = dbProvider.SubjectAllowedToProviders
                    .Where(w => !existingSubjectIds.Contains(w.Id) && !w.Deleted && w.Id > 0)
                    .ToList();

                // projdeme existující nesmazané a smažeme je
                foreach (var allowedSubjectToDelete in allowedSubjectsToDelete)
                {
                    allowedSubjectToDelete.Deleted = true;
                    allowedSubjectToDelete.UpdateDateUtc = DateTime.UtcNow;
                    allowedSubjectToDelete.UpdatedByCustomerId = currentUser.Id;
                }

                // uložení administrátorů
                SetProviderAdminUsers(currentUser.Id, mappedProvider);
        }

        await _providerRepository.UpdateProviderTaskAsync(dbProvider);
        return Ok();
    }

    private void SetProviderAdminUsers(int userId, Provider? provider)
    {      
        var providerAdminsOfProvider = _effectiveUserRepository.GetAdminUsersByProviderId(provider.Id, (int)RoleEnum.ProviderAdmin).ToList();

        // všichni SP za danou organizaci
        var providerAdminsOfOrganization = _effectiveUserRepository.GetAdminUsersByOrganizationId(provider.OrganizationId, (int)RoleEnum.ProviderAdmin).ToList();

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
                    EffectiveUser existingEffectiveProviderUser = _effectiveUserRepository.GetEffectiveUsersByProviderId(provider.Id).Where(w => w.UserId == effectiveAdminUser.UserId).FirstOrDefault();

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

