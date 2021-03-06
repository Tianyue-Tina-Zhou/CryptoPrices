using System;
using System.Text.Json;

namespace CryptoPrices.Models
{
    /// <summary>
    /// This class represents a price record which contains the buying and selling price of a crypto currency from a client
    /// </summary>
    public class PriceRecord
    {
        /// <summary>
        /// The purchase/offer price of the crypto currency
        /// </summary>
        public decimal BuyPrice { get; set; }

        /// <summary>
        /// The selling/ask price of the crypto currency
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// The type of the crypto currency
        /// </summary>
        public CryptoCurrency Type { get; set; }

        /// <summary>
        /// The name of the client offering these prices
        /// </summary>
        public String Client { get; set; }

        public override string ToString() => JsonSerializer.Serialize<PriceRecord>(this);
    }
}