using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptoPrices.Models
{
    public class Data
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

    public class CoinbaseRoot
    {
        public Data data { get; set; }
    }

}
