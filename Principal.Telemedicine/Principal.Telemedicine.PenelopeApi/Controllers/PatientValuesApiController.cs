using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.Shared.Enums;
using System.Data;
using Newtonsoft.Json;
using Principal.Telemedicine.PenelopeData.Models;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.Shared.Models;
using Principal.Telemedicine.Shared.Security;

namespace Principal.Telemedicine.SharedApi.Controllers;

/// <summary>
/// API metody týkající se ukládání a vracení měřitelných dat (a s nimi souvisejících plánů aktivit) pacienta v aplikaci Penelope
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class PatientValuesApiController : ControllerBase
{

    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly UserApiController _userApiController;

    private readonly string _logName = "PatientValuesApiController";

    public PatientValuesApiController(ILogger<PatientValuesApiController> logger, DbContextApi dbContext, UserApiController userApiController)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userApiController = userApiController;
    }

    /// <summary>
    /// Vrátí plánovač pacienta s naměřenými hodnotami.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userGlobalId"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet(Name = "PENEGetUserPregnancyCalendarWithMeasuredValues")]          
    public async Task<IGenericResponse<List<CalendarWithMeasuredValuesDataModel>>> PENEGetUserPregnancyCalendarWithMeasuredValues(string preferredLanguageCode)
    {
        string logHeader = _logName + ".PENEGetUserPregnancyCalendarWithMeasuredValues:";

        if (string.IsNullOrEmpty(preferredLanguageCode))
        {
            _logger.LogWarning($"{logHeader} PreferredLanguage parameter is empty.");
            return new GenericResponse<List<CalendarWithMeasuredValuesDataModel>>(null, false, -2, "PreferredLanguage parameter is empty", "PreferredLanguage must be set.");
        }

        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<List<CalendarWithMeasuredValuesDataModel>>(null, false, -4, "Current user not found", "User not found");
        }

        try
        {
            LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
            int languageId = (int)lce;

            var peneCalendar =  await _dbContext.CalendarWithMeasuredValuesDataModels.FromSql($"dbo.sp_GetUserPregnancyCalendarWithMeasuredValues @GlobalId = {currentUser.GlobalId}, @LanguageId = {languageId} ").AsNoTracking().ToListAsync();
            
            if (!peneCalendar.Any())
            {
                _logger.LogWarning($"{logHeader} No calendar with measured values was found");
                return new GenericResponse<List<CalendarWithMeasuredValuesDataModel>>(null, false, -3, "Stored procedure dbo.sp_GetUserPregnancyCalendarWithMeasuredValues has failed.");
            }

            return new GenericResponse<List<CalendarWithMeasuredValuesDataModel>>(peneCalendar, true, 0);

        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<List<CalendarWithMeasuredValuesDataModel>>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrátí seznam platných aktivit v daném dni pro konkrétního pacienta.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userGlobalId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet(Name = "PENEGetOneDayScheduledActivities")]
    public async Task<IGenericResponse<List<ScheduledActivitiesDataModel>>> PENEGetOneDayScheduledActivities(string preferredLanguageCode)
    {
        string logHeader = _logName + ".PENEGetOneDayScheduledActivities:";

        if (string.IsNullOrEmpty(preferredLanguageCode))
        {
            _logger.LogWarning($"{logHeader} PreferredLanguage parameter is empty.");
            return new GenericResponse<List<ScheduledActivitiesDataModel>>(null, false, -2, "PreferredLanguage parameter is empty", "PreferredLanguage must be set.");
        }

        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<List<ScheduledActivitiesDataModel>>(null, false, -4, "Current user not found", "User not found");
        }

        try
        {
            LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
            int languageId = (int)lce;

            DateTime dateFrom = DateTime.Today;
            DateTime dateTo = DateTime.Today.AddDays(1).AddSeconds(-1);


            var oneDayActivities = await _dbContext.ScheduledActivitiesDataModels.FromSql($"dbo.sp_GetScheduledActivities @GlobalId = {currentUser.GlobalId}, @DateFrom = {dateFrom}, @DateTo = {dateTo}, @LanguageId = {languageId} ").AsNoTracking().ToListAsync();

            if (!oneDayActivities.Any())
            {
                _logger.LogWarning($"{logHeader} No scheduled activities were found");
                return new GenericResponse<List<ScheduledActivitiesDataModel>>(null, false, -3, "Stored procedure dbo.sp_GetScheduledActivities has failed.");
            }

            return new GenericResponse<List<ScheduledActivitiesDataModel>>(oneDayActivities, true, 0);

        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<List<ScheduledActivitiesDataModel>>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrátí seznam všech platných aktivit pro konkrétního pacienta.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userGlobalId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet(Name = "PENEGetAllScheduledActivities")]
    public async Task<IGenericResponse<List<ScheduledActivitiesDataModel>>> PENEGetAllScheduledActivities(string preferredLanguageCode)
    {
        string logHeader = _logName + ".PENEGetAllScheduledActivities:";

        if (string.IsNullOrEmpty(preferredLanguageCode))
        {
            _logger.LogWarning($"{logHeader} PreferredLanguage parameter is empty.");
            return new GenericResponse<List<ScheduledActivitiesDataModel>>(null, false, -2, "PreferredLanguage parameter is empty", "PreferredLanguage must be set.");
        }

        CompleteUserContract? currentUser = HttpContext.GetTmUser();
        if (currentUser == null)
        {
            _logger.LogWarning("{0} Current User not found", logHeader);
            return new GenericResponse<List<ScheduledActivitiesDataModel>>(null, false, -4, "Current user not found", "User not found");
        }

        try
        {
            LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
            int languageId = (int)lce;

            DateTime? dtStart = null;
            DateTime? dtEnd = null;

            var allActivities = await _dbContext.ScheduledActivitiesDataModels.FromSql($"dbo.sp_GetScheduledActivities @GlobalId = {currentUser.GlobalId}, @DateFrom = {dtStart}, @DateTo = {dtEnd}, @LanguageId = {languageId} ").AsNoTracking().ToListAsync();

            if (!allActivities.Any())
            {
                _logger.LogWarning($"{logHeader} No scheduled activities were found");
                return new GenericResponse<List<ScheduledActivitiesDataModel>>(null, false, -3, "Stored procedure dbo.sp_GetScheduledActivities has failed.");
            }

            return new GenericResponse<List<ScheduledActivitiesDataModel>>(allActivities, true, 0);

        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<List<ScheduledActivitiesDataModel>>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrácení posledních známých naměřených hodnot pro homepage - zde se nejedná o hodinové agregované hodnoty, ale hodnoty aktualizované po provedeném měření.
    /// </summary>
    /// <param name="userGlobalId"></param>
    /// <param name="preferredLanguageCode"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet(Name = "PENEGetLastUserMeasuredValue")]
    public async Task<IGenericResponse<List<UserMeasuredValuesDataModel>>> PENEGetLastUserMeasuredValue(string userGlobalId, string preferredLanguageCode)
    {
        string logHeader = _logName + ".PENEGetLastUserMeasuredValue:";

        if (string.IsNullOrEmpty(userGlobalId) || string.IsNullOrEmpty(preferredLanguageCode))
        {
            _logger.LogWarning($"{logHeader} UserGlobalId and preferredLanguage parameters are empty.");
            return new GenericResponse<List<UserMeasuredValuesDataModel>>(null, false, -2, "UserGlobalId and preferredLanguage parameters are empty", "UserGlobalId and preferredLanguage must be set.");
        }

        try
        {
            LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
            int languageId = (int)lce;
            
            var userMeasuredValues = await _dbContext.UserMeasuredValuesDataModels.FromSql($"dbo.sp_GetLastUserMeasuredValue @userGlobalId = {userGlobalId}, @LanguageId = {languageId} ").AsNoTracking().ToListAsync();

            if (!userMeasuredValues.Any())
            {
                _logger.LogWarning($"{logHeader} No measured values of the user were found.");
                return new GenericResponse<List<UserMeasuredValuesDataModel>>(null, false, -3, "Stored procedure dbo.sp_GetLastUserMeasuredValue has failed.");
            }

            return new GenericResponse<List<UserMeasuredValuesDataModel>>(userMeasuredValues, true, 0);

        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<List<UserMeasuredValuesDataModel>>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrátí seznamu sledovaných symptomů a očekávaných odpovědí pro mobilní aplikaci. Zjednodušená struktura symptomů nabízí výčet symptomů bez zařazení do kategorií, 
    /// s odpověďmi ve variantách true/false.
    /// </summary>
    /// <param name="preferredLanguageCode"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet(Name = "PENEGetClinicalSymptomQuestions")]
    public async Task<IGenericResponse<List<ClinicalSymptomQuestionDataModel>>> PENEGetClinicalSymptomQuestions(string preferredLanguageCode)
    {
        string logHeader = _logName + ".PENEGetClinicalSymptomQuestions:";

        if (string.IsNullOrEmpty(preferredLanguageCode))
        {
            _logger.LogWarning($"{logHeader} PreferredLanguage parameter is empty.");
            return new GenericResponse<List<ClinicalSymptomQuestionDataModel>>(null, false, -2, "PreferredLanguage parameter is empty", "PreferredLanguage must be set.");
        }

        try
        {
            LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
            int languageId = (int)lce;

            var clinicalSymptomQuestions = await _dbContext.ClinicalSymptomQuestionDataModels.FromSql($"dbo.sp_GetClinicalSymptomQuerySimpleRaw @LanguageId = {languageId} ").AsNoTracking().ToListAsync();

            if (!clinicalSymptomQuestions.Any())
            {
                _logger.LogWarning($"{logHeader} No clinical symptom questions were found.");
                return new GenericResponse<List<ClinicalSymptomQuestionDataModel>>(null, false, -3, "Stored procedure dbo.sp_GetClinicalSymptomQuerySimpleRaw has failed.");
            }

            return new GenericResponse<List<ClinicalSymptomQuestionDataModel>>(clinicalSymptomQuestions, true, 0);

        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<List<ClinicalSymptomQuestionDataModel>>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vrátí počet dní zbývajících do vypočteného termínu porodu, k zobrazení v mobilní aplikaci.
    /// </summary>
    /// <param name="userGlobalId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet(Name = "PENEGetExpectedBirthDate")]
    public async Task<IGenericResponse<List<PregnancyInfoDataModel>>> PENEGetExpectedBirthDate(string userGlobalId)
    {
        string logHeader = _logName + ".PENEGetExpectedBirthDate:";

        if (string.IsNullOrEmpty(userGlobalId))
        {
            _logger.LogWarning($"{logHeader} UserGlobalId parameter is empty.");
            return new GenericResponse<List<PregnancyInfoDataModel>>(null, false, -2, "UserGlobalId parameter is empty", "UserGlobalId must be set.");
        }

        try
        {
            var pregnancyInfo = await _dbContext.PregnancyInfoDataModels.FromSql($"dbo.sp_GetExpectedBirthDate @GlobalId = {userGlobalId}").AsNoTracking().ToListAsync();

            if (!pregnancyInfo.Any())
            {
                _logger.LogWarning($"{logHeader} No pregnancy info was found.");
                return new GenericResponse<List<PregnancyInfoDataModel>>(null, false, -3, "Stored procedure dbo.sp_GetExpectedBirthDate has failed.");
            }

            return new GenericResponse<List<PregnancyInfoDataModel>>(pregnancyInfo, true, 0);

        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<List<PregnancyInfoDataModel>>(null, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Uloží pacientky sledované hodnoty, které byly získány koncovým monitorovacím zařízením nebo vznikly ručním zadáním pacientkou.
    /// </summary>
    /// <param name="physiologicalDataRoot"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost(Name = "PENESaveMeasuredPhysiologicalDataFromMA")]
    public async Task<IGenericResponse<int>> PENESaveMeasuredPhysiologicalDataFromMA([FromBody]PhysiologicalDataRoot physiologicalDataRoot)
    {
        string logHeader = _logName + ".PENESaveMeasuredPhysiologicalDataFromMA:";
        int returnValue = -1;

        if (physiologicalDataRoot == null)
        {
            _logger.LogWarning($"{logHeader} Invalid PhysiologicalDataRoot object: {physiologicalDataRoot}");
            return new GenericResponse<int>(returnValue, false, -2, "Invalid PhysiologicalDataRoot object", "PhysiologicalDataRoot object must be set.");
        }

        try
        {
            object[] parameters = new object[1];

            string json = JsonConvert.SerializeObject(physiologicalDataRoot);

            var dataJson = new SqlParameter("dataJson", json);
            dataJson.SqlDbType = SqlDbType.NVarChar;
            dataJson.Direction = ParameterDirection.Input;
            parameters[0] = dataJson;
            
            int procedureResultState = await _dbContext.Database.ExecuteSqlRawAsync("dbo.sp_SaveMeasuredPhysiologicalDataFromMA @dataJson", parameters);
            if (procedureResultState < 0)
            {
                _logger.LogWarning($"{logHeader} Stored procedure dbo.sp_SaveMeasuredPhysiologicalDataFromMA has failed.");
                return new GenericResponse<int>(returnValue, false, -3, "Stored procedure dbo.sp_SaveMeasuredPhysiologicalDataFromMA has failed.");
            }

            return new GenericResponse<int>(procedureResultState, true, 0);
        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<int>(returnValue, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Přijme informaci o reakci pacientky na konkrétní aktivitu plánovače a uloží stav splnění konkrétní aktivity do databáze.
    /// </summary>
    /// <param name="scheduledActivity"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost(Name = "PENESaveUserResponseToNotification")]
    public async Task<IGenericResponse<int>> PENESaveUserResponseToNotification([FromBody] ScheduledActivityDataModel scheduledActivity)
    {
        string logHeader = _logName + ".PENESaveUserResponseToNotification:";
        int returnValue = -1;

        if (scheduledActivity == null)
        {
            _logger.LogWarning($"{logHeader} Invalid ScheduledActivityDataModel object: {scheduledActivity}");
            return new GenericResponse<int>(returnValue, false, -2, "Invalid ScheduledActivityDataModel object", "ScheduledActivityDataModel object must be set.");
        }

        try
        {
            object[] parameters = new object[3];

            var activityUniqueId = new SqlParameter("ActivityUniqueId", scheduledActivity.ActivityUniqueId);
            activityUniqueId.SqlDbType = SqlDbType.VarChar;
            activityUniqueId.Direction = ParameterDirection.Input;
            parameters[0] = activityUniqueId;

            var isCompleted = new SqlParameter("IsCompleted", scheduledActivity.IsCompleted);
            isCompleted.SqlDbType = SqlDbType.Bit;
            isCompleted.Direction = ParameterDirection.Input;
            parameters[1] = isCompleted;

            var dateOfCompletion = new SqlParameter("DateOfCompletion", scheduledActivity.DateOfCompletion);
            dateOfCompletion.SqlDbType = SqlDbType.DateTime;
            dateOfCompletion.Direction = ParameterDirection.Input;
            parameters[2] = dateOfCompletion;

            int procedureResultState = await _dbContext.Database.ExecuteSqlRawAsync("dbo.sp_SetActivityStatus @ActivityUniqueId, @IsCompleted,  @DateOfCompletion", parameters);
            if (procedureResultState < 0)
            {
                _logger.LogWarning($"{logHeader} Stored procedure dbo.sp_SetActivityStatus has failed.");
                return new GenericResponse<int>(returnValue, false, -3, "Stored procedure dbo.sp_SetActivityStatus has failed.");
            }

            return new GenericResponse<int>(procedureResultState, true, 0);
        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<int>(returnValue, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Uloží pacientkou označené subjektivní symptomy ze sekce Diagnostické otázky.
    /// </summary>
    /// <param name="clinicalSymptomAnswerDataModels"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost(Name = "PENESaveUserDiseaseSymptomSubjectiveAssessment")]
    public async Task<IGenericResponse<int>> PENESaveUserDiseaseSymptomSubjectiveAssessment([FromBody]List<ClinicalSymptomAnswerDataModel> clinicalSymptomAnswerDataModels)
    {
        string logHeader = _logName + ".PENESaveUserDiseaseSymptomSubjectiveAssessment:";
        int returnValue = -1;

        if (!clinicalSymptomAnswerDataModels.Any())
        {
            _logger.LogWarning($"{logHeader} Invalid ClinicalSymptomAnswerDataModel object: {clinicalSymptomAnswerDataModels}");
            return new GenericResponse<int>(returnValue, false, -2, "Invalid ClinicalSymptomAnswerDataModel object", "ClinicalSymptomAnswerDataModel object must be set.");
        }

        try
        {
            string json = JsonConvert.SerializeObject(clinicalSymptomAnswerDataModels);

            var answerJson = new SqlParameter("answerJson", json);
            answerJson.ParameterName = "@answerJson";
            answerJson.SqlDbType = SqlDbType.NVarChar;
            answerJson.Direction = ParameterDirection.Input;

            int resultState = await _dbContext.Database.ExecuteSqlAsync($"dbo.sp_SaveUserDiseaseSymptomSubjectiveAssessmentSimple @answerJson = {answerJson}");
            if (resultState < 0)
            {
                _logger.LogWarning($"{logHeader} Stored procedure dbo.sp_SaveUserDiseaseSymptomSubjectiveAssessmentSimple has failed.");
                return new GenericResponse<int>(returnValue, false, -3, "Stored procedure dbo.sp_SaveUserDiseaseSymptomSubjectiveAssessmentSimple has failed.");
            }

            return new GenericResponse<int>(resultState, true, 0);
        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<int>(returnValue, false, -1, ex.Message);
        }
    }

    /// <summary>
    /// Vygeneruje plán aktivit pacientky při aktivaci MA
    /// </summary>
    /// <param name="userGlobalId"></param>
    /// <returns></returns>
    [HttpGet(Name = "CreateScheduledActivities")]
    public async Task<IGenericResponse<int>> CreateScheduledActivities(string userGlobalId)
    {
        string logHeader = _logName + ".CreateScheduledActivities:";
        int returnValue = -1;


        if (string.IsNullOrEmpty(userGlobalId))
        {
            _logger.LogWarning($"{logHeader} UserGlobalId parameter is empty.");
            return new GenericResponse<int>(returnValue, false, -2, "UserGlobalId parameter is empty", "UserGlobalId must be set.");

        }

        try
        {
            object[] parameters = new object[1];

            var globalId = new SqlParameter("GlobalId", userGlobalId);
            globalId.SqlDbType = SqlDbType.VarChar;
            globalId.Direction = ParameterDirection.Input;
            parameters[0] = globalId;

            int procedureResultState = await _dbContext.Database.ExecuteSqlRawAsync("dbo.sp_CreateScheduledActivities_BloodPressureMeasurement @GlobalId", parameters);
            if (procedureResultState < 0)
            {
                _logger.LogWarning($"{logHeader} Stored procedure dbo.sp_CreateScheduledActivities_BloodPressureMeasurement has failed.");
                return new GenericResponse<int>(returnValue, false, -3, "Stored procedure dbo.sp_CreateScheduledActivities_BloodPressureMeasurement has failed.");
            }

            int procResultState = await _dbContext.Database.ExecuteSqlRawAsync("dbo.sp_CreateScheduledActivities_UrineProteinMeasurement @GlobalId", parameters);
            if (procResultState < 0)
            {
                _logger.LogWarning($"{logHeader} Stored procedure dbo.sp_CreateScheduledActivities_UrineProteinMeasurement has failed.");
                return new GenericResponse<int>(returnValue, false, -3, "Stored procedure dbo.sp_CreateScheduledActivities_UrineProteinMeasurement has failed.");
            }

            int procedureResult = await _dbContext.Database.ExecuteSqlRawAsync("dbo.sp_CreateScheduledActivities_Medication @GlobalId", parameters);
            if (procedureResult < 0)
            {
                _logger.LogWarning($"{logHeader} Stored procedure dbo.sp_CreateScheduledActivities_Medication has failed.");
                return new GenericResponse<int>(returnValue, false, -3, "Stored procedure dbo.sp_CreateScheduledActivities_Medication has failed.");
            }


            return new GenericResponse<int>(procedureResult, true, 0);

        }
        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<int>(returnValue, false, -1, ex.Message);
        }
    }
}

