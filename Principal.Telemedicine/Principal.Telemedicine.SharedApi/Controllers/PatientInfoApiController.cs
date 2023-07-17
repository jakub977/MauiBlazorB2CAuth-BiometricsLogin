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
using Microsoft.Identity.Web.Resource;

namespace Principal.Telemedicine.SharedApi.Controllers;

    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredScope(RequiredScopesConfigurationKey = "PatientInfoApiController:PatientInfoApiControllerScope")]
    public class PatientInfoApiController : ControllerBase
    {

        private readonly ApiDbContext _dbContext;


        public PatientInfoApiController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet(Name = "GetAggregatedUserSymptomProgressionDataModel")]
        public PatientAggregatedSymptomProgressionDataModel GetAggregatedUserSymptomProgressionDataModel([FromHeader] string authorization,int userId)
        {
            
            var result = _dbContext.Database.SqlQuery<string>($"dbo.sp_GetAggregatedUserSymptomProgression @userId = {userId}").ToList();
            if (!result.Any())
                return null;

            var sb = new StringBuilder();
            foreach (var jsonPart in result)
                sb.Append(jsonPart);

            return JsonConvert.DeserializeObject<PatientAggregatedSymptomProgressionDataModel>(sb.ToString());

        }


        [HttpGet(Name = "GetMedicalDeviceMeasuringHistoryItems")]
        public List<MedicalDeviceMeasuringHistoryItemDataModel> GetMedicalDeviceMeasuringHistoryItems([FromHeader] string authorization,int userId)
        {

            List<MedicalDeviceMeasuringHistoryItemDataModel> measuredHistoryValues = _dbContext.ExecSqlQuery<MedicalDeviceMeasuringHistoryItemDataModel>($"dbo.sp_GetMedicalDeviceMeasuringHistory @userId = {userId}");

            return measuredHistoryValues.OrderByDescending(x => x.MeasuredDateUtc).ToList();
        }


        [HttpGet(Name = "GetDiseaseDetectionResultFromMLItems")]
        public List<DiseaseDetectionResultFromMLItemDataModel> GetDiseaseDetectionResultFromMLItems([FromHeader] string authorization, int userId)
        {

            List<DiseaseDetectionResultFromMLItemDataModel> diseaseDetectionResultsFromML = _dbContext.ExecSqlQuery<DiseaseDetectionResultFromMLItemDataModel>($"dbo.sp_GetDiseaseDetectionResultFromML @userId = {userId}");

            return diseaseDetectionResultsFromML;

        }


        [HttpGet(Name = "GetDiseaseOriginDetectionResultFromMLItems")]
        public List<DiseaseOriginDetectionResultFromMLItemDataModel> GetDiseaseOriginDetectionResultFromMLItems([FromHeader] string authorization, int userId)
        {

            List<DiseaseOriginDetectionResultFromMLItemDataModel> originDetectionResultsFromML = _dbContext.ExecSqlQuery<DiseaseOriginDetectionResultFromMLItemDataModel>($"dbo.sp_GetDiseaseOriginDetectionResultFromML @userId = {userId}");

            return originDetectionResultsFromML;
        }

    } 

