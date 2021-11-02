using System;
using System.Text.Json.Serialization;

namespace CryptoPrices.Models
{
    internal class CoinbaseData
    {
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
        public decimal Price => System.Convert.ToDecimal(Amount);

        [JsonIgnore]
        public CryptoCurrency CurrencyType => String.Equals(Base, "BTC") ? CryptoCurrency.Bitcoin : CryptoCurrency.Ethereum;
    }
}
