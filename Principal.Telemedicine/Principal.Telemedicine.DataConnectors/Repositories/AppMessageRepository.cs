using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class AppMessageRepository : IAppMessageRepository
{
    private readonly DbContextApi _dbContext;

    public AppMessageRepository(DbContextApi dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<AppMessageTemplate?> GetTemplateByContentTypeIdTaskAsync(int contentTypeId)
    {
        var data = await _dbContext.AppMessageTemplates.Where(p => p.AppMessageContentTypeId == contentTypeId).FirstOrDefaultAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<AppMessageAdditionalAttribute?> GetAdditionalAttributeByContentTypeIdTaskAsync(int contentTypeId)
    {
        var data = await _dbContext.AppMessageAdditionalAttributes.Where(p => p.AppMessageContentTypeId == contentTypeId).FirstOrDefaultAsync();

        return data;
    }

    /// <inheritdoc/>
    public async Task<bool> InsertSentLogTaskAsync(AppMessageSentLog sentLog)
    {
        bool ret = false;

        _dbContext.AppMessageSentLogs.Add(sentLog);

        int result = await _dbContext.SaveChangesAsync();
        if (result != 0)
            ret = true;

        return ret;
    }
}

