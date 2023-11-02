using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.Shared.Firebase;
using Principal.Telemedicine.Shared.Models;

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

            FcmNotificationResponse response = await _fcmNotificationService.SendFcmNotification("emqG_SPObkznssmWdAlwnI:APA91bFvBmphVtW1fNSRsDL5L4LjNMSHkxXRnNRVs0SZe1R2wbFpR6Asf0yT0H3mJh3lpZixIPCsEIK3plE0QyoS5q1WP_883eHReZCnTAUcKAf9Jx1r-iKSAEDjyzZSeArZphtt4vlt", "test-lenka", "test-lenka");

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

