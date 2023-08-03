using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Principal.Telemedicine.Shared.Models;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Web.Resource;
using Principal.Telemedicine.DataConnectors.Models;

namespace Principal.Telemedicine.SharedApi.Controllers;

    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[RequiredScope(RequiredScopesConfigurationKey = "PatientInfoApiController:PatientInfoApiControllerScope")]
    public class PatientInfoApiController : ControllerBase
    {

        private readonly ApiDbContext _dbContext;
        private readonly ILogger<PatientInfoApiController> _logger;



    public PatientInfoApiController(ILogger<PatientInfoApiController> logger, ApiDbContext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    /// <summary>
    /// Vrátí agregovaný průběh příznaků uživatele (uživatelů) za časové úseky. Nebo přesněji naměřené hodnoty, subjektivní příznaky a predikce.
    /// </summary>
    /// <param name="authorization"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetAggregatedUserSymptomProgressionDataModel")]
    public IActionResult GetAggregatedUserSymptomProgressionDataModel([FromHeader] string authorization, int userId)
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
            return Problem();
        }

    }
        
    //TODO po dohodě smazat
    //[HttpGet(Name = "GetMedicalDeviceMeasuringHistoryItems")]
    //public IActionResult GetMedicalDeviceMeasuringHistoryItems([FromHeader] string authorization, int userId)
    //{

    //    if (userId <= 0)
    //    {
    //        return BadRequest();
    //    }

    //    try
    //    {
    //        List<MedicalDeviceMeasuringHistoryItemDataModel> measuredHistoryValues = _dbContext.ExecSqlQuery<MedicalDeviceMeasuringHistoryItemDataModel>($"dbo.sp_GetMedicalDeviceMeasuringHistory @userId = {userId}");

    //        if (measuredHistoryValues.Count < 1)
    //        {
    //            return NotFound();
    //        }
    //        return Ok(measuredHistoryValues.OrderByDescending(x => x.MeasuredDateUtc).ToList());

    //    }

    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex.Message);
    //        return Problem();
    //    }
    //}
    

    /// <summary>
    /// Vrátí výsledky predikce onemocnění s určitou pravděpodobností pro daného uživatele
    /// </summary>
    /// <param name="authorization"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetDiseaseDetectionResultFromMLItems")]
    public IActionResult GetDiseaseDetectionResultFromMLItems([FromHeader] string authorization, int userId)
    {

        if (userId <= 0)
        {
            return BadRequest();
        }

        try
        {

            List<DiseaseDetectionResultFromMLItemDataModel> diseaseDetectionResultsFromML = _dbContext.ExecSqlQuery<DiseaseDetectionResultFromMLItemDataModel>($"dbo.sp_GetDiseaseDetectionResultFromML @userId = {userId}");
            if (diseaseDetectionResultsFromML.Count < 1)
            {
                return NotFound();
            }
            return Ok(diseaseDetectionResultsFromML);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem();
        }
    }


    /// <summary>
    /// Vrátí výsledky predikce původce onemocnění s určitou pravděpodobností pro daného uživatele
    ///  </summary>
    /// <param name="authorization"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetDiseaseOriginDetectionResultFromMLItems")]
    public IActionResult GetDiseaseOriginDetectionResultFromMLItems([FromHeader] string authorization, int userId)
    {

        if (userId <= 0)
        {
            return BadRequest();
        }

        try
        {
               
            List<DiseaseOriginDetectionResultFromMLItemDataModel> originDetectionResultsFromML = _dbContext.ExecSqlQuery<DiseaseOriginDetectionResultFromMLItemDataModel>($"dbo.sp_GetDiseaseOriginDetectionResultFromML @userId = {userId}");

            if (originDetectionResultsFromML.Count < 1)
            {
                return NotFound();
            }
            return Ok(originDetectionResultsFromML);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem();
        }
    }


    /// <summary>
    /// Vrátí informace o klíčových vstupech do ML, které byly rozhodující pro určení diagnózy uživatele 
    /// </summary>
    /// <param name="authorization"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetDiseaseDetectionKeyInputsToMLItems")]
    public IActionResult GetDiseaseDetectionKeyInputsToMLItems([FromHeader] string authorization, int userId)
    {

        if (userId <= 0)
        {
            return BadRequest();
        }

        try
        {

            List<DiseaseDetectionKeyInputsToMLItemDataModel> keyInputsToML = _dbContext.ExecSqlQuery<DiseaseDetectionKeyInputsToMLItemDataModel>($"dbo.sp_GetDiseaseDetectionKeyInputsToML @userId = {userId}");

            if (keyInputsToML.Count < 1)
            {
                return NotFound();
            }
            return Ok(keyInputsToML);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem();
        }
    }

    /// <summary>
    /// Vrátí přehled o aktuálních/nadcházejících prohlídkách/karanténách/izolacích daného pacienta
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet(Name = "GetVirtualSurgeryBasicOverview")]
    public async Task<IActionResult> GetVirtualSurgeryBasicOverview(/*[FromHeader] string authorization,*/ int userId)
    {

        if (userId == 0)
        {
            return BadRequest();
        }

        try
        {

            var virtualBasicOverviewDataOverview = _dbContext.VirtualSurgeryBasicOverviewDataModel.FromSql($"dbo.sp_GetVirtualSurgeryBasicOverview @userId = {userId}").ToList();

            if (virtualBasicOverviewDataOverview == null)
            {
                return NotFound();
            }
            return Ok(virtualBasicOverviewDataOverview);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem();
        }
    }


    [AllowAnonymous]
    [HttpGet(Name = "GetAvailableDeviceListItems")]
    public IActionResult GetAvailableDeviceListItems(/*[FromHeader] string authorization,*/ string userGlobalId)
    {

        if (string.IsNullOrEmpty(userGlobalId))
        {
            return BadRequest();
        }

        try
        {

            List<AvailableDeviceListItemDataModel> availableDeviceList = _dbContext.ExecSqlQuery<AvailableDeviceListItemDataModel>($"dbo.sp_GetAvailableDeviceList @userGlobalId = '{userGlobalId}' ");

            if (availableDeviceList.Count < 1)
            {
                return NotFound();
            }
            return Ok(availableDeviceList);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem();
        }
    }

} 

