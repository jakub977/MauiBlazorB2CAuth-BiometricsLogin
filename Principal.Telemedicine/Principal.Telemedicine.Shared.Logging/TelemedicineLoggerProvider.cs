using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using System;
using System.Data.SqlClient;

namespace Principal.Telemedicine.Shared.Logging;
public class TelemedicineLoggerProvider: ILoggerProvider
{
    private readonly DbContextGeneral _context;


    public TelemedicineLoggerProvider(DbContextGeneral context)
    {
        _context = context;
    }

    public ILogger CreateLogger(string categoryName)
    {

        return new TelemedicineDbLogger(categoryName, _context);
    }

    public void Dispose()
    {
        if (_context != null) _context.Dispose();
    }
}
