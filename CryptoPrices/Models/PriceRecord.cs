using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptoPrices.Models
{
    public class PriceRecord
    {
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public CryptoCurrency Type { get; set; }

        public override string ToString() => JsonSerializer.Serialize<PriceRecord>(this);

    }
}
