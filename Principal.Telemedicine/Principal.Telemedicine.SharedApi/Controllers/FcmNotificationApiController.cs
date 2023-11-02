using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.Shared.Firebase;

namespace Principal.Telemedicine.SharedApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class FcmNotificationApiController : ControllerBase
{
    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly IFcmNotificationService _fcmNotificationService;

    private readonly string _logName = "FcmNotificationApiController";

    public FcmNotificationApiController(DbContextApi dbContext, ILogger<UserApiController> logger, IMapper mapper, IFcmNotificationService fcmNotificationService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _fcmNotificationService = fcmNotificationService;
    }

    
    [HttpPost(Name = "NotifyUser")]
    public async Task<IGenericResponse<FcmNotificationResponse>> NotifyUser([FromBody]FcmNotificationRequest fcmNotificationRequest)
    {
        string logHeader = _logName + ".NotifyUser:";
        
        try
        {

            FcmNotificationResponse response = await _fcmNotificationService.SendFcmNotification("hmMK6ZZK8Qxo_JLPEeoMFLgVVWLloSnfEtFFBhA40mO", "test-lenka", "test-lenka");

            string message = response.Message;
            bool success = response.IsSuccess;

            return new GenericResponse<FcmNotificationResponse>(null, true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<FcmNotificationResponse>(null, false, -1, ex.Message);
        }
    }
}

