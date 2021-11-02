using CryptoPrices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoPrices.Services
{
    public interface IPriceRetrievingService
    {
        IEnumerable<PriceRecord> RefreshPriceRecords();
    }
}
