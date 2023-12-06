using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class LocaleStringResourceRepository : ILocaleStringResourceRepository
{

    private readonly DbContextApi _dbContext;
    private readonly ILogger _logger;
    private readonly string _logName = "LocaleStringResourceRepository";

    public LocaleStringResourceRepository(DbContextApi dbContext, ILogger<LocaleStringResourceRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<LocaleStringResource> GetLocaleStringResourceByNameAsync(string name, int languageId)
    {
        string logHeader = string.Format("{0}.GetLocaleStringResourceByNameAsync: Name '{1}', LanguageId: {2}", _logName, name, languageId);
        LocaleStringResource? data = await _dbContext.LocaleStringResources.Where(w => w.LanguageId == languageId && w.ResourceName == name).FirstOrDefaultAsync();
        if (data == null)
        {
            _logger.LogWarning("{0} - Data not found", logHeader);
        }

        return data ?? new LocaleStringResource();
    }

    /// <inheritdoc/>
    public async Task<string> GetLocaleStringResourceValueByNameAsync(string name, int languageId)
    {
        string logHeader = string.Format("{0}.GetLocaleStringResourceValueByNameAsync: Name '{1}', LanguageId: {2}", _logName, name, languageId);
        string ret = name;
        LocaleStringResource? data = await _dbContext.LocaleStringResources.Where(w => w.LanguageId == languageId && w.ResourceName == name).FirstOrDefaultAsync();

        if (data == null)
        {
            _logger.LogWarning("{0} - Data not found", logHeader);
            return ret;
        }

        return data.ResourceValue ?? ret;
    }
}

