using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.Shared.Enums;
using System.Data;
using Newtonsoft.Json;
using Principal.Telemedicine.PenelopeData.Models;

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

    public PatientValuesApiController(ILogger<PatientValuesApiController> logger, DbContextApi dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Vrátí plánovač pacienta s naměřenými hodnotami.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userGlobalId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet(Name = "PENEGetUserPregnancyCalendarWithMeasuredValues")]          
    public async Task<IActionResult> PENEGetUserPregnancyCalendarWithMeasuredValues(string userGlobalId, string preferredLanguageCode)
    {

        if (string.IsNullOrEmpty(userGlobalId) || string.IsNullOrEmpty(preferredLanguageCode))
        {
            _logger.Log(LogLevel.Error, "GlobalId and preferredLanguage are empty.");
            return BadRequest();
        }

        try
        {
            LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
            int languageId = (int)lce;

            var peneCalendar =  await _dbContext.CalendarWithMeasuredValuesDataModels.FromSql($"dbo.sp_GetUserPregnancyCalendarWithMeasuredValues @GlobalId = {userGlobalId}, @LanguageId = {languageId} ").AsNoTracking().ToListAsync();
            
            if (!peneCalendar.Any())
            {
                _logger.Log(LogLevel.Error, "No calendar with measured values was found");
                return NotFound();
            }
            return Ok(peneCalendar);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
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
    public async Task<IActionResult> PENEGetOneDayScheduledActivities(string userGlobalId, string preferredLanguageCode)
    {

        if (string.IsNullOrEmpty(userGlobalId) || string.IsNullOrEmpty(preferredLanguageCode))
        {
            _logger.Log(LogLevel.Error, "GlobalId and preferredLanguage are empty.");
            return BadRequest();
        }

        try
        {
            LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
            int languageId = (int)lce;

            DateTime dateFrom = DateTime.Today;
            DateTime dateTo = DateTime.Today.AddDays(1).AddSeconds(-1);


            var oneDayActivities = await _dbContext.ScheduledActivitiesDataModels.FromSql($"dbo.sp_GetScheduledActivities @GlobalId = {userGlobalId}, @DateFrom = {dateFrom}, @DateTo = {dateTo}, @LanguageId = {languageId} ").AsNoTracking().ToListAsync();

            if (!oneDayActivities.Any())
            {
                _logger.Log(LogLevel.Error, "No scheduled activities were found");
                return NotFound();
            }
            return Ok(oneDayActivities);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
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
    public async Task<IActionResult> PENEGetAllScheduledActivities(string userGlobalId, string preferredLanguageCode)
    {

        if (string.IsNullOrEmpty(userGlobalId) || string.IsNullOrEmpty(preferredLanguageCode))
        {
            _logger.Log(LogLevel.Error, "GlobalId and preferredLanguage are empty.");
            return BadRequest();
        }


        try
        {
            LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
            int languageId = (int)lce;

            DateTime? dtStart = null;
            DateTime? dtEnd = null;

            var allActivities = await _dbContext.ScheduledActivitiesDataModels.FromSql($"dbo.sp_GetScheduledActivities @GlobalId = {userGlobalId}, @DateFrom = {dtStart}, @DateTo = {dtEnd}, @LanguageId = {languageId} ").AsNoTracking().ToListAsync();

            if (!allActivities.Any())
            {
                _logger.Log(LogLevel.Error, "No scheduled activities were found");
                return NotFound();
            }
            return Ok(allActivities);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
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
    public async Task<IActionResult> PENEGetLastUserMeasuredValue(string userGlobalId, string preferredLanguageCode)
    {

        if (string.IsNullOrEmpty(userGlobalId) || string.IsNullOrEmpty(preferredLanguageCode))
        {
            _logger.Log(LogLevel.Error, "GlobalId and preferredLanguage are empty.");
            return BadRequest();
        }

        try
        {
            LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
            int languageId = (int)lce;
            
            var userMeasuredValues = await _dbContext.UserMeasuredValuesDataModels.FromSql($"dbo.sp_GetLastUserMeasuredValue @GlobalId = {userGlobalId}, @LanguageId = {languageId} ").AsNoTracking().ToListAsync();

            if (!userMeasuredValues.Any())
            {
                _logger.Log(LogLevel.Error, "No measured values of the user were found");
                return NotFound();
            }
            return Ok(userMeasuredValues);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
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
    public async Task<IActionResult> PENEGetClinicalSymptomQuestions(string preferredLanguageCode)
    {

        if (string.IsNullOrEmpty(preferredLanguageCode))
        {
            _logger.Log(LogLevel.Error, "PreferredLanguage is empty.");
            return BadRequest();
        }

        try
        {
            LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
            int languageId = (int)lce;

            var clinicalSymptomQuestions = await _dbContext.ClinicalSymptomQuestionDataModels.FromSql($"dbo.sp_GetClinicalSymptomQuerySimpleRaw @LanguageId = {languageId} ").AsNoTracking().ToListAsync();

            if (!clinicalSymptomQuestions.Any())
            {
                _logger.Log(LogLevel.Error, "No clinical symptom questions were found");
                return NotFound();
            }
            return Ok(clinicalSymptomQuestions);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Vrátí počet dní zbývajících do vypočteného termínu porodu, k zobrazení v mobilní aplikaci.
    /// </summary>
    /// <param name="userGlobalId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet(Name = "PENEGetExpectedBirthDate")]
    public async Task<IActionResult> PENEGetExpectedBirthDate(string userGlobalId)
    {

        if (string.IsNullOrEmpty(userGlobalId))
        {
            _logger.Log(LogLevel.Error, "GlobalId parameter is empty.");
            return BadRequest();
        }

        try
        {
            var pregnancyInfo = _dbContext.PregnancyInfoDataModels.FromSql($"dbo.sp_GetExpectedBirthDate @GlobalId = {userGlobalId}");

            if (pregnancyInfo == null)
            {
                _logger.Log(LogLevel.Error, "No pregnancy info was found");
                return NotFound();
            }
            return Ok(pregnancyInfo);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Uloží pacientky sledované hodnoty, které byly získány koncovým monitorovacím zařízením nebo vznikly ručním zadáním pacientkou.
    /// </summary>
    /// <param name="physiologicalDataRoot"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost(Name = "PENESaveMeasuredPhysiologicalDataFromMA")]
    public async Task<IActionResult> PENESaveMeasuredPhysiologicalDataFromMA([FromBody]PhysiologicalDataRoot physiologicalDataRoot)
    {
        if (physiologicalDataRoot == null)
            return BadRequest();

        try
        {
            string json = JsonConvert.SerializeObject(physiologicalDataRoot);
            var dataJson = new SqlParameter("dataJson", json);
            dataJson.ParameterName = "@dataJson";
            dataJson.SqlDbType = SqlDbType.NVarChar;
            dataJson.Direction = ParameterDirection.Input;

            var resultState = await _dbContext.Database.ExecuteSqlRawAsync($"dbo.sp_SaveMeasuredPhysiologicalDataFromMA @dataJson = {dataJson}");
            if (resultState != 1)
            {
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

    [AllowAnonymous]
    [HttpPost(Name = "PENESaveUserResponseToNotification")]
    public async Task<IActionResult> PENESaveUserResponseToNotification([FromBody] ScheduledActivityDataModel scheduledActivity)
    {
        if (scheduledActivity == null)
            return BadRequest();

        try
        {
            var activityUniqueId = new SqlParameter("ActivityUniqueId", scheduledActivity.ActivityUniqueId);
            activityUniqueId.ParameterName = "@ActivityUniqueId";
            activityUniqueId.SqlDbType = SqlDbType.VarChar;
            activityUniqueId.Direction = ParameterDirection.Input;

            var isCompleted = new SqlParameter("IsCompleted", scheduledActivity.IsCompleted);
            isCompleted.ParameterName = "@IsCompleted";
            isCompleted.SqlDbType = SqlDbType.Bit;
            isCompleted.Direction = ParameterDirection.Input;

            var dateOfCompletion = new SqlParameter("DateOfCompletion", scheduledActivity.DateOfCompletion);
            dateOfCompletion.ParameterName = "@DateOfCompletion";
            dateOfCompletion.SqlDbType = SqlDbType.DateTime;
            dateOfCompletion.Direction = ParameterDirection.Input;

            var resultState = await _dbContext.Database.ExecuteSqlAsync($"dbo.sp_SetActivityStatus @ActivityUniqueId = {activityUniqueId}, @IsCompleted = {isCompleted},  @DateOfCompletion = {dateOfCompletion}");
            if (resultState != 1)
            {
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

    /// <summary>
    /// Uloží pacientkou označené subjektivní symptomy ze sekce Diagnostické otázky.
    /// </summary>
    /// <param name="clinicalSymptomAnswerDataModels"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost(Name = "PENESaveUserDiseaseSymptomSubjectiveAssessment")]
    public async Task<IActionResult> PENESaveUserDiseaseSymptomSubjectiveAssessment([FromBody]List<ClinicalSymptomAnswerDataModel> clinicalSymptomAnswerDataModels)
    {
        if (!clinicalSymptomAnswerDataModels.Any())
            return BadRequest();

        try
        {
            string json = JsonConvert.SerializeObject(clinicalSymptomAnswerDataModels);
            var answerJson = new SqlParameter("answerJson", json);
            answerJson.ParameterName = "@answerJson";
            answerJson.SqlDbType = SqlDbType.NVarChar;
            answerJson.Direction = ParameterDirection.Input;


            var resultState = _dbContext.Database.ExecuteSql($"dbo.sp_SaveUserDiseaseSymptomSubjectiveAssessmentSimple @answerJson = {answerJson}");
            if (resultState != 1)
            {
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

