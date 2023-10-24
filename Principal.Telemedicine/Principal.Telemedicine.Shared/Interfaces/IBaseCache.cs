using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Interfaces;
/// <summary>
/// Společné rozhraní pro lokální i distribuovanou keš.
/// </summary>
public interface IBaseCache
{
    /// <summary>
    /// Vrací položku keše dle klíče.
    /// </summary>
    /// <typeparam name="T">Typ hodnoty</typeparam>
    /// <param name="key">Klíč</param>
    /// <returns>Položka <typeparamref name="T"/> uložená v keši.</returns>
    T Get<T>(string key);

    /// <summary>
    /// Vrací položku keše dle klíče.
    /// </summary>
    /// <typeparam name="T">Typ hodnoty</typeparam>
    /// <param name="key">Klíč</param>
    /// <param name="cancellationToken">Stornovací token.</param>
    /// <returns>Položka <typeparamref name="T"/> uložená v keši.</returns>
    Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Přidá položku do keše.
    /// </summary>
    /// <typeparam name="T">Typ objektu</typeparam>
    /// <param name="key">Klíč</param>
    /// <param name="value">Ukládáná položka</param>        
    /// <param name="options">Nastavení expirace apod. Pokud není uvedeno, použije se výchozí nastavení.</param>
    /// <returns>Vrací aktuálně použité nastavení.</returns>
    IBaseCacheEntryOptions Add<T>(string key, T value, IBaseCacheEntryOptions? options = default);

    /// <summary>
    /// Přidává položku do keše.
    /// </summary>
    /// <typeparam name="T">Typ objektu</typeparam>
    /// <param name="key">Klíč</param>
    /// <param name="value">Ukládáná položka</param>        
    /// <param name="cancellationToken">Stornovací token.</param>
    /// <param name="options">Nastavení expirace apod. Pokud není uvedeno, použije se výchozí nastavení.</param>
    /// <returns>Vrací aktuálně použité nastavení.</returns>
    Task<IBaseCacheEntryOptions> AddAsync<T>(string key, T value, CancellationToken cancellationToken = default, IBaseCacheEntryOptions? options = default);

    /// <summary>
    /// Odebere položku z keše.
    /// </summary>
    /// <param name="key">Klíč</param>
    /// <returns>Vrací <see langword="true"/> v případě úspěšného vymazání položky, <see langword="false"/> pokud položka nebyla nalezena.</returns>        
    bool TryRemove(string key);

    /// <summary>
    /// Odebere položku z keše.
    /// </summary>
    /// <param name="key">Klíč</param>
    /// <returns>Vrací <see langword="true"/> v případě úspěšného vymazání položky, <see langword="false"/> pokud položka nebyla nalezena.</returns>        
    Task<bool> TryRemoveAsync(string key);
}
