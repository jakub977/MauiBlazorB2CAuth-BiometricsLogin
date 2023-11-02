using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Interfaces;
/// <summary>
/// Nastavení vkládané položky do <see cref="IBaseCache" />
/// </summary>
public interface IBaseCacheEntryOptions
{
    /// <summary>
    // Získá nebo nastaví absolutní datum vypršení platnosti položky mezipaměti.
    /// </summary>
    public DateTimeOffset? AbsoluteExpiration { get; set; }

    /// <summary>
    // Získá nebo nastaví absolutní datum vypršení platnosti položky mezipaměti.
    /// </summary>
    // Získá nebo nastaví absolutní čas vypršení platnosti vzhledem k současnosti.
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    //
    // Summary:
    //  Získá nebo nastaví, jak dlouho může být položka mezipaměti neaktivní (např. bez přístupu) předtím
    //  než bude odstraněn. Tím se neprodlouží životnost vstupu nad absolutní hodnotou
    //  vypršení platnosti (pokud je nastaveno).
    TimeSpan? SlidingExpiration { get; set; }
}