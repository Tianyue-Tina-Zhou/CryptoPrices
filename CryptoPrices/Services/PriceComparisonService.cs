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
        IEnumerable<PriceRecord> _priceRecords;

        public PriceComparisonService(ILogger<PriceComparisonService> logger, IPriceRetrievingService priceRetrievingService)
        {
            _logger = logger;
            _priceRetrievingService = priceRetrievingService;
            StartAsync();
        }

        public Task StartAsync()
        {
            _logger.LogInformation("Price Comparison Service is starting.");

            _timer = new Timer(RefreshPrice, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        public IEnumerable<PriceRecord> GetPriceRecords()
        {
            if (_priceRecords == null)
                RefreshPrice(null) ;
            return _priceRecords;
        }

        private void RefreshPrice(object state)
        {
            _logger.LogInformation("Price Retrieving Service is refreshing.");
            _priceRecords = _priceRetrievingService.RefreshPriceRecords();
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
    }
}
