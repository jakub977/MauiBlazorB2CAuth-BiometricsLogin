using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.DataConnectors.Utils;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.Shared.Models;
using Principal.Telemedicine.Shared.Security;
using StackExchange.Redis;
using System;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Principal.Telemedicine.SharedApi.Controllers;

/// <summary>
/// API metody vztažené k rolím
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class RoleApiController : ControllerBase
{
    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepository;

    private readonly string _logName = "RoleApiController";


    public RoleApiController(DbContextApi dbContext, ILogger<RoleApiController> logger, IMapper mapper, IRoleRepository roleRepository)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
        _roleRepository = roleRepository;
    }
    /// <summary>
    /// Vrátí seznam rolí (pro Kendo grid)
    /// </summary>
    /// <param name="activeRolesOnly">Filtrování - pouze aktivní role</param>
    /// <param name="filterRoleCategoryId">Filtrování - podle ID kategorie role</param>
    /// <param name="filterAvailability">Filtrování - podle dostupnosti role</param>
    /// <param name="searchText">Filtrování - hledaný text v názvu role</param>
    /// <param name="showHidden">Zobrazit i smazané záznamy?</param>
    /// <param name="showSpecial">Příznak, že se jedná o uživatele v Roli Super admin nebo Správce organizace</param>
    /// <param name="order">Řazení</param>
    /// <param name="providerId">Id Poskytovatele</param>
    /// <param name="organizationId">Id Organizace</param>
    /// <returns>GenericResponse s parametrem "success" a seznamem RoleContract nebo chybu:
    /// -1 = obecná chyba
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen</returns>

    [Authorize]
    [HttpGet(Name = "GetRoles")]
    public async Task<IGenericResponse<List<RoleContract>>> GetRoles(bool activeRolesOnly, string? searchText, int? filterRoleCategoryId, int? filterAvailability, bool showHidden = false, bool showSpecial = false, string? order = "created_desc", int? providerId = null, int? organizationId = null)
     {
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".GetRoles:";
        // kontrola na vstupní data
        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<List<RoleContract>>(null, false, -4, "Current user not found", "User not found by GlobalId.");
        }

        try
        {
            List<RoleContract> data = new List<RoleContract>();

            List<DataConnectors.Models.Shared.Role> resultData = (List<DataConnectors.Models.Shared.Role>)await _roleRepository.GetRolesForGridTaskAsync(currentUser, activeRolesOnly, searchText, filterRoleCategoryId, filterAvailability, showHidden, showSpecial, order, providerId, organizationId);

            TimeSpan timeMiddle = DateTime.Now - startTime;

            foreach (var item in resultData)
            {
                data.Add(item.ConvertToRoleContract());
            }

            TimeSpan timeEnd = DateTime.Now - startTime;
            _logger.LogInformation($"{logHeader} Returning data - records: {resultData.Count}, duration: {timeEnd}, middle: {timeMiddle}");


            return new GenericResponse<List<RoleContract>>(data, true, 0, null, null, resultData.Count);
        }

        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<List<RoleContract>>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrátí seznam rolí pro drodown
    /// </summary>
    /// <param name="providerId">Id Poskytovatele</param>
    /// <param name="organizationId">Id Organizace</param>
    /// <returns>GenericResponse s parametrem "success" a seznamem RoleContract nebo chybu:
    /// -1 = obecná chyba
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen</returns>

    [Authorize]
    [HttpGet(Name = "GetRolesForDropdown")]
    public async Task<IGenericResponse<List<RoleContract>>> GetRolesForDropdown(int providerId, [FromQuery] List<int>? roleIds = null)
    {
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".GetRolesForDropdown:";
        // kontrola na vstupní data
        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<List<RoleContract>>(null, false, -4, "Current user not found", "User not found by GlobalId.");
        }

        try
        {
            List<RoleContract> data = new List<RoleContract>();
            List<DataConnectors.Models.Shared.Role> roleList = (List<DataConnectors.Models.Shared.Role>)await _roleRepository.GetRolesForDropdownListTaskAsync(currentUser, providerId, roleIds);

            TimeSpan timeMiddle = DateTime.Now - startTime;

            foreach (DataConnectors.Models.Shared.Role item in roleList)
            {
                data.Add(item.ConvertToRoleContract(false, false, false, false));
            }

            TimeSpan timeEnd = DateTime.Now - startTime;
            _logger.LogInformation($"{logHeader} Returning data - records: {roleList.Count}, duration: {timeEnd}, middle: {timeMiddle}");

            return new GenericResponse<List<RoleContract>>(data, true, 0, null, null, roleList.Count);
        }

        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<List<RoleContract>>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Uloží novou roli
    /// </summary>
    /// <param name="roleContract"> objekt RoleContract</param>
    /// <param name="createClone"> bool identifikátor zda spustit proceduru pro klonování role</param>
    ///<param name="languageId"> identifikátor vybraného jazyka</param>
    /// <returns>GenericResponse s parametrem "success" TRUE a objektem "RoleContract" nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = roli se nepodařilo dohledat dle ID
    /// -6 = roli se nepodařilo uložit
    /// -7 = roli se nepodařilo naklonovat
    /// </returns>
    [Authorize]
    [HttpPost(Name = "InsertRole")]
    public async Task<IGenericResponse<RoleContract>> InsertRole([FromBody] RoleContract? roleContract, bool createClone)
    {
        string logHeader = _logName + ".InsertRole:";
        int ret;
        DateTime startTime = DateTime.Now;
        using var tran = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // kontrola na vstupní data
            var mappedRole = new DataConnectors.Models.Shared.Role();
            mappedRole = _mapper.Map<DataConnectors.Models.Shared.Role>(roleContract);

            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Current User not found");
                return new GenericResponse<RoleContract>(roleContract, false, -4, "Current user not found", "Current user not found by GlobalId.");
            }

            if (mappedRole == null)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader}: Role is NULL, CustomerId {currentUser.Id}");
                return new GenericResponse<RoleContract>(roleContract, false, -5, "Role is NULL", "Role is NULL");
            }
            
            ret = await _roleRepository.InsertRoleTaskAsync(currentUser, mappedRole);
            TimeSpan timeMiddle = DateTime.Now - startTime;
            
            if (ret <= -1)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Role '{mappedRole.Name}', ID: {mappedRole.Id} was not inserted, duration: {timeMiddle}");
                return new GenericResponse<RoleContract>(roleContract, false, ret, "Role was not inserted", "Error when inserting role.");
            }
 
            roleContract.Id = mappedRole.Id;
            _logger.LogInformation($"{logHeader} Role '{mappedRole.Name}', ID: {mappedRole.Id} was successfully inserted, duration: {timeMiddle}");

            if (createClone)
            {
                bool cloneSuccees = false;
                
                cloneSuccees = await _roleRepository.CloneRole(mappedRole.Id);
                TimeSpan timeEnd = DateTime.Now - startTime;
                if (!cloneSuccees)
                {
                    tran.Rollback();
                    _logger.LogWarning($"{logHeader} Role '{mappedRole.Name}', ID: {mappedRole.Id} was not cloned, duration: {timeEnd}");
                    return new GenericResponse<RoleContract>(roleContract, false, -7, "Role was not cloned", "Error when cloning role.");
                }
                _logger.LogInformation($"{logHeader} Role '{mappedRole.Name}', ID: {mappedRole.Id} was successfully cloned, duration: {timeEnd}");
            }

            tran.Commit();
            return new GenericResponse<RoleContract>(roleContract, true, 0, null, null);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<RoleContract>(roleContract, false, -1, ex.Message);
        }

    }

    /// <summary>
    /// Updatuje roli
    /// </summary>
    /// <param name="roleContract"> objekt RoleContract</param>
    /// <returns>GenericResponse s parametrem "success" TRUE nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = roli se nepodařilo dohledat podle ID
    /// -6 = roli se nepodařilo updatovat
    /// -7 = roli se nepodařilo naklonovat
    /// </returns>
    [Authorize]
    [HttpPost(Name = "UpdateRole")]
    public async Task<IGenericResponse<bool>> UpdateRole([FromBody] RoleContract? roleContract, bool createClone)
    {
        string logHeader = _logName + ".UpdateRole:";
        bool ret = false;
        DateTime startTime = DateTime.Now;
        using var tran = await _dbContext.Database.BeginTransactionAsync(); 

        try
        {
            // kontrola na vstupní data
            var mappedRole = new DataConnectors.Models.Shared.Role();
            mappedRole = _mapper.Map<DataConnectors.Models.Shared.Role>(roleContract);

            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Current User not found");
                return new GenericResponse<bool>(ret, false, -4, "Current user not found", "Current user not found by GlobalId.");
            }

            var dbRole = await _roleRepository.GetRoleByIdTaskAsync(mappedRole.Id);
            if (dbRole == null)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Role not found");
                return new GenericResponse<bool>(ret, false, -5, "Role not found", "Role not found by Id.");
            }

            int update = await _roleRepository.UpdateRoleTaskAsync(currentUser, dbRole, mappedRole);
            TimeSpan timeEnd = DateTime.Now - startTime;
            if (update <= -1)
            {
                tran.Rollback();
                _logger.LogWarning($"{logHeader} Role '{mappedRole.Name}', ID: {mappedRole.Id} was not updated, duration: {timeEnd}");
                return new GenericResponse<bool>(ret, false, -6, "Role was not updated", "Error when updating role.");
            }

            _logger.LogInformation($"{logHeader} Role '{mappedRole.Name}', ID: {mappedRole.Id} was successfully updated, duration: {timeEnd}");

            if (createClone)
            {
                bool cloneSuccees = false;
                cloneSuccees = await _roleRepository.CloneRole(mappedRole.Id);
                if (!cloneSuccees)
                {
                    tran.Rollback();
                    _logger.LogWarning($"{logHeader} Role '{mappedRole.Name}', ID: {mappedRole.Id} was not cloned, duration: {timeEnd}");
                    return new GenericResponse<bool>(ret, false, -7, "Role was not cloned", "Error when cloning role.");
                }
                _logger.LogInformation($"{logHeader} Role '{mappedRole.Name}', ID: {mappedRole.Id} was successfully cloned, duration: {timeEnd}");
            }

            tran.Commit();
            return new GenericResponse<bool>(ret, true, 0, null, null);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<bool>(ret, false, -1, ex.Message);
        }

    }

    /// <summary>
    /// Smaže roli
    /// </summary>
    /// <param name="roleId">Id Role</param>
    /// <returns>GenericResponse s parametrem "success" TRUE nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné RoleId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// -5 = roli se nepodařilo dohledat dle ID
    /// -6 = roli se nepodařilo smazat
    [Authorize]
    [HttpGet(Name = "DeleteRole")]
    public async Task<IGenericResponse<bool>> DeleteRole (int roleId)
    {
        string logHeader = _logName + ".DeleteRole:";
        bool ret = false;
        DateTime startTime = DateTime.Now;

        try
        {
            // kontrola na vstupní data
            CompleteUserContract? currentUser = HttpContext.GetTmUser();
            if (currentUser == null)
            {
                _logger.LogWarning("{0} Current User not found", logHeader);
                return new GenericResponse<bool>(ret, false, -4, "Current user not found", "Current user not found by GlobalId.");
            }

            if (roleId <= 0)
            {
                _logger.LogWarning("{0} Invalid RoleId: {1}", logHeader, roleId);
                return new GenericResponse<bool>(ret, false, -2, "Invalid RoleId", "RoleId value must be greater then '0'.");
            }
 
            var dbRole = await _roleRepository.GetRoleByIdTaskAsync(roleId);
            if (dbRole == null)
            {
                _logger.LogWarning($"{logHeader} Role not found");
                return new GenericResponse<bool>(ret, false, -5, "Role not found", "Role not found by Id.");
            }

            bool delete = await _roleRepository.DeleteRoleTaskAsync(currentUser, dbRole);
            TimeSpan timeEnd = DateTime.Now - startTime;
            if (!delete)
            {
                _logger.LogWarning($"{logHeader} Role '{dbRole.Name}', ID: {dbRole.Id} was not deleted, duration: {timeEnd}");
                return new GenericResponse<bool>(ret, false, -6, "Role was not deleted", "Error when deleting role.");
            }

            ret = true;
            _logger.LogInformation($"{logHeader} Role '{dbRole.Name}', Id: {dbRole.Id} was successfully deleted, duration: {timeEnd}");

            return new GenericResponse<bool>(ret, true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<bool>(false, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrátí roli
    /// </summary>
    /// <param name="roleId">Id požadovaného poskytovatele</param>
    /// <returns>GenericResponse s parametrem "success" TRUE a objektem "RoleContract" nebo FALSE a případně chybu:
    /// -1 = obecná chyba
    /// -2 = neplatné RoleId
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen
    /// </returns>
    [Authorize]
    [HttpGet(Name = "GetRole")]
    public async Task<IGenericResponse<RoleContract>> GetRole(int roleId)
    {
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".GetRole:";
        RoleContract? role = null;

        //Aktualni uživatel
        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<RoleContract>(role, false, -4, "Current user not found", "User not found by GlobalId.");
        }

        // kontrola na vstupní data
        if (roleId <= 0)
        {
            _logger.LogWarning("{0} Invalid RoleId: {1}", logHeader, roleId);
            return new GenericResponse<RoleContract>(role, false, -2, "Invalid RoleId", "RoleId value must be greater then '0'.");
        }

        try
        {
            DataConnectors.Models.Shared.Role? data = await _roleRepository.GetRoleByIdTaskAsync(roleId);

            if (data != null)
            {
                role = data.ConvertToRoleContract();
            }

            TimeSpan endTime = DateTime.Now - startTime;

            _logger.LogInformation("{0} Data found, duration: {1}", logHeader, endTime);

            return new GenericResponse<RoleContract>(role, true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<RoleContract>(role, false, -1, ex.Message);
        }
    }

}
