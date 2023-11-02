using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Interfaces;
/// <summary>
/// Keš v paměti procesu.
/// </summary>
public interface IMemoryCache : IBaseCache
{
    /// <summary>
    /// Vymaže celou keš.
    /// </summary>
    void Clear();

    /// <summary>
    /// Vymaže celou keš.
    /// </summary>
    /// <param name="cancellationToken">Stornovací token.</param>
    /// <returns></returns>
    Task ClearAsync(CancellationToken cancellationToken = default);
}

