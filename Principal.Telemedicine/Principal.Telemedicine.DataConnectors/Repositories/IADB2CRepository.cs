﻿using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad AD B2C.
/// </summary>
public interface IADB2CRepository
{
    /// <summary>
    /// Metoda aktualizuje položku Customer. 
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>Výsledek uložení true / false</returns>
    Task<bool> UpdateUserAsyncTask(Customer customer);


}
