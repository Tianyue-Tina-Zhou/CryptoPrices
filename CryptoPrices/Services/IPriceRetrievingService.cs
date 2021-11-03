using CryptoPrices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoPrices.Services
{
    /// <summary>
    /// This interface defines the contract for retrieving <see cref="PriceRecord"/>
    /// </summary>
    public interface IPriceRetrievingService
    {
        /// <summary>
        /// Async function to get a list of price records from remote servers
        /// </summary>
        /// <returns>list of <see cref="PriceRecord"/></returns>
        Task<IEnumerable<PriceRecord>> RefreshPriceRecordsAsync();
    }
}