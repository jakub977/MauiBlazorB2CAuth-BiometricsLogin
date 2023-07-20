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
    private readonly DbContextGeneral _context;

    /// <summary>
    /// Konstruktor třídy
   /// </summary>
    /// <param name="context">Předpokládá instanci DBContextu DbContextGeneral obahující model Log</param>
    public TelemedicineLoggerProvider(DbContextGeneral context)
    {
        _context = context;
    }

    /// <summary>
    /// Metoda pro vytvoření instance ILogger
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public ILogger CreateLogger(string categoryName)
    {

        return new TelemedicineDbLogger(categoryName, _context);
    }

    /// <summary>
    /// Dispose contextu, pokud existuje.
    /// </summary>
    public void Dispose()
    {
        if (_context != null) _context.Dispose();
    }
}
