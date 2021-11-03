using CryptoPrices.Models;
using System.Collections.Generic;

namespace CryptoPrices.Services
{
    /// <summary>
    /// This interface defines the contract for retrieving <see cref="PriceComparisonResult"/>
    /// </summary>
    public interface IPriceComparisonService
    {
        /// <summary>
        /// Get a list of price comparison results
        /// </summary>
        /// <returns>List of <see cref="PriceComparisonResult"/></returns>
        IEnumerable<PriceComparisonResult> GetPriceComparisonResults();
    }
}