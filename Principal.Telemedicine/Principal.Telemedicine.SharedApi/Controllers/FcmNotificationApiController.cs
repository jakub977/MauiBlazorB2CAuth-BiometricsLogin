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

    public FcmNotificationApiController(DbContextApi dbContext, ILogger<UserApiController> logger, IFcmNotificationService fcmNotificationService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _fcmNotificationService = fcmNotificationService;
    }

    
    [HttpPost(Name = "NotifyUser")]
    public async Task<IGenericResponse<string>> NotifyUser([FromBody]FcmNotificationRequest fcmNotificationRequest)
    {
        string logHeader = _logName + ".NotifyUser:";
        
        try
        {

            string resp = await _fcmNotificationService.SendFcmNotification("fGQj8YRAGUjjqjh__J4fzX:APA91bF-QxFw7aTIHO07NOYBU1xbjUwcYXc_c0qTChH2X2-wHEwTIYzpc2JI4dnT_OPqUXwmr3BvFrqKQ535ibfpuF4QSQJb3elnoTtChWlKCYXP-Zj3vd9AqNjkyh-7hBIgV-zpdn3N", "test-lenka", "test-lenka");

            //string message = response.Message;
            //bool success = response.IsSuccess;

            return new GenericResponse<string>(null, true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<string>(null, false, -1, ex.Message);
        }
    }
}

