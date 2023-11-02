using Microsoft.AspNetCore.Http;
using Principal.Telemedicine.Shared.Constants;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.Shared.Security;

/// <summary>
/// Extension pro HttpContext
/// </summary>
public static class HttpContextExtension
{
    /// <summary>
    /// Vrací aktuálního uživatele na základě ověření
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static CompleteUserContract? GetTmUser(this HttpContext context)
    {
        return (CompleteUserContract?)context.Items[ItemsKeysConst.CONTEXT_ITEM_USER];
    }

}
