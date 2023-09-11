using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.Shared.Enums;

namespace Principal.Telemedicine.SharedApi.Controllers;

/// <summary>
/// API metody týkající se ukládání a vracení dat pacienta v aplikaci Penelope
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
    [HttpGet(Name = "PENEGetUserPregnancyCalendarWithMeasuredValues")]          //[FromHeader(Name = "x-api-key")] string apiKey,
    public async Task<IActionResult> PENEGetUserPregnancyCalendarWithMeasuredValues(string userGlobalId, string preferredLanguageCode)
    {

        if (string.IsNullOrEmpty(userGlobalId) || string.IsNullOrEmpty(preferredLanguageCode))
        {
            return BadRequest();
        }

        LanguageCodeEnum lce = Enum.Parse<LanguageCodeEnum>(preferredLanguageCode);
        int languageId = (int)lce;
        try
        {

            var peneCalendar = await _dbContext.PENECalendarWithMeasuredValuesDataModel.FromSql($"dbo.sp_GetUserPregnancyCalendarWithMeasuredValues @GlobalId = {userGlobalId}, @LanguageId = {languageId} ").AsNoTracking().ToListAsync();

            if (!peneCalendar.Any())
            {
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
}

