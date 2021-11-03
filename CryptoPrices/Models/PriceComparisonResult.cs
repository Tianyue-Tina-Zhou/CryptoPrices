using System.Collections.Generic;

namespace CryptoPrices.Models
{
    /// <summary>
    /// This class contains info about the comparison results to be displayed on the pricing view
    /// </summary>
    public class PriceComparisonResult
    {
        /// <summary>
        /// Type of crypto currency
        /// </summary>
        public CryptoCurrency CryptoCurrencyType { get; set; }

        /// <summary>
        /// List of buy & sell prices from different clients 
        /// </summary>
        public List<PriceRecord> PriceRecords { get; set; }

        /// <summary>
        /// The name of the client offering the lowest purchase price
        /// </summary>
        public string PurchaseFrom { get; set; }

        /// <summary>
        /// The name of the client offering the highest selling price
        /// </summary>
        public string SellTo { get; set; }
    }
}