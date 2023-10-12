using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad objektem SubjectAllowedToOrganization.
/// </summary>

public interface ISubjectAllowedToOrganizationRepository
{
    /// <summary>
    /// Metoda vrací všechny moduly.
    /// </summary>
    /// <returns>Seznam modulů</returns>
    Task<IEnumerable<SubjectAllowedToOrganization>> GetSubjectsAllowedToOrganizationsAsyncTask();
}
