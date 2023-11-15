using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models.TermStore;
using Microsoft.Graph.Models;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.Shared.Firebase;
using Principal.Telemedicine.DataConnectors.Repositories;

namespace Principal.Telemedicine.SharedApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class FcmNotificationApiController : ControllerBase
{
    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly IFcmNotificationService _fcmNotificationService;
    private readonly ICustomerRepository _customerRepository;


    private readonly string _logName = "FcmNotificationApiController";

    public FcmNotificationApiController(DbContextApi dbContext, ILogger<UserApiController> logger, IFcmNotificationService fcmNotificationService, ICustomerRepository customerRepository)
    {
        _dbContext = dbContext;
        _logger = logger;
        _fcmNotificationService = fcmNotificationService;
        _customerRepository = customerRepository;
    }

    
    [HttpPost(Name = "NotifyUser")]
    public async Task<IGenericResponse<string>> NotifyUser([FromBody]FcmNotificationRequest fcmNotificationRequest)
    {
        string logHeader = _logName + ".NotifyUser:";
        
        try
        {
            //Customer? customer = await _customerRepository.GetCustomerByIdTaskAsync(fcmNotificationRequest.UserId);
            //if (customer == null)
            //{
            //    _logger.LogWarning("{0} User not found, Id: {1}", logHeader, fcmNotificationRequest.UserId);
            //    return new GenericResponse<string>(null, false, -5, "User not found", "User not found by Id.");
            //}

            //string token = customer.AppInstanceToken;

            List<string> tokens = new List<string>();
            tokens.Add("fbljgSKuQoexLrvEUjwBW6:APA91bFHptGHBLgBTsU73qnT4nQ0_g88jMhvUtWSwtYZF608kY3YjyNDHqMcqO-ww_Eax6mzur9Ym47WJqVmhuXQnFi_-Y2QKgVoPi1K3HKCe4-CJ7SQ9n2xgqfn00IBbES3l5gz1ZUA");
            tokens.Add("ecpfY-4kmkHrnuwFHlMy1l:APA91bFefF8KNWzeFTv_ug0-lSaQVQPklgyG1g_FuuFPrz8b24BwdbQEUgdwtDRJTBWRR3skeOk7ht-7aC62gn8SP8VLg2a7_1j7r22xqS3RU8rTIameItQ_1GUocI8qEhSEccRGy7BW");

            string response = await _fcmNotificationService.SendFcmNotification(tokens, "test-lenka-14112023", "test-lenka-14112023");

            

            return new GenericResponse<string>(null, true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} {1}", logHeader, ex.Message);
            return new GenericResponse<string>(null, false, -1, ex.Message);
        }
    }
}

