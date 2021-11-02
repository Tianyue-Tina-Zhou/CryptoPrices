﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptoPrices.Models
{
    internal class CurrentPrice
    {
        [JsonPropertyName("XXBTZUSD")]
        public Value BTZValue { get; set; }
    }

    internal class Value
    {
        // TODO change this! 
        public string[] a { get; set; } = new string[3];

        public List<string> b { get; set; } = new List<string>(3);

        [JsonIgnore]
        public decimal SellPrice => System.Convert.ToDecimal(a[0]);

        [JsonIgnore]
        public decimal BuyPrice => System.Convert.ToDecimal(b[0]);
    }

    // Json mapping object for the response from Kraken
    internal class KrakenData
    {
        public string[] error { get; set; }

        [JsonPropertyName("result")]
        public CurrentPrice CurrentPrice { get; set; }
    }
}
