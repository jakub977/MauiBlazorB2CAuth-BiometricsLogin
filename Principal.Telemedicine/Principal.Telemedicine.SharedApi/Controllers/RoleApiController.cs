﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.DataConnectors.Utils;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.Shared.Models;
using Principal.Telemedicine.Shared.Security;
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

            List<Role> resultData = (List<Role>)await _roleRepository.GetRolesForGridTaskAsync(currentUser, activeRolesOnly, searchText, filterRoleCategoryId, filterAvailability, showHidden, showSpecial, order, providerId, organizationId);

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
            List<Role> roleList = (List<Role>)await _roleRepository.GetRolesForDropdownListTaskAsync(currentUser, providerId, roleIds);

            TimeSpan timeMiddle = DateTime.Now - startTime;

            foreach (Role item in roleList)
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
}