using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Principal.Telemedicine.Shared.Models
{
    /// <summary>
    /// Třída pro vracení chyb API
    /// </summary>
    public static class APIErrorResponseModel
    {
        /// <summary>
        /// Objekt chyby, který se vloží do BadRequest.
        /// Příkald:
        /// BadRequest(APIErrorResponseModel.APIErrorResponse(-1, "Text chyby", "Popis chyby"));
        /// </summary>
        /// <param name="errorCode">Kód chyby</param>
        /// <param name="errorText">Text chyby</param>
        /// <param name="errorDescription">Doplňující popis chyby</param>
        /// <returns>ModelStateDictionary</returns>
        public static ModelStateDictionary APIErrorResponse(int errorCode, string? errorText, string? errorDescription) 
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("ErrorCode", errorCode.ToString());
            modelState.AddModelError("ErrorText", errorText);
            modelState.AddModelError("ErrorDescription", errorDescription);
            return modelState;
        }
    }
}
