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
    /// <param name="providerId">Id požadovaného poskytovatele</param>
    /// <returns>GenericResponse s parametrem "success" TRUE a objektem "ProviderContract" nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné ProviderId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// </returns>
    [Authorize]
    [HttpGet(Name = "GetProvider")]
    public async Task<IGenericResponse<ProviderContract>> GetProvider(int providerId)
    {
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".GetProvider:";
        ProviderContract? provider = null;

        //Aktualni uživatel
        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<ProviderContract>(provider, false, -4, "Current user not found", "User not found by GlobalId.");
        }

        // kontrola na vstupní data
        if (providerId <= 0)
        {
            _logger.LogWarning("{0} Invalid ProviderId: {1}", logHeader, providerId);
             return new GenericResponse<ProviderContract>(provider, false, -2, "Invalid ProviderId", "ProviderId value must be greater then '0'.");
        }

        try
        {
            Provider? data = await _providerRepository.GetProviderByIdTaskAsync(providerId);

            if (data != null)
            {
                provider = data.ConvertToProviderContract(withAllowedSubjects: true);
            }

            TimeSpan endTime = DateTime.Now - startTime;

            _logger.LogInformation("{0} Data found, duration: {1}", logHeader, endTime);

            return new GenericResponse<ProviderContract>(provider, true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<ProviderContract>(provider, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrátí zíkladní údaje poskytovatele pro detail v listu
    /// </summary>
    /// <param name="providerId">Id požadovaného poskytovatele</param>
    /// <returns>GenericResponse s parametrem "success" TRUE a objektem "ProviderContract" nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné ProviderId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// </returns>
    [Authorize]
    [HttpGet(Name = "GetProviderListDetail")]
    public async Task<IGenericResponse<ProviderContract>> GetProviderListDetail(int providerId)
    {
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".GetProviderListDetail:";
        ProviderContract? provider = null;

        //Aktualni uživatel
        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<ProviderContract>(provider, false, -4, "Current user not found", "User not found by GlobalId.");
        }

        // kontrola na vstupní data
        if (providerId <= 0)
        {
            _logger.LogWarning("{0} Invalid ProviderId: {1}", logHeader, providerId);
            return new GenericResponse<ProviderContract>(provider, false, -2, "Invalid ProviderId", "ProviderId value must be greater then '0'.");
        }

        try
        {
            Provider? data = await _providerRepository.GetProviderListDetailByIdTaskAsync(providerId);

            if (data != null)
            {
                provider = data.ConvertToProviderContract(true, true, false, true);
            }

            TimeSpan endTime = DateTime.Now - startTime;

            _logger.LogInformation("{0} Data found, duration: {1}", logHeader, endTime);

            return new GenericResponse<ProviderContract>(provider, true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<ProviderContract>(provider, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrací seznam Poskytovatelů, buď pro grid nebo pro DropdownList
    /// </summary>
    /// <param name="fullData">Příznak, zda vracíme data pro grid (TRUE) nebo pro DropdownList (FALSE)</param>
    /// <param name="organizationId">Id organizace</param>
    /// <returns>GenericResponse s parametrem "success" TRUE a objektem "List<ProviderContract>" nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// </returns>
    [Authorize]
    [HttpGet(Name = "GetProviders")]
    public async Task<IGenericResponse<List<ProviderContract>>> GetProviders(bool fullData = true, int? organizationId = null)
    {
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".GetProviders:";
        List<ProviderContract> data = new List<ProviderContract>();

        //Aktualni uživatel
        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<List<ProviderContract>>(data, false, -4, "Current user not found", "User not found by GlobalId.");
        }

        try
        {
            var providers = await _providerRepository.GetProvidersTaskAsync(fullData, organizationId);

            if (providers != null)
            {
                if (fullData)
                    data = providers.Select(s => s.ConvertToProviderContract()).ToList();
                else
                    data = providers.Select(s => s.ConvertToProviderContract(false, false, false, false)).ToList();
            }

            TimeSpan endTime = DateTime.Now - startTime;

            _logger.LogInformation("{0} Data found, duration: {1}", logHeader, endTime);

            return new GenericResponse<List<ProviderContract>>(data, true, 0, records: data.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<List<ProviderContract>>(data, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Uloží nového poskytovatele
    /// </summary>
    /// <param name="providerContract"> objekt ProviderContract</param>
    /// <returns>GenericResponse s parametrem "success" TRUE a objektem "ProviderContract" nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -7 = poskytovatele se nepodařilo uložit
    /// </returns>
    [Authorize]
    [HttpPost(Name = "InsertProvider")]
    public async Task<IGenericResponse<ProviderContract>> InsertProvider([FromBody] ProviderContract? providerContract)
    {
        string logHeader = _logName + ".InsertProvider:";
        bool ret = false;
        DateTime startTime = DateTime.Now;

        try
        {
            // kontrola na vstupní data
            var mappedProvider = new Provider();
            mappedProvider = _mapper.Map<Provider>(providerContract);

            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                _logger.LogWarning($"{logHeader} Current User not found");
                return new GenericResponse<ProviderContract>(providerContract, false, -4, "Current user not found", "Current user not found by GlobalId.");
            }

            mappedProvider.CreatedByCustomerId = currentUser.Id;
            mappedProvider.CreatedDateUtc = DateTime.UtcNow;

            if (mappedProvider.Picture != null && mappedProvider.Picture.IsNew)
            {
                mappedProvider.Picture.CreatedDateUtc = DateTime.UtcNow;
                mappedProvider.Picture.CreatedByCustomerId = currentUser.Id;
                mappedProvider.Picture.Active = true;
            }

            int organizationId = currentUser.OrganizationId == null ? default(int) : currentUser.OrganizationId.Value;
            var allSubjectsAllowedToOrganization = await _subjectAllowedToOrganizationRepository.GetSubjectsAllowedToOrganizationsByOrganizationIdAsyncTask(organizationId);

            //povolené moduly
            foreach (var permission in providerContract.Permissions)
            {
                SubjectAllowedToOrganization? subjectAllowedToOrganization = allSubjectsAllowedToOrganization.Where(w => w.SubjectId == permission.SubjectId).FirstOrDefault();

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

            ret = await _providerRepository.InsertProviderTaskAsync(currentUser, mappedProvider);
            TimeSpan timeEnd = DateTime.Now - startTime;

            if (!ret)
            {
                _logger.LogWarning($"{logHeader} Provider '{mappedProvider.Name}', ID: {mappedProvider.Id} was not inserted, duration: {timeEnd}");
                return new GenericResponse<ProviderContract>(providerContract, false, -7, "Provider was not updated", "Error when updating provider.");
            }
            else
            {
                providerContract.Id = mappedProvider.Id;
                _logger.LogInformation($"{logHeader} Provider '{mappedProvider.Name}', ID: {mappedProvider.Id} was successfully inserted, duration: {timeEnd}");
                return new GenericResponse<ProviderContract>(providerContract, true, 0);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<ProviderContract>(providerContract, false, -1, ex.Message);
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
    /// -8 = poskytovatele se nepodařilo uložit
    /// </returns>
    [Authorize]
    [HttpPost(Name = "UpdateProvider")]
    public async Task<IGenericResponse<bool>> UpdateProvider([FromBody] ProviderContract? providerContract)
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

            var dbProvider = await _providerRepository.GetProviderByIdTaskAsync(mappedProvider.Id);
            if (dbProvider == null)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Provider not found");
                return new GenericResponse<bool>(ret, false, -5, "Provider not found", "Provider not found by Id.");
            }

            mappedProvider.EffectiveUsers = providerContract.AdminUsers.Select(s => new EffectiveUser()
            {
                Active = s.Active,
                Deleted = s.Deleted,
                Id = s.Id,
                UserId = s.UserId,
                ProviderId = s.ProviderId
            }).ToList();

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
            List<int> selectedSubjectIds = providerContract.Permissions.GroupBy(x => x.SubjectId)
                .Select(grp => grp.Key).ToList();

            int organizationId = currentUser.OrganizationId == null ? default(int) : currentUser.OrganizationId.Value;

            var allSubjectsAllowedToOrganization = await _subjectAllowedToOrganizationRepository.GetSubjectsAllowedToOrganizationsByOrganizationIdAsyncTask(organizationId);

            foreach (var selectedSubjectId in selectedSubjectIds)
            {
                SubjectAllowedToOrganization? subjectAllowedToOrganization = allSubjectsAllowedToOrganization.Where(w => w.SubjectId == selectedSubjectId).FirstOrDefault();

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

            TimeSpan timeMiddle = DateTime.Now - startTime;

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

            ret = await _providerRepository.UpdateProviderTaskAsync(currentUser, dbProvider);

            TimeSpan timeEnd = DateTime.Now - startTime;

            if (!ret)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Provider '{dbProvider.Name}', ID: {dbProvider.Id} was not updated, duration: {timeEnd}, middle: {timeMiddle}");
                return new GenericResponse<bool>(ret, false, -8, "Provider was not updated", "Error when updating provider.");
            }

            else
            {
                tran.Commit();
                _logger.LogInformation($"{logHeader} Provider '{dbProvider.Name}', ID: {dbProvider.Id} was successfully updated, duration: {timeEnd}, middle: {timeMiddle}");
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
    /// Smaže poskytovatele včetně jeho vazeb na efektivní uživatele
    /// </summary>
    /// <param name="providerId">Id Providera</param>
    /// <returns>GenericResponse s parametrem "success" TRUE nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné ProviderId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = poskytovatele se nepodařilo dohledat podle ID
    /// -9 = poskytovatele se nepodařilo smazat
    /// </returns>
    [Authorize]
    [HttpGet(Name = "DeleteProvider")]
    public async Task<IGenericResponse<bool>> DeleteProvider(int providerId)
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

            if (providerId <= 0)
            {
                _logger.LogWarning("{0} Invalid ProviderId: {1}", logHeader, providerId);
                return new GenericResponse<bool>(ret, false, -2, "Invalid ProviderId", "ProviderId value must be greater then '0'.");
            }

            var dbProvider = await _providerRepository.GetProviderByIdTaskAsync(providerId);
            if (dbProvider == null)
            {
                tran.Rollback();
                _logger.LogWarning("{0} Provider not found, Id: {0}", logHeader, providerId);
                return new GenericResponse<bool>(ret, false, -5, "Provider not found", "Provider not found by Id.");
            }

            dbProvider.Deleted = true;
            dbProvider.Active = false;

            // odstranění vazby uživatelů na smazaného poskytovatele
            foreach (var providerEffectiveUser in dbProvider.EffectiveUsers)
            {
                providerEffectiveUser.Deleted = true;
                providerEffectiveUser.UpdateDateUtc = DateTime.UtcNow;
                providerEffectiveUser.UpdatedByCustomerId = currentUser.Id;
            }

            ret = await _providerRepository.UpdateProviderTaskAsync(currentUser, dbProvider);
            TimeSpan timeEnd = DateTime.Now - startTime;

            if (!ret)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Provider '{dbProvider.Name}', Id: {dbProvider.Id} was not deleted, duration: {timeEnd}");
                return new GenericResponse<bool>(ret, false, -9, "Provider was not deleted", "Error when deleting provider.");
            }

            
            // odstranění vazby u uživatele
            var users = await _customerRepository.GetCustomersTaskAsyncTask(dbProvider.Id);

            // projdeme uživatele, pokud uživatel nemá vazbu na jiného poskytovatele, tak ho smažeme
            foreach (var usr in users)
            {
                usr.Deleted = true;
                if (usr.EffectiveUserUsers.Count > 0)
                {
                    var anotherEF = usr.EffectiveUserUsers.FirstOrDefault(f => !f.Deleted && f.ProviderId != dbProvider.Id);
                    if (anotherEF != null)
                    {
                        // našli jsme vazbu na jiného poskytovatele, tak ho nastavíme
                        usr.CreatedByProviderId = anotherEF.ProviderId;
                        usr.Deleted = false;
                    }
                }

                // super admin nebo správce organizace nebude smazán, jen odebereme vazbu na poskytovatele
                if (usr.IsSuperAdminAccount || usr.IsOrganizationAdminAccount)
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
                    _logger.LogWarning($"{logHeader} User '{usr.FriendlyName}', Email: '{usr.Email}', Id: {usr.Id} was not deleted from Provider '{dbProvider.Name}', Id: {dbProvider.Id}, duration: {timeEnd}");
                    return new GenericResponse<bool>(ret, false, -9, "Provider was not deleted", "Error when deleting user from Provider.");
                }
            }
            
            ret = true;

            tran.Commit();
            _logger.LogInformation($"{logHeader} Provider '{dbProvider.Name}', Id: {dbProvider.Id} was successfully deleted, duration: {timeEnd}");
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
    /// Obsluha efektivních uživatelů v roli Správce poskytovatele u Poskytovatele
    /// </summary>
    /// <param name="customer">Aktuální uživatel</param>
    /// <param name="provider">Poskytovatel</param>
    private async void SetProviderAdminUsers(CompleteUserContract customer, Provider provider)
    {
        // všichni SP za danou organizaci
        var providerAdminsOfOrganization = _effectiveUserRepository.GetEffectiveUsersByOrganizationId(provider.OrganizationId).Where(p => p.RoleMembers.Any(r => r.RoleId == (int)RoleEnum.ProviderAdmin && r.Active.Value && !r.Deleted)).ToList();

        var providerAdminsOfProvider = providerAdminsOfOrganization.Where(p => p.Provider.Id == provider.Id).ToList();

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
                    EffectiveUser? existingEffectiveProviderUser = providerAdminsOfOrganization.Where(p => p.Provider.Id == provider.Id && p.UserId == effectiveAdminUser.UserId).FirstOrDefault();

                    // přidání role Správce poskytovatele
                    var roleM = new RoleMember
                    {
                        Active = true,
                        CreatedByCustomerId = customer.Id,
                        CreatedDateUtc = DateTime.UtcNow,
                        Deleted = false,
                        RoleId = (int)RoleEnum.ProviderAdmin
                    };

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

                        roleM.EffectiveUserId = existingEffectiveProviderUser.Id;

                        existingEffectiveProviderUser.RoleMembers.Add(roleM);
                        // přidání EF
                        _effectiveUserRepository.InsertEffectiveUser(customer, existingEffectiveProviderUser);
                    }
                    else
                    {
                        // EF uživatel je neaktivní
                        if (!existingEffectiveProviderUser.Active.GetValueOrDefault())
                        {
                            existingEffectiveProviderUser.Active = true;
                            existingEffectiveProviderUser.UpdatedByCustomerId = customer.Id;
                            existingEffectiveProviderUser.UpdateDateUtc = DateTime.UtcNow;
                            roleM.EffectiveUserId = existingEffectiveProviderUser.Id;
                            existingEffectiveProviderUser.RoleMembers.Add(roleM);
                            await _effectiveUserRepository.UpdateEffectiveUserTaskAsync(customer, existingEffectiveProviderUser);
                        }
                    }
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
            // seznam rolí SP
            var roleMembers = effectiveAdminUserToDelete.RoleMembers
                .Where(rm => rm.RoleId == (int)RoleEnum.ProviderAdmin);

            // odebereme roli SP
            foreach (var roleMember in roleMembers)
            {
                roleMember.Deleted = true;
                roleMember.UpdateDateUtc = DateTime.UtcNow;
                roleMember.UpdatedByCustomerId = customer.Id;
            }

            // nemá jiné role u tohoto poskytovatele než roli SP, zneaktivníme ho a smažeme
            if (!roleMembers.Any(a => a.RoleId != (int)RoleEnum.ProviderAdmin))
            {
                effectiveAdminUserToDelete.Active = false;
                effectiveAdminUserToDelete.Deleted = true;
            }

            _effectiveUserRepository.UpdateEffectiveUser(customer, effectiveAdminUserToDelete);
        }
    }
}

