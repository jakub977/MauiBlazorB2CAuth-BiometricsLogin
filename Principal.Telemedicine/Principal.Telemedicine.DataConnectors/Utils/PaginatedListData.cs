
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Utils
{
    /// <summary>
    /// Pomocná třída pro stránkování dat
    /// </summary>
    /// <typeparam name="T">List s daty</typeparam>
    public class PaginatedListData<T> : List<T> where T : class
    {
        /// <summary>
        /// Akltuální stránka
        /// </summary>
        public int ActualPage { get; private set; }
        /// <summary>
        /// Celkový počet stránek
        /// </summary>
        public int TotalPages { get; private set; }
        /// <summary>
        /// Celkový počet záznamů
        /// </summary>
        public int TotalRecords { get; private set; }

        /// <summary>
        /// Pomocná třída pro stránkování dat
        /// </summary>
        /// <param name="items">Data</param>
        /// <param name="count">Počet záznamů</param>
        /// <param name="page">Aktuální stránka</param>
        /// <param name="pageSize">Počet záznamů na stránce</param>
        public PaginatedListData(List<T> items, int count, int page, int pageSize)
        {
            ActualPage = page;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalRecords = count;

            this.AddRange(items);
        }

        public bool HasPreviousPage => ActualPage > 1;

        public bool HasNextPage => ActualPage < TotalPages;

        /// <summary>
        /// Vrací seznam dat za konkrétní stránku
        /// </summary>
        /// <param name="source">Zdroj dat</param>
        /// <param name="pageIndex">Index požadované stránky</param>
        /// <param name="pageSize">Počet záznamů na jedné stránce</param>
        /// <returns></returns>
        public static async Task<PaginatedListData<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedListData<T>(items, count, pageIndex, pageSize);
        }
    }
}
