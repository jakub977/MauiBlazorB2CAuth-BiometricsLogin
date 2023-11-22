using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Models;
using Principal.Telemedicine.Shared.Utils;
using Microsoft.Data.SqlClient;
using System.Data;
using Principal.Telemedicine.DataConnectors.Extensions;
using Principal.Telemedicine.Shared.Enums;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.DataConnectors.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using Principal.Telemedicine.Shared.Security;
using Microsoft.Graph.Models;

namespace Principal.Telemedicine.SharedApi.Controllers;

/// <summary>
/// API metody vztažené k uživateli
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class UserApiController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IEffectiveUserRepository _effectiveUserRepository;
    private readonly IADB2CRepository _adb2cRepository;
    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    private readonly string _logName = "UserApiController";

    public UserApiController(ICustomerRepository customerRepository, IProviderRepository providerRepository, IEffectiveUserRepository effectiveUserRepository,
        IADB2CRepository adb2cRepository, DbContextApi dbContext, ILogger<UserApiController> logger, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _providerRepository = providerRepository;
        _effectiveUserRepository = effectiveUserRepository;
        _adb2cRepository = adb2cRepository;
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Vrátí základní údaje uživatele.
    /// </summary>
    /// <param name="userId">Id uživatele</param>
    /// <returns>GenericResponse s parametrem "success" TRUE a objektem "UserContract" nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné UserId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// </returns>
    [Authorize]
    [HttpGet(Name = "GetUserInfo")]
    public async Task<IGenericResponse<UserContract>> GetUserInfo(int userId)
    {
        string logHeader = _logName + ".GetUserInfo:";

        // kontrola na vstupní data
        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<UserContract>(null, false, -4, "Current user not found", "User not found");
        }

        if (userId <= 0)
        {
            _logger.LogWarning("{0} Invalid UserId: {1}", logHeader, userId);
            return new GenericResponse<UserContract>(null, false, -2, "Invalid UserId", "UserId value is not > '0'.");
        }

        try
        {
            var user = await _customerRepository.GetCustomerInfoByIdTaskAsync(userId);
            var mappedUser = _mapper.Map<UserContract>(user);

            return new GenericResponse<UserContract>(mappedUser, true, 0);

        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<UserContract>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrátí údaje uživatele včetně rolí, efektivních uživatelů a oprávnění.
    /// </summary>
    /// <param name="globalId">GlobalId uživatele, který metodu volá</param>
    /// <param name="userId">Id uživatele, kterého chceme vrátit, pokud je jiný než ten co metodu volá</param>
    /// <returns>GenericResponse s parametrem "success" TRUE a objektem "CompleteUserContract" nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné UserId
    /// -3 = chybí GlobalId
    /// </returns>
    [Authorize]
    [HttpGet(Name = "GetUser")]
    public async Task<IGenericResponse<CompleteUserContract>> GetUser(string? globalId = null, int? userId = null)
    {
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".GetUser:";

        //Aktualni uživatel
        CompleteUserContract? actualUser = HttpContext.GetTmUser();
        //Pokud nejsou vstupní parametry, nahrazuji globalId uživatelem volání
        if ((!userId.HasValue || userId.Value <= 0) && string.IsNullOrEmpty(globalId)) globalId = actualUser?.GlobalId;

        // kontrola na vstupní data
        if ((!userId.HasValue || userId.Value <= 0) && string.IsNullOrEmpty(globalId))
        {
            _logger.LogWarning("{0} Invalid UserId: {1} or GlobalId: {2}", logHeader, userId, globalId);
            if (userId > 0)
                return new GenericResponse<CompleteUserContract>(new CompleteUserContract(), false, -2, "Invalid UserId", "UserId value must be greater then '0'.");
            else
                return new GenericResponse<CompleteUserContract>(new CompleteUserContract(), false, -3, "GlobalId is empty", "GlobalId must be set.");
        }

        try
        {
            var mappedUser = new CompleteUserContract();
            int id = userId.HasValue ? userId.Value : 0;
            TimeSpan middleTime = DateTime.Now - startTime;
            if (id <= 0)
            {
                var user = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
                middleTime = DateTime.Now - startTime;
                //mappedUser = _mapper.Map<CompleteUserContract>(user);
                mappedUser = user.ConvertToCompleteUserContract(false, false, true, true, false, true);
            }
            else
            {
                // todo: zjistit jestli má uživatel oprávnění read na jiného uživatele
                // možno nastudovat v Vanda -> SmartMVC.Services -> Customers -> CustomerService.cs, metoda GetAllCustomers na ř. 161
                var user = await _customerRepository.GetCustomerByIdTaskAsync(id);

                middleTime = DateTime.Now - startTime;

                //mappedUser = _mapper.Map<CompleteUserContract>(user);
                mappedUser = user.ConvertToCompleteUserContract(false, false, true, true, false, true);
            }

            TimeSpan endTime = DateTime.Now - startTime;

            _logger.LogInformation("{0} Data found, duration: {1}, middle: {2}", logHeader, endTime, middleTime);

            return new GenericResponse<CompleteUserContract>(mappedUser, true,0);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<CompleteUserContract>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrátí seznam uživatelů pro Kendo grid
    /// </summary>
    /// <param name="activeUsersOnly">Filtrování - pouze aktivní uživatelé?</param>
    /// <param name="filterRole">Filtrování - podle ID role</param>
    /// <param name="filterGroup">Filtrování - podle ID skupiny</param>
    /// <param name="searchText">Filtrování - hledaný text ve jméně a adrese</param>
    /// <param name="order">Řazení</param>
    /// <param name="page">Požadovaná stránka</param>
    /// <param name="pageSize">Počet záznamů na stránce</param>
    /// <param name="providerId">Id Poskytovatele</param>
    /// <returns>GenericResponse s parametrem "success" a seznamem CompleteUserContract nebo chybu:
    /// -1 = obecná chyba
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen</returns>
    [Authorize]
    [HttpGet(Name = "GetUsers")]
    public async Task<IGenericResponse<List<CompleteUserContract>>> GetUsers(bool activeUsersOnly, int? filterRole, int? filterGroup, string? searchText, string? order = "created_desc", int? page = 1, int? pageSize = 20, int? providerId = null)
    {
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".GetUsers:";
        // kontrola na vstupní data
        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<List<CompleteUserContract>>(null, false, -4, "Current user not found", "User not found by GlobalId.");
        }

        try
        {
            List<CompleteUserContract> data = new List<CompleteUserContract>();

            PaginatedListData<Customer> resultData = await _customerRepository.GetCustomersTaskAsync(currentUser, activeUsersOnly, filterRole, filterGroup, searchText, order, page, pageSize, providerId);

            TimeSpan timeMiddle = DateTime.Now - startTime;

            foreach (var item in resultData)
            {
                data.Add(item.ConvertToCompleteUserContract(false, false, false, false, true));
            }

            TimeSpan timeEnd = DateTime.Now - startTime;
            _logger.LogInformation("{0} Returning data - page: {1}, records: {2}, TotalRecords: {3}, TotalPages: {4}, duration: {5}, middle: {6}", logHeader, resultData.ActualPage, resultData.Count, resultData.TotalRecords, resultData.TotalPages, timeEnd, timeMiddle);

            return new GenericResponse<List<CompleteUserContract>>(data, true, 0, null, null, resultData.TotalRecords);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<List<CompleteUserContract>>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Uloží změny uživatele
    /// </summary>
    /// <param name="user">Objekt uživatele</param>
    /// <returns>GenericResponse s parametrem "success" TRUE nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné UserId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = uživatele se nepodařilo dohledat podle ID
    /// -11 = uživatel se stejným emailem existuje
    /// -12 = uživatel se stejným tel. číslem existuje
    /// -13 = uživatel se stejným PersonalIdentificationNumber existuje
    /// -14 = uživatel se stejným GlobalID existuje
    /// </returns>
    [Authorize]
    [HttpPost(Name = "UpdateUser")]
    public async Task<IGenericResponse<bool>> UpdateUser(CompleteUserContract user, int? providerId = null, bool isProviderAdmin = false)
    {
        string logHeader = _logName + ".UpdateUser:";
        DateTime startTime = DateTime.Now;
        using var tran = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // kontrola na vstupní data
            if (user.Id <= 0)
            {
                _logger.LogWarning("{0} Invalid UserId: {1}", logHeader, user.Id);
                return new GenericResponse<bool>(false, false,-2, "Invalid UserId", "UserId value must be greater then '0'.");
            }

            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                _logger.LogWarning("{0} Current User not found", logHeader);
                return new GenericResponse<bool>(false, false, -4, "Current user not found", "User not found by GlobalId.");
            }

            Customer? actualData = await _customerRepository.GetCustomerByIdOnlyForUpdateTaskAsync(user.Id);
            if (actualData == null)
            {
                _logger.LogWarning("{0} User not found, Name: {1}, ID: {2}, Email: {3}", logHeader, user.FriendlyName, user.Id, user.Email);
                return new GenericResponse<bool>(false, false, -5, "User not found", "User not found by Id.");
            }

            int checkRet = await _customerRepository.CheckIfUserExists(currentUser, actualData);
            if (checkRet < 0)
            {
                _logger.LogWarning("{0} User already exists", logHeader);
                return new GenericResponse<bool>(false, false, checkRet, "User already exists", "User with same data already exists in DB.");
            }

            bool haveEFUser = false;

            if (actualData.Deleted)
            {
                actualData.Deleted = false;
            }

            actualData.UpdateDateUtc = DateTime.UtcNow;
            actualData.UpdatedByCustomerId = currentUser.Id;

            string oldEmail = actualData.Email;

            // pokud jde o editaci efektivního uživatele a exituje ještě jiný aktivní efektivní uživatel pro danou entitu Customer,
            // je potřeba ponechat záznam Customer v aktivním stavu 
            if (user.EffectiveUserUsers.Any() && providerId.HasValue)
            {
                var editedEfUser = user.EffectiveUserUsers.First(u => !u.Deleted && u.ProviderId == providerId.Value);
                var existingEfUsers = await _effectiveUserRepository.GetEffectiveUsersTaskAsync(user.Id);
                existingEfUsers = existingEfUsers.Where(x => x.Id != editedEfUser.Id).ToList();

                if (!user.Active && existingEfUsers.Any(x => x.Active.Value))
                    user.Active = true;

            }

            actualData.Active = user.Active;
            actualData.AddressLine = user.AddressLine;
            actualData.Birthdate = user.Birthdate;
            actualData.CityId = user.CityId;
            actualData.Email = user.Email;
            actualData.FirstName = user.FirstName;
            actualData.GenderTypeId = user.GenderTypeId;
            actualData.HealthCareInsurerCode = user.HealthCareInsurerCode;
            actualData.HealthCareInsurerId = user.HealthCareInsurerId.HasValue && user.HealthCareInsurerId.Value > 0 ? user.HealthCareInsurerId.Value : null;
            actualData.IsSystemAccount = user.IsSystemAccount;
            actualData.LastName = user.LastName;
            actualData.TitleBefore = user.TitleBefore;
            actualData.TitleAfter = user.TitleAfter;
            actualData.OrganizationId = user.OrganizationId;
            actualData.PersonalIdentificationNumber = user.PersonalIdentificationNumber;
            actualData.BirthIdentificationNumber = user.BirthIdentificationNumber;
            actualData.GynecologistNote = user.GynecologistNote;

            if (user.Picture != null && user.Picture.IsNew)
            {
                if (user.Picture.Id > 0)
                {
                    if (actualData.Picture == null)
                    {
                        actualData.Picture = new Picture();
                        actualData.Picture.Active = true;
                        actualData.Picture.CreatedDateUtc = DateTime.UtcNow;
                        actualData.Picture.CreatedByCustomerId = currentUser.Id;
                    }

                    actualData.Picture.MediaStorage = _mapper.Map<MediaStorage>(user.Picture.MediaStorage);
                    actualData.Picture.MimeType = user.Picture.MimeType;
                    actualData.Picture.FriendlyName = user.Picture.FriendlyName;
                    actualData.Picture.UpdatedOnUtc = DateTime.UtcNow;
                    actualData.Picture.UpdatedByCustomerId = currentUser.Id;
                    actualData.Picture.Active = user.Picture.Active;
                }
                else
                {
                    actualData.Picture = _mapper.Map<Picture>(user.Picture);
                    actualData.Picture.CreatedDateUtc = DateTime.UtcNow;
                    actualData.Picture.CreatedByCustomerId = currentUser.Id;
                }
            }

            actualData.PostalCode = user.PostalCode;
            actualData.PublicIdentifier = user.PublicIdentifier;
            actualData.TelephoneNumber = user.TelephoneNumber;

            actualData.IsProviderAdminAccount = user.IsProviderAdminAccount;
            actualData.IsOrganizationAdminAccount = user.IsOrganizationAdminAccount;
            actualData.IsSuperAdminAccount = user.IsSuperAdminAccount;

            // nový nebo stávající
            // vždy pracujeme jen s jedním EF uživatelem, protože jeden EF uživatel se vztahuje ke konkrétnímu Poskytovatli (ProviderId)
            // a uživatelé se aktualizují vždy pod konkrétním Poskytovatelem (pokud se jedná o EF uživatele).
            // Pokud nemá mít uživatel Poskytovatele (SuparAdmin apod), pak nemá ani EF uživatele
            foreach (var efUser in user.EffectiveUserUsers)
            {
                var existingEfUser = actualData.EffectiveUserUsers.Where(w => !w.Deleted && w.UserId == efUser.UserId && w.ProviderId == efUser.ProviderId).FirstOrDefault();

                #region Nový EF uživatel

                if (existingEfUser == null)
                {
                    existingEfUser = _mapper.Map<EffectiveUser>(efUser);
                    existingEfUser.CreatedByCustomerId = currentUser.Id;
                    existingEfUser.CreatedDateUtc = DateTime.UtcNow;
                    existingEfUser.Active = user.Active;
                    existingEfUser.Deleted = false;
                    actualData.CreatedByProviderId = existingEfUser.ProviderId;

                    foreach (var group in existingEfUser.GroupEffectiveMembers)
                    {
                        group.Active = true;
                        group.Deleted = false;
                        group.CreatedByCustomerId = currentUser.Id;
                        group.CreatedDateUtc = DateTime.UtcNow;
                    }

                    foreach (var role in existingEfUser.RoleMembers)
                    {
                        role.Active = true;
                        role.Deleted = false;
                        role.CreatedByCustomerId = currentUser.Id;
                        role.CreatedDateUtc = DateTime.UtcNow;
                    }

                    var efUserToSave = _mapper.Map<EffectiveUser>(existingEfUser);

                    await _effectiveUserRepository.InsertEffectiveUserTaskAsync(currentUser, efUserToSave);
                    haveEFUser = true;
                    continue;
                }

                #endregion

                if (existingEfUser.Deleted)
                {
                    existingEfUser.Deleted = false;
                }

                haveEFUser = true;
                existingEfUser.Active = efUser.Active;
                existingEfUser.UpdateDateUtc = DateTime.UtcNow;
                existingEfUser.UpdatedByCustomerId = currentUser.Id;

                if (!actualData.CreatedByProviderId.HasValue)
                    actualData.CreatedByProviderId = existingEfUser.ProviderId;

                #region Skupiny

                //nové a stávající skupiny
                foreach (GroupEffectiveMemberContract group in efUser.GroupEffectiveMembers)
                {
                    GroupEffectiveMember? existingItem = existingEfUser.GroupEffectiveMembers.Where(w => w.GroupId == group.GroupId && w.EffectiveUserId == existingEfUser.Id).FirstOrDefault();
                    if (existingItem == null)
                    {
                        group.CreatedByCustomerId = currentUser.Id;
                        group.CreatedDateUtc = DateTime.UtcNow;
                        group.Active = true;
                        group.Deleted = false;
                        group.EffectiveUserId = existingEfUser.Id;
                        existingEfUser.GroupEffectiveMembers.Add(_mapper.Map<GroupEffectiveMember>(group));
                        continue;
                    }

                    if (!existingItem.Active.GetValueOrDefault() || existingItem.Deleted)
                    {
                        existingItem.Active = true;
                        existingItem.Deleted = false;
                        existingItem.UpdateDateUtc = DateTime.UtcNow;
                        existingItem.UpdatedByCustomerId = currentUser.Id;
                        if (group.EffectiveUserId == 0)
                            group.EffectiveUserId = existingEfUser.Id;
                    }
                }

                // smažeme staré skupiny
                if (existingEfUser.GroupEffectiveMembers.Any(w => !w.Deleted))
                    foreach (var group in existingEfUser.GroupEffectiveMembers.Where(w => !w.Deleted))
                    {
                        if (!efUser.GroupEffectiveMembers.Any(w => w.GroupId == group.GroupId && w.EffectiveUserId == group.EffectiveUserId))
                        {
                            group.Deleted = true;
                            group.UpdateDateUtc = DateTime.UtcNow;
                            group.UpdatedByCustomerId = currentUser.Id;
                        }
                    }

                #endregion

                #region Role

                //nové a stávající role
                foreach (var role in efUser.RoleMembers)
                {
                    var existingItem = existingEfUser.RoleMembers.Where(w => w.RoleId == role.RoleId && w.EffectiveUserId == existingEfUser.Id).FirstOrDefault();
                    if (existingItem == null)
                    {
                        role.CreatedByCustomerId = currentUser.Id;
                        role.CreatedDateUtc = DateTime.UtcNow;
                        role.Active = true;
                        role.Deleted = false;
                        role.EffectiveUserId = existingEfUser.Id;
                        existingEfUser.RoleMembers.Add(_mapper.Map<RoleMember>(role));
                        continue;
                    }

                    if (!existingItem.Active.GetValueOrDefault() || existingItem.Deleted)
                    {
                        existingItem.Active = true;
                        existingItem.Deleted = false;
                        existingItem.UpdateDateUtc = DateTime.UtcNow;
                        existingItem.UpdatedByCustomerId = currentUser.Id;
                        if (role.EffectiveUserId == 0)
                            role.EffectiveUserId = existingEfUser.Id;
                    }
                }

                // smažeme staré role
                if (existingEfUser.RoleMembers.Count > 0)
                    if (existingEfUser.RoleMembers.Any(w => !w.Deleted))
                        foreach (var role in existingEfUser.RoleMembers.Where(w => !w.Deleted))
                        {
                            if (!efUser.RoleMembers.Any(w => w.RoleId == role.RoleId && w.EffectiveUserId == role.EffectiveUserId))
                            {
                                role.Deleted = true;
                                role.UpdateDateUtc = DateTime.UtcNow;
                                role.UpdatedByCustomerId = currentUser.Id;
                            }
                        }

                #endregion
            }

            // ukládání Spráce Poskytovatele, odebereme EF uživatele, které nemáme nastavené
            if (isProviderAdmin)
            {
                List<int> providers = new List<int>();
                providers.AddRange(user.EffectiveUserUsers.Where(w => !w.Deleted).Select(s => s.ProviderId).ToList());

                // platí pouze při editaci Správcem Organizace
                if (!providerId.HasValue)
                {
                    // projdeme EF uživatele, které již nechceme
                    foreach (var efUser in actualData.EffectiveUserUsers.Where(w => !w.Deleted && !providers.Contains(w.ProviderId)))
                    {
                        efUser.Deleted = true;
                        efUser.UpdateDateUtc = DateTime.UtcNow;
                        efUser.UpdatedByCustomerId = currentUser.Id;
                    }
                }

                // kontrola na zaktivnění neaktivních Poskytovatelů
                var setProviders = await _providerRepository.GetProvidersTaskAsync();
                setProviders = setProviders.Where(w => providers.Contains(w.Id) && !w.Active.Value).ToList();
                if (setProviders.Count() > 0)
                    foreach (var provider in setProviders)
                    {
                        if (!provider.Active.GetValueOrDefault())
                        {
                            provider.Active = true;
                            provider.UpdateDateUtc = DateTime.UtcNow;
                            provider.UpdatedByCustomerId = currentUser.Id;

                            await _providerRepository.UpdateProviderTaskAsync(provider, null);
                        }
                    }
            }

            // přechod na EF uživatele, odebereme role přiřazené přímo uživateli (nyní jsou na EF)
            if (haveEFUser && actualData.RoleMemberDirectUsers.Count > 0 && actualData.RoleMemberDirectUsers.Any(a => !a.Deleted))
            {
                // smažeme staré role
                foreach (var role in actualData.RoleMemberDirectUsers.Where(w => !w.Deleted).ToList())
                {
                    role.Deleted = true;
                    role.UpdateDateUtc = DateTime.UtcNow;
                    role.UpdatedByCustomerId = currentUser.Id;
                }
            }

            // nemáme žádné EF uživatele
            if (user.EffectiveUserUsers.Count == 0 && actualData.EffectiveUserUsers.Count > 0 && actualData.EffectiveUserUsers.Any(w => !w.Deleted))
                // smažeme staré
                foreach (var item in actualData.EffectiveUserUsers.Where(w => !w.Deleted))
                {
                    item.Deleted = true;
                    item.UpdateDateUtc = DateTime.UtcNow;
                    item.UpdatedByCustomerId = currentUser.Id;

                    // smažeme staré skupiny
                    foreach (var group in item.GroupEffectiveMembers.Where(w => !w.Deleted))
                    {
                        group.Deleted = true;
                        group.UpdateDateUtc = DateTime.UtcNow;
                        group.UpdatedByCustomerId = currentUser.Id;
                    }

                    // smažeme staré role
                    foreach (var role in item.RoleMembers.Where(w => !w.Deleted))
                    {
                        role.Deleted = true;
                        role.UpdateDateUtc = DateTime.UtcNow;
                        role.UpdatedByCustomerId = currentUser.Id;
                    }
                }

            #region Role spojené přímo s uživatelem

            // nemáme EF uživatele ale máme Role
            if (!actualData.EffectiveUserUsers.Where(w => !w.Deleted).Any() && (actualData.RoleMemberDirectUsers.Where(w => !w.Deleted).Any() || user.RoleMemberDirectUsers.Any()))
            {
                //nové a stávající role
                foreach (var role in user.RoleMemberDirectUsers)
                {
                    var existingItem = actualData.RoleMemberDirectUsers.Where(w => w.RoleId == role.RoleId && w.DirectUserId == user.Id).FirstOrDefault();
                    if (existingItem == null)
                    {
                        role.CreatedByCustomerId = currentUser.Id;
                        role.CreatedDateUtc = DateTime.UtcNow;
                        role.Active = true;
                        role.Deleted = false;
                        role.DirectUserId = user.Id;
                        actualData.RoleMemberDirectUsers.Add(_mapper.Map<RoleMember>(role));
                        continue;
                    }

                    if (!existingItem.Active.GetValueOrDefault() || existingItem.Deleted)
                    {
                        existingItem.Active = true;
                        existingItem.Deleted = false;
                        existingItem.UpdateDateUtc = DateTime.UtcNow;
                        existingItem.UpdatedByCustomerId = currentUser.Id;
                    }
                }

                // smažeme staré role
                if (actualData.RoleMemberDirectUsers.Any(w => !w.Deleted))
                    foreach (var role in actualData.RoleMemberDirectUsers.Where(w => !w.Deleted).ToList())
                    {
                        if (!user.RoleMemberDirectUsers.Any(w => w.RoleId == role.RoleId && w.DirectUserId == role.DirectUserId))
                        {
                            role.Deleted = true;
                            role.UpdateDateUtc = DateTime.UtcNow;
                            role.UpdatedByCustomerId = currentUser.Id;
                        }
                    }
            }

            #endregion

            #region Zakázaná oprávnění pro položky rolí

            //nové a stávající zakázaná oprávnění pro položky Rolí
            foreach (var permission in user.UserPermissionUsers)
            {
                // dohledáme existující oprávnění
                var existingPermission = actualData.UserPermissionUsers.Where(w => w.PermissionId == permission.PermissionId).FirstOrDefault();
                if (existingPermission != null)
                {
                    existingPermission.UpdatedByCustomerId = currentUser.Id;
                    existingPermission.UpdateDateUtc = DateTime.UtcNow;
                    existingPermission.Deleted = false;
                    existingPermission.Active = true;
                    existingPermission.IsDeniedPermission = true;
                }
                else
                {
                    // nové oprávnění
                    permission.CreatedByCustomerId = currentUser.Id;
                    permission.CreatedDateUtc = DateTime.UtcNow;
                    permission.Deleted = false;
                    permission.Active = true;
                    permission.IsDeniedPermission = true;
                    actualData.UserPermissionUsers.Add(_mapper.Map<UserPermission>(permission));
                }
            }

            // odebraná oprávnění označíme jako smazaná
            if (actualData.UserPermissionUsers.Any(w => !w.Deleted))
                foreach (var permission in actualData.UserPermissionUsers.Where(w => !w.Deleted).ToList())
                {
                    if (user.UserPermissionUsers.Any(a => a.PermissionId == permission.PermissionId))
                        continue;

                    permission.Deleted = true;
                    permission.UpdatedByCustomerId = currentUser.Id;
                    permission.UpdateDateUtc = DateTime.UtcNow;
                }

            #endregion

            bool ret = await _customerRepository.UpdateCustomerTaskAsync(currentUser, actualData, ignoreADB2C: null, tran);
            TimeSpan timeEnd = DateTime.Now - startTime;
            if (ret)
            {
                _logger.LogInformation("{0} User '{1}', Email: '{2}', Id: {3} updated succesfully, duration: {4}", logHeader, actualData.FriendlyName, actualData.Email, actualData.Id, timeEnd);
                return new GenericResponse<bool>(true, ret, 0);
            }
            else
            {
                _logger.LogWarning("{0} User was not updated, Name: {1}, ID: {2}, Email: {3}, duration: {4}", logHeader, user.FriendlyName, user.Id, user.Email, timeEnd);
                return new GenericResponse<bool>(false, false, -1, "User was not updated", "Error when updating user.");
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
    /// Uloží nového uživatele
    /// </summary>
    /// <param name="user">Objekt nového uživatele</param>
    /// <returns>GenericResponse s parametrem "success" TRUE a objektem "CompleteUserContract" nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné UserId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -6 = uživatele se nepodařilo založit v DB nebo ADB2C
    /// -7 = chyby při konverzi dat uživatele
    /// -11 = uživatel se stejným emailem existuje
    /// -12 = uživatel se stejným tel. číslem existuje
    /// -13 = uživatel se stejným PersonalIdentificationNumber existuje
    /// -14 = uživatel se stejným GlobalID existuje
    /// </returns>
    [Authorize]
    [HttpPost(Name = "InsertUser")]
    public async Task<IGenericResponse<CompleteUserContract>> InsertUser(CompleteUserContract user)
    {
        string logHeader = _logName + ".InsertUser:";
        bool ret = false;
        bool isProviderAdmin = false;
        DateTime startTime = DateTime.Now;

        try
        {
            // kontrola na vstupní data
            if (user.Id > 0)
            {
                _logger.LogWarning("{0} Invalid UserId: {1}", logHeader, user.Id);
                return new GenericResponse<CompleteUserContract>(null, false,-2, "Invalid UserId", "UserId value is not '0'.");
            }

            // dotáhneme si aktuálního uživatele
            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                _logger.LogWarning("{0} Current User not found", logHeader);
                return new GenericResponse<CompleteUserContract>(null, false,-4, "Current user not found", "User not found by GlobalId.");
            }

            Customer? actualData = _mapper.Map<Customer>(user);

            if (actualData == null)
            {
                _logger.LogWarning("{0} Cannot convert CompleteUserContract to Customer", logHeader);
                return new GenericResponse<CompleteUserContract>(null, false,-7, "Invalid user data", "User data cannot be converted to DB model.");
            }

            int checkRet = await _customerRepository.CheckIfUserExists(currentUser, actualData);
            if (checkRet < 0)
            {
                _logger.LogWarning("{0} User already exists", logHeader);
                return new GenericResponse<CompleteUserContract>(null, false,checkRet, "User already exists", "User with same data already exists in DB.");
            }

            actualData.CreatedByCustomerId = currentUser.Id;
            actualData.CreatedDateUtc = DateTime.UtcNow;
            actualData.PasswordFormatTypeId = 1;

            if (actualData.HealthCareInsurerId.HasValue && actualData.HealthCareInsurerId == 0)
                actualData.HealthCareInsurerId = null;

            if (actualData.Picture != null)
            {
                actualData.Picture.CreatedDateUtc = DateTime.UtcNow;
                actualData.Picture.CreatedByCustomerId = currentUser.Id;
            }

            // efektivní uživatel
            foreach (var item in actualData.EffectiveUserUsers)
            {
                item.Deleted = false;
                item.CreatedByCustomerId = currentUser.Id;
                item.CreatedDateUtc = DateTime.UtcNow;
                actualData.CreatedByProviderId = item.ProviderId;

                // skupiny
                foreach (var group in item.GroupEffectiveMembers)
                {
                    group.Active = true;
                    group.Deleted = false;
                    group.CreatedByCustomerId = currentUser.Id;
                    group.CreatedDateUtc = DateTime.UtcNow;
                }

                // role
                foreach (var role in item.RoleMembers)
                {
                    role.Active = true;
                    role.Deleted = false;
                    role.CreatedByCustomerId = currentUser.Id;
                    role.CreatedDateUtc = DateTime.UtcNow;
                    // přiřazená role je role Správce Poskytovatele?
                    if (!isProviderAdmin && ((role.RoleId == (int)RoleMainEnum.ProviderAdmin) || (role.Role != null && role.Role.ParentRoleId == (int)RoleMainEnum.ProviderAdmin)))
                        isProviderAdmin = true;
                }
            }

            // ukládání Spráce Poskytovatele
            if (isProviderAdmin)
            {
                List<int> providers = new List<int>();
                providers.AddRange(actualData.EffectiveUserUsers.Select(s => s.ProviderId).ToList());

                // kontrola na zaktivnění neaktivních Poskytovatelů
                var setProviders = await _providerRepository.GetProvidersTaskAsync();
                setProviders = setProviders.Where(w => providers.Contains(w.Id) && !w.Active.Value).ToList();
                if (setProviders.Count() > 0)
                    foreach (var provider in setProviders)
                    {
                        if (!provider.Active.GetValueOrDefault())
                        {
                            provider.Active = true;
                            provider.UpdateDateUtc = DateTime.UtcNow;
                            provider.UpdatedByCustomerId = currentUser.Id;
                            await _providerRepository.UpdateProviderTaskAsync(provider, null);
                        }
                    }
            }

            // role spojené přímo s uživatelem
            foreach (var role in actualData.RoleMemberDirectUsers)
            {
                role.Active = true;
                role.Deleted = false;
                role.CreatedByCustomerId = currentUser.Id;
                role.CreatedDateUtc = DateTime.UtcNow;
            }

            // zakázané oprávnění pro položky Rolí
            foreach (var permission in actualData.UserPermissionUsers)
            {
                // nový záznam
                permission.CreatedByCustomerId = currentUser.Id;
                permission.CreatedDateUtc = DateTime.UtcNow;
                permission.Deleted = false;
                permission.Active = true;
                permission.IsDeniedPermission = true;
            }

            // GlobalId nastavujeme v repository
            //actualData.GlobalId = actualData.Email;

            if (actualData.HealthCareInsurerId.HasValue && user.HealthCareInsurerId.GetValueOrDefault() <= 0)
                actualData.HealthCareInsurerId = null;

            // pokud nemáme heslo, tak ho vygenerujeme
            if (string.IsNullOrEmpty(actualData.Password))
                actualData.Password = PasswordGenerator.GetNewPassword();

            ret = await _customerRepository.InsertCustomerTaskAsync(currentUser, actualData);

            TimeSpan timeEnd = DateTime.Now - startTime;
            if (ret)
            {
                user = _mapper.Map<CompleteUserContract>(actualData);
                _logger.LogInformation("{0} User '{1}', Email: '{2}', Id: {3} created succesfully, duration: {4}", logHeader, actualData.FriendlyName, actualData.Email, actualData.Id, timeEnd);
                return new GenericResponse<CompleteUserContract>(user, true, user.Id);
            }
            else
            {
                _logger.LogWarning("{0} User was not created, Name: {1} ID: {2} Email: {3}, duration: {4}", logHeader, user.FriendlyName, user.Id, user.Email, timeEnd);
                return new GenericResponse<CompleteUserContract>(null, false,-6, "User was not created", "Error when inserting new user into DB or ADB2C.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<CompleteUserContract>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Označí existujícího uživatelejako smazaného a smaže ho z ADB2C
    /// </summary>
    /// <param name="userId">ID uživatele</param>
    /// <param name="providerId">ID Poskytovatele pod kterým uživatele mažeme</param>
    /// <returns>GenericResponse s parametrem "success" TRUE nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné UserId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = mazaný uživatel nebyl nalezen
    /// -8 = uživatele se nepodařilo smazat v DB nebo ADB2C
    /// -20 = uživatel je System account a toho nelze smazat
    /// -21 = uživatel nemůže smazat sebe
    /// -22 = nemůžeme smazat posledního uživatele v roli Správce Poskytovatele (u Poskytovatele)
    /// </returns>
    [Authorize]
    [HttpGet(Name = "DeleteUser")]
    public async Task<IGenericResponse<bool>> DeleteUser(int userId, int? providerId)
    {
        string logHeader = _logName + ".DeleteUser:";
        bool ret = false;
        bool deleteCustomer = false;
        DateTime startTime = DateTime.Now;
        try
        {
            // kontrola na vstupní data
            if (userId <= 0)
            {
                _logger.LogWarning("{0} Invalid UserId: {1}", logHeader, userId);
                return new GenericResponse<bool>(ret, false,-2, "Invalid UserId", "UserId value must be greater then '0'.");
            }

            // dotáhneme si aktuálního uživatele
            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                _logger.LogWarning("{0} Current User not found", logHeader);
                return new GenericResponse<bool>(ret, false,-4, "Current user not found", "Current user not found by GlobalId.");
            }

            // dotáhneme si uživatele
            Customer? customer = await _customerRepository.GetCustomerByIdTaskAsync(userId);
            if (customer == null)
            {
                _logger.LogWarning("{0} User not found, Id: {1}", logHeader, userId);
                return new GenericResponse<bool>(ret, false,-5, "User not found", "User not found by Id.");
            }

            if (customer.IsSystemAccount)
            {
                _logger.LogWarning("{0} User is System account, Name: {1}, ID: {2}, Email: {3}", logHeader, customer.FriendlyName, userId, customer.Email);
                return new GenericResponse<bool>(ret, false,-20, "User is System account", "Can not delete user with System account.");
            }

            if (currentUser.Id == customer.Id)
            {
                _logger.LogWarning("{0} User cannot delete himself, Name: {1}, ID: {2}, Email: {3}", logHeader, customer.FriendlyName, userId, customer.Email);
                return new GenericResponse<bool>(ret, false,-21, "User cannot delete himself", "User cannot delete himself.");
            }

            // maže Správce Organizace Správce Poskytovatele?
            if (currentUser.IsOrganizationAdmin() && customer.IsProviderAdmin())
                providerId = null;

            // kontrola odebrání posledního Správce Poskytovatele
            if (customer.IsProviderAdmin())
            {
                DateTime prStart = DateTime.Now;
                // musíme projít všechny nesmazené EF uživatele v roli Správce posyktovatele = všechny Poskytovatele, které má přiřazené
                List<int> providers = customer.EffectiveUserUsers.Where(w => !w.Deleted && w.RoleMembers.Any(r => r.RoleId == (int)RoleMainEnum.ProviderAdmin && r.Active.Value && !r.Deleted)).Select(s => s.ProviderId).ToList();

                foreach (int i in providers)
                {
                    // kontrolujeme každého poskytovatele
                    Provider? provider = await _providerRepository.GetProviderByIdTaskAsync(i);

                    if (provider != null)
                    {
                        int otherAdminsCount = provider.EffectiveUsers.Count(w => w.UserId != customer.Id
                                                                              && !w.Deleted
                                                                              && w.Active.Value
                                                                              && w.RoleMembers.Any(r => r.RoleId == (int)RoleMainEnum.ProviderAdmin && r.Active.Value && !r.Deleted));

                        // odebral bych posledního Správce Poskytovatele
                        if (otherAdminsCount == 0)
                        {
                            _logger.LogWarning("{0} Cannot delete last Provider Admin, Name: {1}, ID: {2}, Email: {3}", logHeader, customer.FriendlyName, userId, customer.Email);
                            return new GenericResponse<bool>(ret, false,-22, "Cannot delete last Provider Admin", "Cannot delete last Provider Admin user in Provider.");
                        }
                    }
                }
            }

            // pokud jde pouze o smazání efektivního uživatele a existuje ještě jiný aktivní efektivní uživatel pro danou entitu Customer,
            // je potřeba ponechat záznam Customer nesmazaný
            if (customer.EffectiveUserUsers.Any() && providerId.HasValue)
            {
                // všichni EF daného Customera
                var existingEfUsers = await _effectiveUserRepository.GetEffectiveUsersTaskAsync(customer.Id);

                // nesmazaný EF aktuálního Poskytovatele
                var editedEfUser = customer.EffectiveUserUsers.FirstOrDefault(u => !u.Deleted && u.ProviderId == providerId.Value);

                if (editedEfUser != null)
                {
                    // mažeme EF uživatele
                    var existingEfUser = existingEfUsers.First(x => x.Id == editedEfUser.Id);
                    await _effectiveUserRepository.DeleteEffectiveUserTaskAsync(currentUser, existingEfUser);
                    // pokud Customer nemá další EF uživatele u tohoto Poskytovatele, smažu ho
                    if (!existingEfUsers.Any(x => x.Id != editedEfUser.Id && x.Active.Value))
                    {
                        deleteCustomer = true;
                    }
                }
                else
                {
                    // aktuální EF je smazaný
                    editedEfUser = customer.EffectiveUserUsers.FirstOrDefault(u => u.ProviderId == providerId.Value);
                    // aktuální EF Poskytovatele je smazaný a neexistují jiní aktivní EF uživatelé Poskytovatele
                    if (editedEfUser != null && !existingEfUsers.Any(x => x.Id != editedEfUser.Id && x.Active.Value))
                    {
                        deleteCustomer = true;
                    }
                }
            }
            else
            {
                deleteCustomer = true;
            }

            if (deleteCustomer)
                ret = await _customerRepository.DeleteCustomerTaskAsync(currentUser, customer);

            TimeSpan timeEnd = DateTime.Now - startTime;

            if (ret)
            {
                _logger.LogInformation("{0} User '{1}', Email: '{2}', Id: {3} deleted succesfully, duration: {4}", logHeader, customer.FriendlyName, customer.Email, customer.Id, timeEnd);
                return new GenericResponse<bool>(ret, true, 0);
            }
            else
            {
                _logger.LogWarning("{0} User was not deleted, Name: {1}, ID: {2}, Email: {3}, duration: {4}", logHeader, customer.FriendlyName, customer.Id, customer.Email, timeEnd);
                return new GenericResponse<bool>(ret, false,-8, "User was not deleted", "Error when deleting user.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<bool>(false, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Metoda kontroluje, zda existuje uživatel v ADB2C
    /// </summary>
    /// <param name="userId">ID uživatele</param>
    /// <returns>GenericResponse s parametrem "INT" 1 (existuje) nebo 0 (neexistuje) a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné UserId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = mazaný uživatel nebyl nalezen
    /// </returns>
    [Authorize]
    [HttpGet(Name = "CheckUserInADB2C")]
    public async Task<IGenericResponse<int>> CheckUserInADB2C(int userId)
    {
        string logHeader = _logName + ".CheckUserInADB2C:";
        int ret = -1;

        DateTime startTime = DateTime.Now;
        try
        {
            // kontrola na vstupní data
            if (userId <= 0)
            {
                _logger.LogWarning("{0} Invalid UserId: {1}", logHeader, userId);
                return new GenericResponse<int>(ret, false, -2, "Invalid UserId", "UserId value must be greater then '0'.");
            }

            // dotáhneme si aktuálního uživatele
            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                _logger.LogWarning("{0} Current User not found", logHeader);
                return new GenericResponse<int>(ret, false, -4, "Current user not found", "Current user not found by GlobalId.");
            }

            // dotáhneme si uživatele
            Customer? customer = await _customerRepository.GetCustomerInfoByIdTaskAsync(userId);

            if (customer == null)
            {
                _logger.LogWarning("{0} User not found, Id: {1}", logHeader, userId);
                return new GenericResponse<int>(ret, false, -5, "User not found", "User not found by Id.");
            }

            ret = await _adb2cRepository.CheckUserAsyncTask(customer);

            TimeSpan timeEnd = DateTime.Now - startTime;
            _logger.LogInformation("{0} User '{1}', Email: '{2}', Id: {3}, returned: {4}, duration: {5}", logHeader, customer.FriendlyName, customer.Email, customer.Id, ret, timeEnd);

            return new GenericResponse<int>(ret, true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<int>(ret, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Metoda založí existujícího uživatele z DB i v ADB2C
    /// </summary>
    /// <param name="userId">ID uživatele</param>
    /// <returns>GenericResponse s parametrem "INT" 1 (založen) nebo 0 (nezaložen) a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné UserId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = uživatel nebyl nalezen v DB
    /// -6 = uživatele se nepodařilo založit v ADB2C nebo aktualizovat DB
    /// </returns>
    [Authorize]
    [HttpGet(Name = "InsertUserInADB2C")]
    public async Task<IGenericResponse<int>> InsertUserInADB2C(int userId)
    {
        string logHeader = _logName + ".InsertUserInADB2C:";
        int ret = -1;

        DateTime startTime = DateTime.Now;
        try
        {
            // kontrola na vstupní data
            if (userId <= 0)
            {
                _logger.LogWarning("{0} Invalid UserId: {1}", logHeader, userId);
                return new GenericResponse<int>(ret, false, -2, "Invalid UserId", "UserId value must be greater then '0'.");
            }

            // dotáhneme si aktuálního uživatele
            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                _logger.LogWarning("{0} Current User not found", logHeader);
                return new GenericResponse<int>(ret, false, -4, "Current user not found", "Current user not found by GlobalId.");
            }

            // dotáhneme si uživatele
            Customer? customer = await _customerRepository.GetCustomerByIdTaskAsync(userId);

            if (customer == null)
            {
                _logger.LogWarning("{0} User not found, Id: {1}", logHeader, userId);
                return new GenericResponse<int>(ret, false, -5, "User not found", "User not found by Id.");
            }

            // nastavíme GlobalId jako UPN
            customer.GlobalId = _adb2cRepository.CreateUPN(customer.Email);

            // vygenerujeme heslo
            customer.Password = PasswordGenerator.GetNewPassword();

            // založíme uživatele v ADB2C
            bool created = await _adb2cRepository.InsertUserAsyncTask(customer);

            TimeSpan timeEnd = DateTime.Now - startTime;

            if (created)
            {
                // aktualizujeme uživatele v DB
                created = await _customerRepository.UpdateCustomerTaskAsync(currentUser, customer, true);
            }
            else
            {
                ret = 0;
                _logger.LogWarning("{0} User was not created, Name: {1} ID: {2} Email: {3}, duration: {4}", logHeader, customer.FriendlyName, customer.Id, customer.Email, timeEnd);
                return new GenericResponse<int>(ret, false, -6, "User was not created", "Error when inserting new user into ADB2C.");
            }

            timeEnd = DateTime.Now - startTime;

            if (created)
            {
                ret = 1;
                _logger.LogInformation("{0} User '{1}', Email: '{2}', Id: {3} created succesfully, duration: {4}", logHeader, customer.FriendlyName, customer.Email, customer.Id, timeEnd);
                return new GenericResponse<int>(ret, true, 0);
            }
            else
            {
                ret = 0;
                _logger.LogWarning("{0} User was created in ADB2C but not updated in DB, Name: {1} ID: {2} Email: {3}, duration: {4}", logHeader, customer.FriendlyName, customer.Id, customer.Email, timeEnd);
                return new GenericResponse<int>(ret, false, -6, "User was not created", "Error when updating user in DB.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<int>(ret, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Uloží Firebase Cloud Messaging token uživatele
    /// </summary>
    /// <param name="globalId">GlobalId uživatele</param>
    /// <param name="appInstanceToken"></param>
    /// <returns>GenericResponse s parametrem "success" TRUE nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné GlobalId nebo appInstanceToken
    /// -3 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -4 = nepodařilo se uložit data
    /// </returns>
    [Authorize]
    [HttpGet(Name = "CreateOrUpdateAppInstanceToken")]
    public async Task<IGenericResponse<bool>> CreateOrUpdateAppInstanceToken(string appInstanceToken)
    {

        string logHeader = _logName + ".CreateOrUpdateAppInstanceToken:";

        if (string.IsNullOrEmpty(appInstanceToken))
        {
            _logger.LogWarning($"{logHeader} Invalid appInstanceToken, appInstanceToken: {appInstanceToken}");
            return new GenericResponse<bool>(false, false, -2, "Invalid appInstanceToken", "");
        }

        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<bool>(false, false, -4, "Current user not found", "User not found");
        }

        try
        {
            Customer? user = await _customerRepository.GetCustomerByGlobalIdTaskAsync(currentUser.GlobalId);
            if (user == null)
            {
                _logger.LogWarning($"{logHeader} Current User not found, globalID: {currentUser.GlobalId}");
                return new GenericResponse<bool>(false, false, -3, "Current User not found", "");
            }

            user.AppInstanceToken = appInstanceToken;

            CompleteUserContract actualUser = new CompleteUserContract();
            actualUser.Id = user.Id;

            bool updated = await _customerRepository.UpdateCustomerTaskAsync(actualUser, user, true);

            if (updated)
            {
                _logger.LogInformation($"{logHeader} AppInstanceToken has been updated successfully for UserId: {user.Id}");
                return new GenericResponse<bool>(updated, true, 0);
            }
            else
            {
                _logger.LogWarning($"{logHeader} AppInstanceToken update has failed for UserId: {user.Id}");
                return new GenericResponse<bool>(updated, true, -4, "AppInstanceToken update has failed", "");
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<bool>(false, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Uloží žádost o smazání dat uživatele do dedikované databáze.
    /// <param name="globalId">globalID uživatele co metodu volá</param>
    /// <returns>GenericResponse s parametrem "success" TRUE nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné GlobalId
    /// -3 = záznam se nepodařilo uložit
    /// </returns>
    [HttpPost(Name = "SaveUserAccountDeletionDemand")]
    public async Task<IGenericResponse<bool>> SaveUserAccountDeletionDemand(string globalId)
    {
        string logHeader = _logName + ".SaveUserAccountDeletionDemand:";

        if (string.IsNullOrEmpty(globalId))
        {
            _logger.LogWarning($"{logHeader} Invalid globalId, globalID: {globalId}");
            return new GenericResponse<bool>(false, false, -2, "Invalid globalId", "");
        }

        try
        {
            //najdeme userId podle globalId
            var user = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
            var mappedUser = new CompleteUserContract();
            mappedUser = _mapper.Map<CompleteUserContract>(user);

            var userId = new SqlParameter
            {
                ParameterName = "@userId",
                Value = mappedUser.Id,
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input
            };

            var returnValue = new SqlParameter
            {
                ParameterName = "@returnValue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            object[] parameters = new object[2];
            parameters[0] = userId;
            parameters[1] = returnValue;


            //zavoláme stored procedure v dedikované db a vezmeme si výstupní parametr
            int affectedRaws = await _dbContext.Database.ExecuteSqlRawAsync($"dbo.sp_SaveUserDataDeletionDemand @userId, @returnValue OUTPUT", parameters);

            if (returnValue.Value == null || returnValue.Value.ToString() != "1")
            {
                _logger.LogWarning($"{logHeader} Stored procedure dbo.sp_SaveUserDataDeletionDemand has failed., globalID: {globalId}");
                return new GenericResponse<bool>(false, false, -3, "Stored procedure dbo.sp_SaveUserDataDeletionDemand has failed.", "");
            }

            return new GenericResponse<bool>(true, true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<bool>(false, false, -1, ex.Message);
        }
    }
}


