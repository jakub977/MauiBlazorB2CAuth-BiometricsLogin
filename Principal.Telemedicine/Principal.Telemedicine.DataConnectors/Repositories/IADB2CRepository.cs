﻿using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad AD B2C.
/// </summary>
public interface IADB2CRepository
{
    /// <summary>
    /// Metoda aktualizuje existujícího uživatele 
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>Výsledek uložení true / false</returns>
    Task<bool> UpdateUserAsyncTask(Customer customer);

    /// <summary>
    /// Metoda založí nového uživatele 
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>Výsledek uložení true / false</returns>
    Task<bool> InsertUserAsyncTask(Customer customer);

    /// <summary>
    /// Metoda smaže uživatele
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>Výsledek smazání true / false</returns>
    Task<bool> DeleteUserAsyncTask(Customer customer);

    /// <summary>
    /// Metoda kontroluje, zda je uživatel založen v ADB2C
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>1 - existuje, 0 - neexistuje, -1 - chyba</returns>
    Task<int> CheckUserAsyncTask(Customer customer);
}

