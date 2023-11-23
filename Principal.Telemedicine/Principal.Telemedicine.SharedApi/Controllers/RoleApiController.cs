using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.DataConnectors.Utils;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.Shared.Models;
using Principal.Telemedicine.Shared.Security;

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


    public RoleApiController(DbContextApi dbContext, ILogger<UserApiController> logger, IMapper mapper, IRoleRepository roleRepository)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
        _roleRepository = roleRepository;
    }
    /// <summary>
    /// Vrátí seznam rolí pro Kendo grid
    /// </summary>
    /// <param name="activeRolesOnly">Filtrování - pouze aktivní role</param>
    /// <param name="filterRoleCategoryId">Filtrování - podle ID kategorie role</param>
    /// <param name="filterAvailability">Filtrování - podle dostupnosti role</param>
    /// <param name="searchText">Filtrování - hledaný text v názvu role</param>
    /// <param name="roleIds">Seznam ID rolí, které chceme mít v seznamu bez ohledu na ostatní podmínky</param>
    /// <param name="showHidden">Zobrazit i smazané záznamy?</param>
    /// <param name="showSpecial">Příznak, že se jedná o uživatele v Roli Super admin nebo Správce organizace</param>
    /// <param name="order">Řazení</param>
    /// <param name="page">Požadovaná stránka</param>
    /// <param name="pageSize">Počet záznamů na stránce</param>
    /// <param name="providerId">Id Poskytovatele</param>
    /// <param name="organizationId">Id Organizace</param>
    /// <returns>GenericResponse s parametrem "success" a seznamem CompleteUserContract nebo chybu:
    /// -1 = obecná chyba
    /// -4 = uživatel volající metodu (podle GlobalID) nenalezen</returns>


    //[Authorize]
    [HttpPost(Name = "GetRolesForGrid")]
    public async Task<IGenericResponse<List<RoleContract>>> GetRolesForGrid(bool activeRolesOnly, string? searchText, int? filterRoleCategoryId, int? filterAvailability, List<int>? roleIds = null, bool showHidden = false, bool showSpecial = false, string? order = "created_desc", int? page = 1, int? pageSize = 20, int? providerId = null, int? organizationId = null)
    {
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".GetRolesForGrid:";
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

            PaginatedListData<Role> resultData = await _roleRepository.GetRolesForGridTaskAsync(currentUser, activeRolesOnly, searchText, filterRoleCategoryId, filterAvailability, roleIds, showHidden, showSpecial, order, page, pageSize, providerId, organizationId);

            TimeSpan timeMiddle = DateTime.Now - startTime;

            //item je Role - potřebuju přidat položku z Role do RoleContract
            //foreach (var item in resultData)
            //{
            //    data.Add(item.ConvertToCompleteUserContract(false, false, false, false, true));
            //}

            // z kontraktu do nekontraktu
            var mappedRoleContract = new RoleContract();
            mappedRoleContract = _mapper.Map<RoleContract>(resultData);

            TimeSpan timeEnd = DateTime.Now - startTime;
            _logger.LogInformation("{0} Returning data - page: {1}, records: {2}, TotalRecords: {3}, TotalPages: {4}, duration: {5}, middle: {6}", logHeader, resultData.ActualPage, resultData.Count, resultData.TotalRecords, resultData.TotalPages, timeEnd, timeMiddle);

            return new GenericResponse<List<RoleContract>>(data, true, 0, null, null, resultData.TotalRecords);
        }

        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<List<RoleContract>>(null, false, -1, ex.Message);
        }
    }
}
