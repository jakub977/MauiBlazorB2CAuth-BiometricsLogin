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
using Castle.Core.Resource;
using Microsoft.Graph.Models;
using Principal.Telemedicine.DataConnectors.Extensions;
using Principal.Telemedicine.Shared.Enums;
using System.Text.Json.Serialization;
using Principal.Telemedicine.Shared.Api;

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
    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    private readonly string _logName = "UserApiController";

    public UserApiController(ICustomerRepository customerRepository, IProviderRepository providerRepository, IEffectiveUserRepository effectiveUserRepository,
        DbContextApi dbContext, ILogger<UserApiController> logger, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _providerRepository = providerRepository;
        _effectiveUserRepository = effectiveUserRepository;
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Vrátí základní údaje uživatele.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetUserInfo")]
    public async Task<IGenericResponse<UserContract>> GetUserInfo([FromHeader(Name = "x-api-g")] string globalId, int userId)
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
    public async Task<IGenericResponse<CompleteUserContract>> GetUser([FromHeader(Name = "x-api-g")] string globalId, int? userId)
    {

        if (userId <= 0 || string.IsNullOrEmpty(globalId))
        {
            return BadRequest();
        }

        try
        {
            var mappedUser = new CompleteUserContract();
            int id = userId.HasValue ? userId.Value : 0;

            if (id <= 0)
            {
                var user = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
                mappedUser = _mapper.Map<CompleteUserContract>(user);
            }
            else
            {
                // todo: zjistit jestli má uživatel oprávnění read na jiného uživatele
                // možno nastudovat v Vanda -> SmartMVC.Services -> Customers -> CustomerService.cs, metoda GetAllCustomers na ř. 161
                var user = await _customerRepository.GetCustomerByIdTaskAsync(id);
                mappedUser = _mapper.Map<CompleteUserContract>(user);
            }

            return Ok(mappedUser);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Vrátí seznam uživatelů.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetUsers([FromHeader(Name = "x-api-g")] string globalId, int? userId)
    {

        if (userId <= 0 || string.IsNullOrEmpty(globalId))
        {
            return BadRequest();
        }

        try
        {
            var mappedUser = new CompleteUserContract();
            int id = userId.HasValue ? userId.Value : 0;

            if (id <= 0)
            {
                var user = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
                mappedUser = _mapper.Map<CompleteUserContract>(user);
            }
            else
            {
                // todo: zjistit jestli má uživatel oprávnění read na jiného uživatele
                // možno nastudovat v Vanda -> SmartMVC.Services -> Customers -> CustomerService.cs, metoda GetAllCustomers na ř. 161
                var user = await _customerRepository.GetCustomerByIdTaskAsync(id);
                mappedUser = _mapper.Map<CompleteUserContract>(user);
            }

            return Ok(mappedUser);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Uloží změny uživatele
    /// </summary>
    /// <param name="globalId">globalID uživatele co metodu volá</param>
    /// <param name="user">Objekt uživatele</param>
    /// <returns>Objekt uživatele nebo chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné UserId
    /// -3 = chybí GlobalId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = uživatele se nepodařilo dohledat podle ID
    /// -11 = uživatel se stejným emailem existuje
    /// -12 = uživatel se stejným tel. číslem existuje
    /// -13 = uživatel se stejným PersonalIdentificationNumber existuje
    /// -14 = uživatel se stejným GlobalID existuje
    /// </returns>
    [HttpPost(Name = "UpdateUser")]
    public async Task<IGenericResponse<bool>> UpdateUser([FromHeader(Name = "x-api-g")] string globalId, CompleteUserContract user, int? providerId = null, bool isProviderAdmin = false)
    {
        string logHeader = _logName + ".UpdateUser:";
        try
        {
            // kontrola na vstupní data
            if (user.Id <= 0 || string.IsNullOrEmpty(globalId))
            {
                _logger.LogWarning("{0} Invalid UserId: {1} or GlobalId: {2}", logHeader, user.Id, globalId);
                if (user.Id > 0)
                    return BadRequest(APIErrorResponseModel.APIErrorResponse(-2, "Invalid UserId", "UserId vali is not '0'."));
                else
                    return BadRequest(APIErrorResponseModel.APIErrorResponse(-3, "GlobalId is empty", "GlobalId must be set."));
            }

            Customer? currentUser = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
            if (currentUser == null)
            {
                _logger.LogWarning("{0} Current User not found", logHeader);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-4, "Current user not found", "User not found by GlobalId."));
            }

            Customer? actualData = await _customerRepository.GetCustomerByIdTaskAsync(user.Id);
            if (actualData == null)
            {
                _logger.LogWarning("{0} User not found, Name: {1}, ID: {2}, Email: {3}", logHeader, user.FriendlyName, user.Id, user.Email);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-5, "User not found", "User not found by Id."));
            }

            int checkRet = await _customerRepository.CheckIfUserExists(currentUser, actualData);
            if (checkRet < 0)
            {
                _logger.LogWarning("{0} User already exists", logHeader);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(checkRet, "User already exists", "User with same data already exists in DB."));
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

                if (!user.Active && existingEfUsers.Any(x => x.Active.GetValueOrDefault()))
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
            actualData.HealthCareInsurerId = user.HealthCareInsurerId;
            actualData.IsSystemAccount = user.IsSystemAccount;
            actualData.LastName = user.LastName;
            actualData.TitleBefore = user.TitleBefore;
            actualData.TitleAfter = user.TitleAfter;
            actualData.OrganizationId = user.OrganizationId;
            actualData.PersonalIdentificationNumber = user.PersonalIdentificationNumber;
            actualData.BirthIdentificationNumber = user.BirthIdentificationNumber;

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
                setProviders = setProviders.Where(w => providers.Contains(w.Id) && !w.Active.GetValueOrDefault()).ToList();
                if (setProviders.Count() > 0)
                    foreach (var provider in setProviders)
                    {
                        if (!provider.Active.GetValueOrDefault())
                        {
                            provider.Active = true;
                            provider.UpdateDateUtc = DateTime.UtcNow;
                            provider.UpdatedByCustomerId = currentUser.Id;

                            await _providerRepository.UpdateProviderTaskAsync(provider);
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

            bool ret = await _customerRepository.UpdateCustomerTaskAsync(currentUser, actualData);

            if (ret)
            {
                _logger.LogInformation("{0} User '{1}', Email: '{2}', Id: {3} updated succesfully", logHeader, actualData.FriendlyName, actualData.Email, actualData.Id);
                return Ok(user);
            }
            else
            {
                _logger.LogWarning("{0} User was not updated, Name: {1}, ID: {2}, Email: {3}", logHeader, user.FriendlyName, user.Id, user.Email);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-1, "User was not updated", "Error when updating user."));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Uloží nového uživatele
    /// </summary>
    /// <param name="globalId">GlobalID uživatele co metodu volá</param>
    /// <param name="user">Objekt nového uživatele</param>
    /// <returns>Objekt uživatele nebo chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné UserId
    /// -3 = chybí GlobalId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -6 = uživatele se nepodařilo založit v DB nebo ADB2C
    /// -7 = chyby při konverzi dat uživatele
    /// -11 = uživatel se stejným emailem existuje
    /// -12 = uživatel se stejným tel. číslem existuje
    /// -13 = uživatel se stejným PersonalIdentificationNumber existuje
    /// -14 = uživatel se stejným GlobalID existuje
    /// </returns>
    [HttpPost(Name = "InsertUser")]
    public async Task<IGenericResponse<CompleteUserContract>> InsertUser([FromHeader(Name = "x-api-g")] string globalId, CompleteUserContract user)
    {
        string logHeader = _logName + ".InsertUser:";
        bool ret = false;
        bool isProviderAdmin = false;
        DateTime startTime = DateTime.Now;
        try
        {
            _logger.LogDebug("{0} START", logHeader);

            // kontrola na vstupní data
            if (user.Id > 0 || string.IsNullOrEmpty(globalId))
            {
                _logger.LogWarning("{0} Invalid UserId: {1} or GlobalId: {2}", logHeader, user.Id, globalId);
                if (user.Id > 0)
                    return BadRequest(APIErrorResponseModel.APIErrorResponse(-2, "Invalid UserId", "UserId vali is not '0'."));
                else
                    return BadRequest(APIErrorResponseModel.APIErrorResponse(-3, "GlobalId is empty", "GlobalId must be set."));
            }

            // dotáhneme si aktuálního uživatele
            Customer? currentUser = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
            if (currentUser == null)
            {
                _logger.LogWarning("{0} Current User not found", logHeader);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-4, "Current user not found", "User not found by GlobalId."));
            }

            Customer? actualData = _mapper.Map<Customer>(user);

            if (actualData == null)
            {
                _logger.LogWarning("{0} Cannot convert CompleteUserContract to Customer", logHeader);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-7, "Invalid user data", "User data cannot be converted to DB model."));
            }

            int checkRet = await _customerRepository.CheckIfUserExists(currentUser, actualData);
            if (checkRet < 0)
            {
                _logger.LogWarning("{0} User already exists", logHeader);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(checkRet, "User already exists", "User with same data already exists in DB."));
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
                setProviders = setProviders.Where(w => providers.Contains(w.Id) && !w.Active.GetValueOrDefault()).ToList();
                if (setProviders.Count() > 0)
                    foreach (var provider in setProviders)
                    {
                        if (!provider.Active.GetValueOrDefault())
                        {
                            provider.Active = true;
                            provider.UpdateDateUtc = DateTime.UtcNow;
                            provider.UpdatedByCustomerId = currentUser.Id;
                            await _providerRepository.UpdateProviderTaskAsync(provider);
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

            actualData.GlobalId = actualData.Email;

            // pokud nemáme heslo, tak ho vygenerujeme
            if (string.IsNullOrEmpty(actualData.Password))
                actualData.Password = PasswordGenerator.GetNewPassword();

            ret = await _customerRepository.InsertCustomerTaskAsync(currentUser, actualData);

            TimeSpan timeEnd = DateTime.Now - startTime;
            if (ret)
            {
                user = _mapper.Map<CompleteUserContract>(actualData);
                _logger.LogInformation("{0} User '{1}', Email: '{2}', Id: {3} created succesfully, duration: {4}", logHeader, actualData.FriendlyName, actualData.Email, actualData.Id, timeEnd);
                return Ok(user);
            }
            else
            {
                _logger.LogWarning("{0} User was not created, Name: {1} ID: {2} Email: {3}, duration: {4}", logHeader, user.FriendlyName, user.Id, user.Email, timeEnd);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-6, "User was not created", "Error when inserting new user into DB or ADB2C."));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Označí existujícího uživatelejako smazaného a smaže ho z ADB2C
    /// </summary>
    /// <param name="globalId">GlobalID uživatele co metodu volá</param>
    /// <param name="userID">ID uživatele</param>
    /// <param name="providerId">ID Poskytovatele pod kterým uživatele mažeme</param>
    /// <returns>HTTP 200 nebo HTTP 500 nebo HTTP 400 a chybu:
    /// -2 = neplatné UserId
    /// -3 = chybí GlobalId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = mazaný uživatel nebyl nalezen
    /// -8 = uživatele se nepodařilo smazat v DB nebo ADB2C
    /// -20 = uživatel je System account a toho nelze smazat
    /// -21 = uživatel nemůže smazat sebe
    /// -22 = nemůžeme smazat posledního uživatele v roli Správce Poskytovatele (u Poskytovatele)
    /// </returns>
    [HttpGet(Name = "DeleteUser")]
    public async Task<IGenericResponse<bool>> DeleteUser([FromHeader(Name = "x-api-g")] string globalId, int userId, int? providerId)
    {
        string logHeader = _logName + ".DeleteUser:";
        bool ret = false;
        bool deleteCustomer = false;
        DateTime startTime = DateTime.Now;
        try
        {
            _logger.LogDebug("{0} START", logHeader);

            // kontrola na vstupní data
            if (userId <= 0 || string.IsNullOrEmpty(globalId))
            {
                _logger.LogWarning("{0} Invalid UserId: {1} or GlobalId: {2}", logHeader, userId, globalId);
                if (userId <= 0)
                    return BadRequest(APIErrorResponseModel.APIErrorResponse(-2, "Invalid UserId", "UserId value must be greater then '0'."));
                else
                    return BadRequest(APIErrorResponseModel.APIErrorResponse(-3, "GlobalId is empty", "GlobalId must be set."));
            }

            // dotáhneme si aktuálního uživatele
            Customer? currentUser = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
            if (currentUser == null)
            {
                _logger.LogWarning("{0} Current User not found", logHeader);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-4, "Current user not found", "Current user not found by GlobalId."));
            }

            // dotáhneme si uživatele
            Customer? customer = await _customerRepository.GetCustomerByIdTaskAsync(userId);
            if (customer == null)
            {
                _logger.LogWarning("{0} User not found, Id: {1}", logHeader, userId);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-5, "User not found", "User not found by Id."));
            }

            if (customer.IsSystemAccount)
            {
                _logger.LogWarning("{0} User is System account, Name: {1}, ID: {2}, Email: {3}", logHeader, customer.FriendlyName, userId, customer.Email);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-20, "User is System account", "Can not delete user with System account."));
            }

            if (currentUser.Id == customer.Id)
            {
                _logger.LogWarning("{0} User cannot delete himself, Name: {1}, ID: {2}, Email: {3}", logHeader, customer.FriendlyName, userId, customer.Email);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-21, "User cannot delete himself", "User cannot delete himself."));
            }

            // maže Správce Organizace Správce Poskytovatele?
            if (currentUser.IsOrganizationAdmin() && customer.IsProviderAdmin())
                providerId = null;

            // kontrola odebrání posledního Správce Poskytovatele
            if (customer.IsProviderAdmin())
            {
                DateTime prStart = DateTime.Now;
                // musíme projít všechny nesmazené EF uživatele v roli Správce posyktovatele = všechny Poskytovatele, které má přiřazené
                List<int> providers = customer.EffectiveUserUsers.Where(w => !w.Deleted && w.RoleMembers.Any(r => r.RoleId == (int)RoleMainEnum.ProviderAdmin && r.Active.GetValueOrDefault() && !r.Deleted)).Select(s => s.ProviderId).ToList();

                foreach (int i in providers)
                {
                    // kontrolujeme každého poskytovatele
                    Provider? provider = await _providerRepository.GetProviderByIdTaskAsync(i);

                    if (provider != null)
                    {
                        int otherAdminsCount = provider.EffectiveUsers.Count(w => w.UserId != customer.Id
                                                                              && !w.Deleted
                                                                              && w.Active.GetValueOrDefault()
                                                                              && w.RoleMembers.Any(r => r.RoleId == (int)RoleMainEnum.ProviderAdmin && r.Active.GetValueOrDefault() && !r.Deleted));

                        // odebral bych posledního Správce Poskytovatele
                        if (otherAdminsCount == 0)
                        {
                            _logger.LogWarning("{0} Cannot delete last Provider Admin, Name: {1}, ID: {2}, Email: {3}", logHeader, customer.FriendlyName, userId, customer.Email);
                            return BadRequest(APIErrorResponseModel.APIErrorResponse(-22, "Cannot delete last Provider Admin", "Cannot delete last Provider Admin user in Provider."));
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
                    if (!existingEfUsers.Any(x => x.Id != editedEfUser.Id && x.Active.GetValueOrDefault()))
                    {
                        deleteCustomer = true;
                    }
                }
                else
                {
                    // aktuální EF je smazaný
                    editedEfUser = customer.EffectiveUserUsers.FirstOrDefault(u => u.ProviderId == providerId.Value);
                    // aktuální EF Poskytovatele je smazaný a neexistují jiní aktivní EF uživatelé Poskytovatele
                    if (editedEfUser != null && !existingEfUsers.Any(x => x.Id != editedEfUser.Id && x.Active.GetValueOrDefault()))
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
                return Ok();
            }
            else
            {
                _logger.LogWarning("{0} User was not deleted, Name: {1}, ID: {2}, Email: {3}, duration: {4}", logHeader, customer.FriendlyName, customer.Id, customer.Email, timeEnd);
                return BadRequest(APIErrorResponseModel.APIErrorResponse(-8, "User was not deleted", "Error when deleting user."));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Uloží Firebase Cloud Messaging token uživatele
    /// </summary>
    /// <param name="globalId"></param>
    /// <param name="appInstanceToken"></param>
    /// <returns></returns>
    [HttpGet(Name = "CreateOrUpdateAppInstanceToken")]
    public async Task<IActionResult> CreateOrUpdateAppInstanceToken(string globalId, string appInstanceToken)
    {

        string logHeader = _logName + ".CreateOrUpdateAppInstanceToken:";

        if (string.IsNullOrEmpty(globalId) || string.IsNullOrEmpty(appInstanceToken))
        {
            return BadRequest();
        }

        try
        {
            Customer? user = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
            if (user == null)
            {
                _logger.LogWarning($"{logHeader} Current User not found, globalID: {globalId}");
                return BadRequest();
            }

            user.AppInstanceToken = appInstanceToken;

            bool updated = await _customerRepository.UpdateCustomerTaskAsync(user, user, true);

            if (updated)
            {
                _logger.LogInformation($"{logHeader} AppInstanceToken has been updated successfully for UserId: {user.Id}");
                return Ok();
            }
            else
            {
                _logger.LogWarning($"{logHeader} AppInstanceToken update has failed for UserId: {user.Id}");
                return BadRequest();
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Uloží žádost o smazání dat uživatele do dedikované databáze.
    /// <param name="globalId">globalID uživatele co metodu volá</param>
    /// <returns></returns>
    [HttpPost(Name = "SaveUserAccountDeletionDemand")]
    public async Task<IActionResult> SaveUserAccountDeletionDemand(string globalId)
    {

        if (string.IsNullOrEmpty(globalId))
        {
            return BadRequest();
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
                _logger.Log(LogLevel.Error, "Stored procedure dbo.sp_SaveUserDataDeletionDemand has failed.");
                return NotFound();
            }

            return Ok();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

}


