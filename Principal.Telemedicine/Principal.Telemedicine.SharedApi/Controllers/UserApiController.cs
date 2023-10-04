using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Models;
using System.Collections.ObjectModel;

namespace Principal.Telemedicine.SharedApi.Controllers;

/// <summary>
/// API metody vztažené k uživateli
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class UserApiController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public UserApiController(ICustomerRepository customerRepository, DbContextApi dbContext, ILogger<UserApiController> logger, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Vrátí základní údaje uživatele.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetUserInfo")]
    public async Task<IActionResult> GetUserInfo([FromHeader(Name = "x-api-key")] string apiKey, int userId)
    {

        if (userId <= 0)
        {
            return BadRequest();
        }

        try
        {
            var user = await _customerRepository.GetCustomerByIdTaskAsync(userId);
            var mappedUser = _mapper.Map<UserContract>(user);

            return Ok(mappedUser);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Vrátí údaje uživatele včetně rolí, efektivních uživatelů a oprávnění.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetUser")]
    public async Task<IActionResult> GetUser([FromHeader(Name = "x-api-g")] string globalId, int? userId)
    {

        if (userId <= 0 || string.IsNullOrEmpty(globalId))
        {
            return BadRequest();
        }

        try
        {
            var mappedUser = new CompleteUserContract();
            int id = userId.HasValue ? userId.Value : 0;

            

            if (id <= 0)
            {
                var user = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
                mappedUser = _mapper.Map<CompleteUserContract>(user);
            }
            else
            {
                // todo: zjistit jestli má uživatel oprávnění read na jiného uživatele
                // možno nastudovat v Vanda -> SmartMVC.Services -> Customers -> CustomerService.cs, metoda GetAllCustomers na ř. 161
                var user = await _customerRepository.GetCustomerByIdTaskAsync(id);
                mappedUser = _mapper.Map<CompleteUserContract>(user);
            }

            return Ok(mappedUser);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Uloží žádost o smazání uživatelského účtu uživatele do dedikované databáze.
    /// <param name="globalId"></param>
    /// <returns></returns>
    [HttpPost(Name = "SaveUserAccountDeletionDemand")]
    public async Task<IActionResult> SaveUserAccountDeletionDemand(string globalId)
    {

        if (string.IsNullOrEmpty(globalId))
        {
            return BadRequest();
        }

        try
        {
            var user = await _customerRepository.GetCustomerByGlobalIdTaskAsync(globalId);
            var mappedUser = new CompleteUserContract();
            mappedUser = _mapper.Map<CompleteUserContract>(user);

            int userId = mappedUser.Id;

            var procedureResultState = _dbContext.Database.SqlQuery<int>($"dbo.sp_SaveUserAccountDeletionDemand @userId = {userId}").ToList();
            if (!procedureResultState.Any())
            {
                _logger.Log(LogLevel.Error, "Stored procedure dbo.sp_SaveUserAccountDeletionDemand has failed.");
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

