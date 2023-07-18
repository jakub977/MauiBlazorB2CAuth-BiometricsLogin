using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Principal.Telemedicine.SharedApi.Models;
using Microsoft.AspNetCore.SignalR;
using Principal.Telemedicine.Shared.Models;
using System.Collections.Generic;
using System.Net;
using Microsoft.Identity.Web.Resource;

namespace Principal.Telemedicine.SharedApi.Controllers;

   
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredScope(RequiredScopesConfigurationKey = "PatientInfoApiController:PatientInfoApiControllerScope")]
    public class PatientInfoApiController : ControllerBase
    {

        private readonly ApiDbContext _dbContext;
        private readonly ILogger<PatientInfoApiController> _logger;



    public PatientInfoApiController(ILogger<PatientInfoApiController> logger, ApiDbContext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    [HttpGet(Name = "GetAggregatedUserSymptomProgressionDataModel")]
    public IActionResult GetAggregatedUserSymptomProgressionDataModel([FromHeader] string authorization, int userId)
    {

        try
        {
            var result = _dbContext.Database.SqlQuery<string>($"dbo.sp_GetAggregatedUserSymptomProgression @userId = {userId}").ToList();
            if (!result.Any())
                return NotFound();

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
        

    [HttpGet(Name = "GetMedicalDeviceMeasuringHistoryItems")]
    public IActionResult GetMedicalDeviceMeasuringHistoryItems([FromHeader] string authorization, int userId)
    {

        if (userId == null)
            return BadRequest();

        try
        {
            List<MedicalDeviceMeasuringHistoryItemDataModel> measuredHistoryValues = _dbContext.ExecSqlQuery<MedicalDeviceMeasuringHistoryItemDataModel>($"dbo.sp_GetMedicalDeviceMeasuringHistory @userId = {userId}");

            if (measuredHistoryValues.Count < 1)
                return NotFound();

            return Ok(measuredHistoryValues.OrderByDescending(x => x.MeasuredDateUtc).ToList());

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem();
        }
    }
    

    [AllowAnonymous]
    [HttpGet(Name = "GetDiseaseDetectionResultFromMLItems")]
    public IActionResult GetDiseaseDetectionResultFromMLItems([FromHeader] string authorization, int userId)
    {

        if (userId == null)
            return BadRequest();

        try
        {

            List<DiseaseDetectionResultFromMLItemDataModel> diseaseDetectionResultsFromML = _dbContext.ExecSqlQuery<DiseaseDetectionResultFromMLItemDataModel>($"dbo.sp_GetDiseaseDetectionResultFromML @userId = {userId}");
            if (diseaseDetectionResultsFromML.Count < 1)
                return NotFound();

            return Ok(diseaseDetectionResultsFromML);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem();
        }
    }


    [AllowAnonymous]
    [HttpGet(Name = "GetDiseaseOriginDetectionResultFromMLItems")]
    public IActionResult GetDiseaseOriginDetectionResultFromMLItems([FromHeader] string authorization, int userId)
    {

        if (userId == null)
            return BadRequest();

        try
        {
               
            List<DiseaseOriginDetectionResultFromMLItemDataModel> originDetectionResultsFromML = _dbContext.ExecSqlQuery<DiseaseOriginDetectionResultFromMLItemDataModel>($"dbo.sp_GetDiseaseOriginDetectionResultFromML @userId = {userId}");

            if (originDetectionResultsFromML.Count < 1)
                    return NotFound();

            return Ok(originDetectionResultsFromML);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem();
        }
    }

} 

