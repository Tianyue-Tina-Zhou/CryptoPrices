using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CryptoPrices.Models
{
    // Json mapping object for the response from Kraken
    internal class KrakenData
    {
        public string[] error { get; set; }

        [JsonPropertyName("result")]
        public CurrentPrice CurrentPrice { get; set; }
    }

    internal class CurrentPrice
    {
        [JsonPropertyName("XXBTZUSD")]
        public Value BTZValue { get; set; }

        [JsonPropertyName("XETHZUSD")]
        public Value ETHValue { get; set; }
    }

    internal class Value
    {
        public List<string> a { get; set; } = new List<string>(3);

        public List<string> b { get; set; } = new List<string>(3);

        [JsonIgnore]
        public decimal SellPrice => Math.Round(System.Convert.ToDecimal(a[0]), 2);

        [JsonIgnore]
        public decimal BuyPrice => Math.Round(System.Convert.ToDecimal(b[0]), 2);
    }
}