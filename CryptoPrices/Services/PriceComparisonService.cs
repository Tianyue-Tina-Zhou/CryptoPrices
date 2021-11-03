using CryptoPrices.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoPrices.Services
{
    public class PriceComparisonService : IPriceComparisonService
    {
        private ILogger _logger;
        private Timer _timer;
        private IPriceRetrievingService _priceRetrievingService;
        private IEnumerable<PriceRecord> _priceRecords;
        private IList<PriceComparisonResult> _priceComparisonResults;

        public PriceComparisonService(ILogger<PriceComparisonService> logger, IPriceRetrievingService priceRetrievingService)
        {
            _logger = logger;
            _priceRetrievingService = priceRetrievingService;
            StartAsync();
        }

        public IEnumerable<PriceComparisonResult> GetPriceComparisonResults()
        {
            if (_priceComparisonResults == null)
                RefreshPrice(null);
            return _priceComparisonResults;
        }

        public Task StartAsync()
        {
            _logger.LogInformation("Price Comparison Service is starting.");

            _timer = new Timer(RefreshPrice, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }
        public Task StopAsync()
        {
            _logger.LogInformation("Price Comparison Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void RefreshPrice(object state)
        {
            _logger.LogInformation("Price Retrieving Service is refreshing.");
            _priceRecords = _priceRetrievingService.RefreshPriceRecordsAsync().Result;
            var cryptoPrices = _priceRecords.GroupBy(x => x.Type).ToList();
            var temp = new List<PriceComparisonResult>();
            foreach (var group in cryptoPrices)
            {
                temp.Add(new PriceComparisonResult()
                {
                    PurchaseFrom = group.First(x => x.BuyPrice == group.Min(x => x.BuyPrice)).Client
                                                                          ,
                    SellTo = group.First(x => x.SellPrice == group.Max(x => x.SellPrice)).Client
                                                                          ,
                    PriceRecords = group.ToList()
                                                                          ,
                    CryptoCurrencyType = group.Key
                });
            }
            _priceComparisonResults = temp;
        }
    }
}