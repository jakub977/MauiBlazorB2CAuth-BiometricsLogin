
namespace Principal.Telemedicine.Shared.Api
{
    public interface IGenericResponse
    {
        int Code { get; set; }
        string? Message { get; set; }
        string? Detail { get; set; }
        bool Success { get; set; }
    }
}
