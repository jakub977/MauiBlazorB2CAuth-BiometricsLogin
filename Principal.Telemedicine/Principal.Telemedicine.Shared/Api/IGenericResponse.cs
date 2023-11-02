
namespace Principal.Telemedicine.Shared.Api
{
    /// <summary>
    /// Jednotný objekt pro návratové hodnoty API
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IGenericResponse<TResult>
    {
        /// <summary>
        /// Návratová kód, záporné číslo reptezentuje chybu
        /// </summary>
        int Code { get; set; }
        /// <summary>
        /// Počet záznamů pokud vracíme seznam
        /// </summary>
        int Records { get; set; }
        /// <summary>
        /// Text zprávy
        /// </summary>
        string? Message { get; set; }
        /// <summary>
        /// Detail zprávy
        /// </summary>
        string? Detail { get; set; }
        /// <summary>
        /// Příznak, zda metoda proběhla správně
        /// </summary>
        bool Success { get; set; }
        /// <summary>
        /// Návratová data
        /// </summary>
        TResult? Data { get; set; }
    }
}
