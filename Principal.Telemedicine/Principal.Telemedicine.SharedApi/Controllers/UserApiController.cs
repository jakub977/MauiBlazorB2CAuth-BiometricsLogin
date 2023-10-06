using AutoMapper;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Models;
using System.Diagnostics;

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
    private readonly IConfiguration _configuration;
    private readonly IADB2CRepository _adb2cRepository;

    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    private readonly string _logName = "UserApiController";

    public UserApiController(ICustomerRepository customerRepository, IProviderRepository providerRepository, IEffectiveUserRepository effectiveUserRepository,
        IConfiguration configuration, IADB2CRepository adb2cRepository, ILogger<UserApiController> logger, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _providerRepository = providerRepository;
        _effectiveUserRepository = effectiveUserRepository;
        _configuration = configuration;
        _adb2cRepository = adb2cRepository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Vrátí základní údaje uživatele.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetUserInfo")]
    public async Task<IActionResult> GetUserInfo(int userId)
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
    public async Task<IActionResult> GetUser([FromHeader(Name = "x-api-g")] string globalId, int? userId)
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
    /// <returns>Objekt uživatele</returns>
    [HttpPost(Name = "UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromHeader(Name = "x-api-g")] string globalId, CompleteUserContract user, int? providerId = null, bool isProviderAdmin = false)
    {
        string logHeader = _logName + ".UpdateUser:";
        try
        {
            if (user.Id <= 0 || string.IsNullOrEmpty(globalId))
            {
                return BadRequest();
            }

            Customer? currentUser = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
            if (currentUser == null)
            {
                _logger.LogWarning("{0} Current User not found", logHeader);
                return BadRequest();
            }

            //Customer? oldCustomerData = await _customerRepository.GetCustomerByIdTaskAsync(user.Id);
            Customer? actualData = await _customerRepository.GetCustomerByIdTaskAsync(user.Id);
            if (actualData == null)
            {
                _logger.LogWarning("{0} User not found, Name: {1} ID: {2} Email: {3}", logHeader, user.FriendlyName, user.Id, user.Email);
                return BadRequest();
            }

            //CompleteUserContract actualData = _mapper.Map<CompleteUserContract>(oldCustomerData);


            bool haveEFUser = false;
            //string oldFirstName = "";
            //string oldLastName = "";
            //bool azureEmailChange = true;
            //bool azureUpdate = false;
            //bool isRenew = false;

            if (actualData.Deleted)
            {
                //isRenew = true;
                actualData.Deleted = false;
            }

            actualData.UpdateDateUtc = DateTime.UtcNow;
            actualData.UpdatedByCustomerId = currentUser.Id;

            // došlo ke změně jméne / příjmení?
            //if (actualData.FirstName != user.FirstName || actualData.LastName != user.LastName)
            //{
            //    oldFirstName = actualData.FirstName;
            //    oldLastName = actualData.LastName;

            //    //await LogSensitiveData(currentUser, customer, actualData);
            //}

            string oldEmail = actualData.Email;
            //bool userInsertedToAzure = false;

            // pokud jde o editaci efektivního uživatele a exituje ještě jiný aktivní efektivní uživatel pro danou entitu Customer,
            // je potřeba ponechat záznam Customer v aktivním stavu 
            if (user.EffectiveUserUsers.Any() && providerId.HasValue)
            {
                var editedEfUser = user.EffectiveUserUsers.First(u => !u.Deleted && u.ProviderId == providerId.Value);
                var existingEfUsers = await _effectiveUserRepository.GetEffectiveUsersTaskAsyncTask(user.Id);
                existingEfUsers = existingEfUsers.Where(x => x.Id != editedEfUser.Id).ToList();

                if (!user.Active && existingEfUsers.Any(x => x.Active))
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

            //// pokud je vyplněný kód pojišťovny postaru, doplním vazbu přes ID
            //if (!actualData.HealthCareInsurerId.HasValue && !string.IsNullOrEmpty(actualData.HealthCareInsurerCode))
            //{
            //    var oldCode = _healthCareInsurerService.Value.GetHealthCareInsurerByCode(currentUser, actualData.HealthCareInsurerCode);
            //    if (oldCode != null)
            //    {
            //        actualData.HealthCareInsurerId = oldCode.Id;
            //        actualData.HealthCareInsurerCode = "";
            //    }
            //}

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

                    await _effectiveUserRepository.InsertEffectiveUserTaskAsync(efUserToSave);
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
                //if (existingEfUser.Expertises.Count > 0)
                //    existingEfUser.Expertises.Clear();
                //if (efUser.Expertises.Count > 0)
                //    existingEfUser.Expertises.AddRange(efUser.Expertises);
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

                    if (!existingItem.Active || existingItem.Deleted)
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

                    if (!existingItem.Active || existingItem.Deleted)
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
                var setProviders = await _providerRepository.GetProvidersTaskAsyncTask();
                setProviders = setProviders.Where(w => providers.Contains(w.Id) && !w.Active).ToList();
                if (setProviders.Count() > 0)
                    foreach (var provider in setProviders)
                    {
                        if (!provider.Active)
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
            if (actualData.EffectiveUserUsers.Where(w => !w.Deleted).Count() == 0 && (actualData.RoleMemberDirectUsers.Where(w => !w.Deleted).Count() > 0 || user.RoleMemberDirectUsers.Count() > 0))
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

                    if (!existingItem.Active || existingItem.Deleted)
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

            /*
            // nemáme GlobalId, zkusíme ho získat
            if (string.IsNullOrEmpty(actualData.GlobalId))
            {
                if (useDBToAzure)
                {
                    if (!_biDirectionalQueueService.GetAzureCustomerGlobalId(currentUser, actualData)) // GlobalId jsme nezískali, uživatel asi neexistuje, založíme ho
                    {
                        userInsertedToAzure = _biDirectionalQueueService.InsertUpdateAzureCustomer(currentUser, actualData, true);
                    }
                }
                else
                {
                    if (!azure.GetAzureCustomerGlobalId(currentUser, actualData)) // GlobalId jsme nezískali, uživatel asi neexistuje, založíme ho
                    {
                        userInsertedToAzure = azure.InsertUpdateAzureCustomer(currentUser, actualData, true);
                    }
                }
            }
            else
            {
                // obnova uživatele
                if (!userInsertedToAzure && isRenew)
                {
                    // pokud došlo ke změně emailu, použijeme starý
                    if (actualData.Email != oldEmail)
                        actualData.Email = oldEmail;

                    if (string.IsNullOrEmpty(oldFirstName))
                    {
                        if (useDBToAzure)
                        {
                            azureUpdate = _biDirectionalQueueService.InsertUpdateAzureCustomer(currentUser, actualData, false, false, true, "", "");
                        }
                        else
                        {
                            azureUpdate = azure.InsertUpdateAzureCustomer(currentUser, actualData, false, false, true, "", "");
                        }
                    }
                    else
                    {
                        actualData.FirstName = oldFirstName;
                        actualData.LastName = oldLastName;

                        if (useDBToAzure)
                        {
                            azureUpdate = _biDirectionalQueueService.InsertUpdateAzureCustomer(currentUser, actualData, false, false, true, "", "");
                        }
                        else
                        {
                            azureUpdate = azure.InsertUpdateAzureCustomer(currentUser, actualData, false, false, true, "", "");
                        }

                        actualData.FirstName = user.FirstName;
                        actualData.LastName = user.LastName;
                    }
                    // nastavíme případný nový email
                    actualData.Email = user.Email;
                }

                // změna emailu
                if (!userInsertedToAzure && actualData.Email != oldEmail)
                {
                    if (string.IsNullOrEmpty(oldFirstName))
                    {
                        if (useDBToAzure)
                        {
                            azureEmailChange = _biDirectionalQueueService.UpdateAzureCustomerEmail(currentUser, actualData, oldEmail);
                        }
                        else
                        {
                            azureEmailChange = azure.UpdateAzureCustomerEmail(currentUser, actualData, oldEmail);
                        }
                    }
                    else
                    {
                        actualData.FirstName = oldFirstName;
                        actualData.LastName = oldLastName;

                        if (useDBToAzure)
                        {
                            azureEmailChange = _biDirectionalQueueService.UpdateAzureCustomerEmail(currentUser, actualData, oldEmail);
                        }
                        else
                        {
                            azureEmailChange = azure.UpdateAzureCustomerEmail(currentUser, actualData, oldEmail);
                        }

                        actualData.FirstName = user.FirstName;
                        actualData.LastName = user.LastName;
                    }
                }

                // update uživatele
                if (useDBToAzure)
                {
                    azureUpdate = _biDirectionalQueueService.InsertUpdateAzureCustomer(currentUser, actualData, false, false, false, oldFirstName, oldLastName);
                }
                else
                {
                    azureUpdate = azure.InsertUpdateAzureCustomer(currentUser, actualData, false, false, false, oldFirstName, oldLastName);
                }
            }

            if (!azureUpdate || !azureEmailChange)
            {
                Logger.WarnFormat("{0}: Azure update error, {1}", customerId: currentUser.Id, logMethod, logData);
                return -20;
            }
            */

            bool ret = await _customerRepository.UpdateCustomerTaskAsync(actualData);

            if (ret)
            {
                _logger.LogInformation("{0} User '{1}', Email: '{2}', Id: {3} updated succesfully", logHeader, actualData.FriendlyName, actualData.Email, actualData.Id);
                return Ok(user);
            }
            else
            {
                _logger.LogWarning("{0} User was not updated, Name: {1} ID: {2} Email: {3}", logHeader, user.FriendlyName, user.Id, user.Email);
                return BadRequest();
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
                _logger.LogWarning($"{logHeader} Current User not found");
                return BadRequest();
            }

            user.AppInstanceToken = appInstanceToken;

            bool updated = await _customerRepository.UpdateCustomerTaskAsync(user, true);

            if (updated)
            {
                _logger.LogInformation("AppInstanceToken has been updated successfully");
                return Ok();
            }
            else
            {
                _logger.LogWarning("AppInstanceToken update has failed");
                return BadRequest();
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

}