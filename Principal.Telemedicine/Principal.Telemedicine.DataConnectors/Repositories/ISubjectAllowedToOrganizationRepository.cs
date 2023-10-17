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

    /// <summary>
    /// Metoda vrací konkrétního moduly organizace na základě id a subjectId.
    /// </summary>
    /// <param name="organizationId">ID organizace</param>
    /// // <param name="subjectId">ID modulu</param>
    /// <returns>Konkrétní poskytovatel</returns>
    Task<SubjectAllowedToOrganization?> GetSubjectAllowedToOrganizationsBySubjectAndOrganizationIdAsyncTask(int subjectId, int organizationId);
}
