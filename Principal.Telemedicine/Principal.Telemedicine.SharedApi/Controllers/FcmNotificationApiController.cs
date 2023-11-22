using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.Shared.Firebase;
using Principal.Telemedicine.DataConnectors.Repositories;

namespace Principal.Telemedicine.SharedApi.Controllers;

/// <summary>
/// Api metody pro zpracování požadavku na notifikaci a komunikaci s FCM
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class FcmNotificationApiController : ControllerBase
{
    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly IFcmNotificationService _fcmNotificationService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IAppMessageRepository _appMessageRepository;


    private readonly string _logName = "FcmNotificationApiController";

    public FcmNotificationApiController(DbContextApi dbContext, ILogger<UserApiController> logger, IFcmNotificationService fcmNotificationService, ICustomerRepository customerRepository, IAppMessageRepository appMessageRepository)
    {
        _dbContext = dbContext;
        _logger = logger;
        _fcmNotificationService = fcmNotificationService;
        _customerRepository = customerRepository;
        _appMessageRepository = appMessageRepository;
    }

    /// <summary>
    /// Zpracuje a odešle požadavek na notifikování uživatele/ů prostřednictvím FCM
    /// </summary>
    /// <param name="fcmNotificationRequest"></param>
    /// <returns>bool value</returns>
    [HttpPost(Name = "NotifyUser")]
    public async Task<IGenericResponse<bool>> NotifyUser([FromBody]FcmNotificationRequest fcmNotificationRequest)
    {
        string logHeader = _logName + ".NotifyUser:";

        if (fcmNotificationRequest == null)
        {
            _logger.LogWarning($"{logHeader} Invalid FcmNotificationRequest object: {fcmNotificationRequest}");
            return new GenericResponse<bool>(false, false, -2, "Invalid FcmNotificationRequest object", "FcmNotificationRequest object must be set.");
        }

        AppMessageTemplate? messageTemplate;
        AppMessageAdditionalAttribute? additionalAttribute;

        try
        {
            messageTemplate = await _appMessageRepository.GetTemplateByContentTypeIdTaskAsync((int)fcmNotificationRequest.AppMessageContentTypeEnum);

            additionalAttribute = await _appMessageRepository.GetAdditionalAttributeByContentTypeIdTaskAsync((int)fcmNotificationRequest.AppMessageContentTypeEnum);

            if (fcmNotificationRequest.NotifyAllUsers)
            {
                var users = await _customerRepository.GetCustomersTaskAsyncTask();
                foreach (var user in users)
                {
                    if (!user.AppInstanceToken.IsNullOrEmpty())
                    {
                        FcmNotificationResponse response = await _fcmNotificationService.SendFcmNotification(user.AppInstanceToken, messageTemplate?.Title, messageTemplate?.Body, additionalAttribute?.AttributeContent, fcmNotificationRequest?.ValidToDate);

                        if (response.IsSuccess == false)
                        {
                            _logger.LogWarning($"{logHeader} Notification process has failed. User: {user.FriendlyName}, reason: {response.Message}");
                            continue;
                        }

                        AppMessageSentLog sentLog = new AppMessageSentLog();
                        sentLog.UserId = user.Id;
                        sentLog.AppMessageContentTypeId = (int)fcmNotificationRequest.AppMessageContentTypeEnum;
                        sentLog.MessageSentDateUtc = DateTime.UtcNow;
                        sentLog.MessageId = response.MessageId;

                        bool saved = await _appMessageRepository.InsertSentLogTaskAsync(sentLog);
                        if (saved)
                        {
                            _logger.LogInformation($"{logHeader} User: {user.FriendlyName}, Email: {user.Email}, MessageId: {sentLog.MessageId} has been notificated succesfully");
                        }
                        else
                        {
                            _logger.LogWarning($"{logHeader} Error when saving AppMessageSentLog. User: {user.FriendlyName}, Email: {user.Email}");
                        }
                    }
                }
            }
            else
            {
                Customer? user = await _customerRepository.GetCustomerByGlobalIdTaskAsync(fcmNotificationRequest.UserGlobalId);
                if (user == null)
                {
                    _logger.LogWarning($"{logHeader} Current User not found, globalID: {fcmNotificationRequest.UserGlobalId}");
                    return new GenericResponse<bool>(false, false, -3, "Current User not found", "");
                }
                
                if (!user.AppInstanceToken.IsNullOrEmpty())
                {
                    FcmNotificationResponse response = await _fcmNotificationService.SendFcmNotification(user.AppInstanceToken, messageTemplate?.Title, messageTemplate?.Body, additionalAttribute?.AttributeContent, fcmNotificationRequest?.ValidToDate);

                    if (response.IsSuccess == false)
                    {
                        _logger.LogWarning($"{logHeader} Notification process has failed. User: {user.FriendlyName}, reason: {response.Message}");
                        return new GenericResponse<bool>(false, false, -1, $"Notification process has failed. User: {user.FriendlyName}", $"{response.Message}");
                    }

                    AppMessageSentLog sentLog = new AppMessageSentLog();
                    sentLog.UserId = user.Id;
                    sentLog.AppMessageContentTypeId = (int)fcmNotificationRequest.AppMessageContentTypeEnum;
                    sentLog.MessageSentDateUtc = DateTime.UtcNow;
                    sentLog.MessageId = response.MessageId;

                    bool saved = await _appMessageRepository.InsertSentLogTaskAsync(sentLog);
                    if (saved)
                    {
                        _logger.LogInformation($"{logHeader} User: {user.FriendlyName}, Email: {user.Email}, MessageId: {sentLog.MessageId} has been notificated succesfully");
                        return new GenericResponse<bool>(true, saved, 0);
                    }
                    else
                    {
                        _logger.LogWarning($"{logHeader} Error when saving AppMessageSentLog. User: {user.FriendlyName}, Email: {user.Email}");
                        return new GenericResponse<bool>(false, false, -1, "AppMessageSentLog has not been saved", "Error when saving AppMessageSentLog.");
                    }
                }
            }

            return new GenericResponse<bool>(true, true, 0);
        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return new GenericResponse<bool>(false, false, -1, ex.Message);
        }
    }
}

