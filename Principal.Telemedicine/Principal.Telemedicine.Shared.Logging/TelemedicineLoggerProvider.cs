using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using System;
using System.Data.SqlClient;

namespace Principal.Telemedicine.Shared.Logging;

/// <summary>
/// ILogger provider pro potřeby TM
/// </summary>
public class TelemedicineLoggerProvider: ILoggerProvider
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Konstruktor třídy
    /// </summary>
    /// <param name="context">Předpokládá instanci DBContextu DbContextGeneral obahující model Log</param>
    public TelemedicineLoggerProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Metoda pro vytvoření instance ILogger
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public ILogger CreateLogger(string categoryName)
    {
        TelemedicineDbLogger retValue = null;
        try
        {
            retValue = new TelemedicineDbLogger(categoryName,_serviceProvider);
        }
        catch
        {
            //
        }
        return retValue;
    }

    /// <summary>
    /// Dispose , pokud neco existuje a je potreba disposnout.
    /// </summary>
    public void Dispose()
    {
      
    }
}
