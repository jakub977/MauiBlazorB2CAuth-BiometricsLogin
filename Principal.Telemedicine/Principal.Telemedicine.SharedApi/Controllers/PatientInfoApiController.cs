using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Models;
using System.Data;
using System.Text;

namespace Principal.Telemedicine.SharedApi.Controllers;
    
    /// <summary>
    /// API metody vztažené k sekci Karta pacienta.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController] 
    public class PatientInfoApiController : ControllerBase
    {

        private readonly ApiDbContext _dbContext;
        private readonly ILogger _logger;



    public PatientInfoApiController(ILogger<PatientInfoApiController> logger, ApiDbContext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    /// <summary>
    /// Vrátí agregovaný průběh příznaků uživatele (uživatelů) za časové úseky. Nebo přesněji naměřené hodnoty, subjektivní příznaky a predikce.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userId"></param>
    /// <returns> StatusCodes </returns>
    [HttpGet(Name = "GetAggregatedUserSymptomProgressionDataModel")]
    public async Task<IActionResult> GetAggregatedUserSymptomProgressionDataModel([FromHeader(Name = "x-api-key")] string apiKey, int userId)
    {

        if (userId <= 0)
        {
            return BadRequest();
        }

        try
        {
            var result = _dbContext.Database.SqlQuery<string>($"dbo.sp_GetAggregatedUserSymptomProgression @userId = {userId}").ToList();
            if (!result.Any())
            {
                return NotFound();
            }

            var sb = new StringBuilder();
            foreach (var jsonPart in result)
                sb.Append(jsonPart);

            PatientAggregatedSymptomProgressionDataModel patientAggregatedSymptomProgressionDataModel = JsonConvert.DeserializeObject<PatientAggregatedSymptomProgressionDataModel>(sb.ToString());

            return Ok(patientAggregatedSymptomProgressionDataModel);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Vrátí výsledky predikce onemocnění s určitou pravděpodobností pro daného uživatele.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userId"></param>
    /// <returns> StatusCodes </returns>
    [HttpGet(Name = "GetDiseaseDetectionResultFromMLItems")]
    public async Task<IActionResult> GetDiseaseDetectionResultFromMLItems([FromHeader(Name = "x-api-key")] string apiKey, int userId)
    {

        if (userId <= 0)
        {
            return BadRequest();
        }

        try
        {
            
            string currentDatetime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var currentDate = new SqlParameter("CurrentDateUtc", currentDatetime);
            currentDate.DbType = DbType.DateTime;
            
            var diseaseDetectionResultsFromML = await _dbContext.DetectionResultFromMlItemDataModels.FromSql($"EXEC dbo.sp_GetDiseaseDetectionResultFromML @UserId = {userId}, @CurrentDateUtc = {currentDate} ").AsNoTracking().ToListAsync();

            if (!diseaseDetectionResultsFromML.Any())
            {
                return NotFound();
            }
            return Ok(diseaseDetectionResultsFromML);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Vrátí výsledky predikce původce onemocnění s určitou pravděpodobností pro daného uživatele.
    ///  </summary>
    /// <param name="apiKey"></param>
    /// <param name="userId"></param>
    /// <returns> StatusCodes </returns>
    [HttpGet(Name = "GetDiseaseOriginDetectionResultFromMLItems")]
    public async Task<IActionResult> GetDiseaseOriginDetectionResultFromMLItems([FromHeader(Name = "x-api-key")] string apiKey, int userId)
    {

        if (userId <= 0)
        {
            return BadRequest();
        }

        try
        {

            string currentDatetime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var currentDate = new SqlParameter("CurrentDateUtc", currentDatetime);
            currentDate.DbType = DbType.DateTime;

            var originDetectionResultsFromML = await _dbContext.DiseaseOriginDetectionResultFromMLItemDataModels.FromSql($"EXEC dbo.sp_GetDiseaseOriginDetectionResultFromML @userId = {userId},  @CurrentDateUtc = {currentDate}, @MinimumPredictionProbability = 0 ").AsNoTracking().ToListAsync();

            if (!originDetectionResultsFromML.Any())
            {
                return NotFound();
            }
            return Ok(originDetectionResultsFromML);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Vrátí informace o klíčových vstupech do ML, které byly rozhodující pro určení diagnózy uživatele.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userId"></param>
    /// <returns> StatusCodes </returns>
    [HttpGet(Name = "GetDiseaseDetectionKeyInputsToMLItems")]
    public async Task<IActionResult> GetDiseaseDetectionKeyInputsToMLItems([FromHeader(Name = "x-api-key")] string apiKey, int userId)
    {

        if (userId <= 0)
        {
            return BadRequest();
        }

        try
        {

            var keyInputsToML =  await _dbContext.DiseaseDetectionKeyInputsToMLItemDataModels.FromSql($"dbo.sp_GetDiseaseDetectionKeyInputsToML @userId = {userId}").AsNoTracking().ToListAsync();

            if (!keyInputsToML.Any())
            {
                return NotFound();
            }
            return Ok(keyInputsToML);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Vrátí přehled o aktuálních/nadcházejících prohlídkách/karanténách/izolacích daného pacienta.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userId"></param>
    /// <returns> StatusCodes </returns>
    [HttpGet(Name = "GetVirtualSurgeryBasicOverview")]
    public async Task<IActionResult> GetVirtualSurgeryBasicOverview([FromHeader(Name = "x-api-key")] string apiKey, int userId)
    {

        if (userId <= 0)
        {
            return BadRequest();
        }

        try
        {

            var virtualBasicOverviewDataOverview = await _dbContext.VirtualSurgeryBasicOverviewDataModels.FromSql($"dbo.sp_GetVirtualSurgeryBasicOverview @userId = {userId}").AsNoTracking().ToListAsync();

            if (!virtualBasicOverviewDataOverview.Any())
            {
                return NotFound();
            }
            return Ok(virtualBasicOverviewDataOverview);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Vrátí seznam in/aktivních zařízení daného uživatele.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userGlobalId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet(Name = "GetAvailableDeviceListItems")]
    public async Task<IActionResult> GetAvailableDeviceListItems([FromHeader(Name = "x-api-key")] string apiKey, string userGlobalId)
    {

        if (string.IsNullOrEmpty(userGlobalId))
        {
            return BadRequest();
        }

        try
        {

            var availableDeviceList = await _dbContext.AvailableDeviceListItemDataModels.FromSql($"dbo.sp_GetAvailableDeviceList @userGlobalId = '{userGlobalId}' ").AsNoTracking().ToListAsync();

            if (!availableDeviceList.Any())
            {
                return NotFound();
            }
            return Ok(availableDeviceList);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

} 

