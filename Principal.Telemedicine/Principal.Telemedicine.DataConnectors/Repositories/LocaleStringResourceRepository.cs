using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Models;

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

    /// <inheritdoc/>
    public async Task<bool> UpdateLocaleStringResourceAsync(LocaleStringResource localeStringResource)
    {
        bool ret = false;

        bool tracking = _dbContext.ChangeTracker.Entries<LocaleStringResource>().Any(x => x.Entity.Id == localeStringResource.Id);
        if (!tracking)
        {
            _dbContext.LocaleStringResources.Update(localeStringResource);
        }

        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public bool UpdateLocaleStringResource(LocaleStringResource localeStringResource)
    {
        bool ret = false;

        bool tracking = _dbContext.ChangeTracker.Entries<LocaleStringResource>().Any(x => x.Entity.Id == localeStringResource.Id);
        if (!tracking)
        {
            _dbContext.LocaleStringResources.Update(localeStringResource);
        }

        int result = _dbContext.SaveChanges();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> InsertLocaleStringResourceAsync(LocaleStringResource localeStringResource)
    {
        bool ret = false;

        _dbContext.LocaleStringResources.Add(localeStringResource);

        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }

    /// <inheritdoc/>
    public bool InsertLocaleStringResource(LocaleStringResource localeStringResource)
    {
        bool ret = false;

        _dbContext.LocaleStringResources.Add(localeStringResource);
        int result = _dbContext.SaveChanges();
        if (result != 0)
            ret = true;

        return ret;
    }
}

