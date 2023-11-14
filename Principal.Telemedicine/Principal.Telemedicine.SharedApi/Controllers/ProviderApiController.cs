using AutoMapper;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.TermStore;
using Newtonsoft.Json.Linq;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.Shared.Enums;
using Principal.Telemedicine.Shared.Models;
using Principal.Telemedicine.Shared.Security;
using System.Data;
using System.Linq;

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

            mappedProvider = _mapper.Map<ProviderContract>(provider);

            return Ok(mappedProvider);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Uloží nového poskytovatele
    /// </summary>
    /// <param name="providerContract"> objekt ProviderContract</param>
    /// <returns>GenericResponse s parametrem "success" TRUE nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    [Authorize]
    [HttpPost(Name = "InsertProvider")]
    public async Task<IGenericResponse<bool>> InsertProvider([FromBody] ProviderContract? providerContract)
    {
        string logHeader = _logName + ".InsertProvider:";
        bool ret = false;
        DateTime startTime = DateTime.Now;
        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // kontrola na vstupní data
            var mappedProvider = new Provider();
            mappedProvider = _mapper.Map<Provider>(providerContract);

            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Current User not found");
                return new GenericResponse<bool>(ret, false, -4, "Current user not found", "Current user not found by GlobalId.");
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

            ret = await _providerRepository.InsertProviderTaskAsync(mappedProvider);
            TimeSpan timeEnd = DateTime.Now - startTime;

            if (!ret)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Provider '{mappedProvider.Name}', ID: {mappedProvider.Id} was not inserted, duration: {timeEnd}");
                return new GenericResponse<bool>(ret, false, -1, "Provider was not updated", "Error when updating provider.");
            }

            else
            {
                tran.Commit();
                _logger.LogInformation($"{logHeader} Provider '{mappedProvider.Name}', ID: {mappedProvider.Id} was successfully inserted, duration: {timeEnd}");
                return new GenericResponse<bool>(ret, true, 0);
            }
        }

        catch (Exception ex)
        {
            tran.Rollback();
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<bool>(false, false, -1, ex.Message);
        }

    }

    /// <summary>
    /// Updatuje poskytovatele
    /// </summary>
    /// <param name="providerContract"> objekt ProviderContract</param>
    /// <returns>GenericResponse s parametrem "success" TRUE nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = poskytovatele se nepodařilo dohledat podle ID
    /// -6 = subjekty pro organizaci se nepodařilo dohledat podle ID a OrganizationId
    [Authorize]
    [HttpPost(Name = "UpdateProvider")]
    public async Task<IGenericResponse<bool>> UpdateProvider( [FromBody] ProviderContract? providerContract)
    {
        string logHeader = _logName + ".UpdateProvider:";
        bool ret = false;
        DateTime startTime = DateTime.Now;
        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // kontrola na vstupní data
            var mappedProvider = new Provider();
            mappedProvider = _mapper.Map<Provider>(providerContract);

            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Current User not found");
                return new GenericResponse<bool>(ret, false, -4, "Current user not found", "Current user not found by GlobalId.");
            }

            var dbProvider = _providerRepository.GetProviderById(mappedProvider.Id);
            if (dbProvider == null)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Provider not found");
                return new GenericResponse<bool>(ret, false, -5, "Provider not found", "Provider not found by Id.");
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
                        tran.Rollback();
                        _logger.LogWarning($"{logHeader} Subjects allowed to organization not found");
                        return new GenericResponse<bool>(ret, false, -6, "Subjects allowed to organization not found", "Subjects allowed to organization not found by Id.");
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
                SetProviderAdminUsers(currentUser, mappedProvider);
                ret = await _providerRepository.UpdateProviderTaskAsync(dbProvider, tran, true);

                TimeSpan timeEnd = DateTime.Now - startTime;

                if (!ret)
                {
                    tran.Rollback();
                    _logger.LogWarning($"{logHeader} Provider '{dbProvider.Name}', ID: {dbProvider.Id} was not updated, duration: {timeEnd}");
                    return new GenericResponse<bool>(ret, false, -1, "Provider was not updated", "Error when updating provider.");
                }

                else
                {
                    tran.Commit();
                    _logger.LogInformation($"{logHeader} Provider '{dbProvider.Name}', ID: {dbProvider.Id} was successfully updated, duration: {timeEnd}");
                    return new GenericResponse<bool>(ret, true, 0);
                }
            }

            tran.Commit();
            return new GenericResponse<bool>(ret, true, 0);
        }
        catch (Exception ex)
        {
            tran.Rollback();
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<bool>(false, false, -1, ex.Message);
        }

    }

    /// <summary>
    /// Smaže poskytovatele včetně jeho vazeb na efektivní uživatele
    /// </summary>
    /// <param name="providerContract"> objekt Providera</param>
    /// <returns>GenericResponse s parametrem "success" TRUE nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -3 = chybí GlobalId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = poskytovatele se nepodařilo dohledat podle ID
    /// </returns>
    [Authorize]
    [HttpPost(Name = "DeleteProvider")]
    public async Task<IGenericResponse<bool>> DeleteProvider([FromBody] ProviderContract? providerContract)
    {
        string logHeader = _logName + ".DeleteProvider:";
        bool ret = false;
        DateTime startTime = DateTime.Now;

        using var tran = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // kontrola na vstupní data
            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                tran.Rollback();
                _logger.LogWarning("{0} Current User not found", logHeader);
                return new GenericResponse<bool>(ret, false, -4, "Current user not found", "Current user not found by GlobalId.");
            }

            var mappedProvider = new Provider();
            mappedProvider = _mapper.Map<Provider>(providerContract);

            var dbProvider = _providerRepository.GetProviderById(mappedProvider.Id);
            if (dbProvider == null)
            {
                tran.Rollback();
                _logger.LogWarning("{0} Provider not found", logHeader);
                return new GenericResponse<bool>(ret, false, -5, "Provider not found", "Provider not found by Id.");
            }

            if (_providerRepository.ListOfAllProviders().Any(i => i.Id == mappedProvider.Id))
            {
                dbProvider.Deleted = true;
                dbProvider.Active = false;
                ret = await _providerRepository.UpdateProviderTaskAsync(dbProvider, tran, true);
                TimeSpan timeEnd = DateTime.Now - startTime;

                if (!ret)
                {
                    tran.Rollback();
                    _logger.LogWarning($"{logHeader} Provider '{dbProvider.Name}', Id: {dbProvider.Id} was not updated, duration: {timeEnd}");
                    return new GenericResponse<bool>(ret, false, -1, "Provider was not updated", "Error when deleting provider.");
                }

                // odstranění vazby uživatelů na smazaného poskytovatele
                var providerEffectiveUsers = _effectiveUserRepository.GetEffectiveUsersByOrganizationId(mappedProvider.OrganizationId).Where(p => p.Provider.Id == mappedProvider.Id).ToList();

                foreach (var providerEffectiveUser in providerEffectiveUsers)
                {
                    providerEffectiveUser.Deleted = true;
                    providerEffectiveUser.UpdateDateUtc = DateTime.UtcNow;
                    providerEffectiveUser.UpdatedByCustomerId = currentUser.Id;
                    ret = await _effectiveUserRepository.UpdateEffectiveUserTaskAsync(currentUser, providerEffectiveUser);
                    timeEnd = DateTime.Now - startTime;
                    if (!ret)
                    {
                        tran.Rollback();
                        _logger.LogWarning($"{logHeader} Effective user was not updated, ID: {providerEffectiveUser.Id}, duration: {timeEnd}");
                        return new GenericResponse<bool>(ret, false, -1, "Effective user was not updated", "Error when deleting provider.");
                    }
                }
                // odstranění vazby u uživatele
                var users = await _customerRepository.GetCustomersTaskAsyncTask(mappedProvider.Id);
                users = users.Where(w => !w.Deleted);
                // projdeme uživatele, pokud uživatel nemá vazbu na jiného poskytovatele, tak ho smažeme
                foreach (var usr in users)
                {
                    usr.Deleted = true;
                    if (usr.EffectiveUserUsers.Count > 0)
                    {
                        var anotherEF = usr.EffectiveUserUsers.FirstOrDefault(f => !f.Deleted && f.ProviderId != mappedProvider.Id);
                        if (anotherEF != null)
                        {
                            // našli jsme vazbu na jiného poskytovatele, tak ho nastavíme
                            usr.CreatedByProviderId = anotherEF.ProviderId;
                            usr.Deleted = false;
                        }
                    }

                    // super admin nebo správce organizace nebude smazán, jen odebereme vazbu na poskytovatele
                    if (usr.IsSuperAdminAccount || usr.IsOrganizationAdminAccount) //usr.IsGlobalAdmin() || usr.IsOrganizationAdmin()
                    {
                        usr.Deleted = false;
                        usr.CreatedByProviderId = null;
                    }

                    usr.UpdateDateUtc = DateTime.UtcNow;
                    usr.UpdatedByCustomerId = currentUser.Id;

                    int retUser = await _customerRepository.UpdateCustomerTaskAsync(currentUser, usr, true, tran, true);

                    timeEnd = DateTime.Now - startTime;

                    if (retUser != 1)
                    {
                        tran.Rollback();
                        _logger.LogWarning($"{logHeader} User '{usr.FriendlyName}', Email: '{usr.Email}', Id: {usr.Id} was not updated, duration: {timeEnd}");
                        return new GenericResponse<bool>(ret, false, -1, "User was not updated", "Error when deleting user.");
                    }
                    else
                    {
                        tran.Commit();
                        _logger.LogInformation($"{logHeader} User '{usr.FriendlyName}', Email: '{usr.Email}', Id: {usr.Id} was successfully updated, duration: {timeEnd}");
                        return new GenericResponse<bool>(ret, true, 0);
                    }
                }
            }
            tran.Commit();
            return new GenericResponse<bool>(ret, true, 0);
        }
        catch (Exception ex)
        {
            tran.Rollback();
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<bool>(false, false, -1, ex.Message);
        }
    }

    private void SetProviderAdminUsers(CompleteUserContract customer, Provider provider)
    {
        var providerAdminsOfProvider = _effectiveUserRepository.GetEffectiveUsersByOrganizationId(provider.OrganizationId).Where(p => p.Provider.Id == provider.Id && p.RoleMembers.Any(r => r.RoleId == (int)RoleEnum.ProviderAdmin && !r.Deleted)).ToList();

        // všichni SP za danou organizaci
        var providerAdminsOfOrganization = _effectiveUserRepository.GetEffectiveUsersByOrganizationId(provider.OrganizationId).Where(p => p.RoleMembers.Any(r => r.RoleId == (int)RoleEnum.ProviderAdmin && r.Active.Value && !r.Deleted)).ToList();

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
                    EffectiveUser? existingEffectiveProviderUser = _effectiveUserRepository.GetEffectiveUsersByOrganizationId(provider.OrganizationId).Where(p => p.Provider.Id == provider.Id && p.UserId == effectiveAdminUser.UserId).FirstOrDefault();

                    if (existingEffectiveProviderUser == null)
                    {
                        // máme EF uživatele jiného poskytovatele, musíme si vytvořit svého EF uživatele
                        existingEffectiveProviderUser = new EffectiveUser()
                        {
                            Active = true,
                            CreatedByCustomerId = customer.Id,
                            CreatedDateUtc = DateTime.UtcNow,
                            Deleted = false,
                            UserId = existingEffectiveAdminUser.UserId,
                            ProviderId = provider.Id,
                        };
                        // přidání EF
                        _effectiveUserRepository.InsertEffectiveUserTaskAsync(customer, existingEffectiveProviderUser);
                    }
                    else
                    {
                        // EF uživatel je neaktivní
                        if (!existingEffectiveProviderUser.Active.GetValueOrDefault())
                        {
                            existingEffectiveProviderUser.Active = true;
                            existingEffectiveProviderUser.UpdatedByCustomerId = customer.Id;
                            existingEffectiveProviderUser.UpdateDateUtc = DateTime.UtcNow;
                            _effectiveUserRepository.UpdateEffectiveUserTaskAsync(customer, existingEffectiveProviderUser);
                        }
                    }

                    // přidání role Správce poskytovatele
                    _roleMemberRepository.InsertRoleMemberTaskAsync(new RoleMember
                    {
                        Active = true,
                        CreatedByCustomerId = customer.Id,
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
                    roleMember.UpdatedByCustomerId = customer.Id;
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
                effectiveAdminUserToDelete.UpdatedByCustomerId = customer.Id;
                effectiveAdminUserToDelete.UpdateDateUtc = DateTime.UtcNow;
                _effectiveUserRepository.UpdateEffectiveUserTaskAsync(customer, effectiveAdminUserToDelete);
            }
        }
    }
}

