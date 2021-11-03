using CryptoPrices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoPrices.Services
{
    // This interface defines the contract for providing price comparisons
    public interface IPriceComparisonService
    {
        IEnumerable<PriceRecord> GetPriceRecords();
        IEnumerable<PriceComparisonResult> GetPriceComparisonResults();
    }
}
