using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoPrices.Models
{
    public class PriceComparisonResult
    {
        public List<PriceRecord> PriceRecords { get; set; }
        public string PurchaseFrom { get; set; }
        public string SellTo { get; set; }
        public CryptoCurrency CryptoCurrencyType { get; set; }
    }
}
