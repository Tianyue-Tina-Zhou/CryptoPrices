using System;
using System.Text.Json.Serialization;

namespace CryptoPrices.Models
{
    // Json mapping object for the response from Coinbase
    internal class CoinbaseData
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    internal class Data
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("@base")]
        public string Base { get; set; }

        [JsonIgnore]
        public decimal Price => Math.Round(System.Convert.ToDecimal(Amount), 2);

        [JsonIgnore]
        public CryptoCurrency CurrencyType => String.Equals(Base, "BTC") ? CryptoCurrency.Bitcoin : CryptoCurrency.Ethereum;
    }
}