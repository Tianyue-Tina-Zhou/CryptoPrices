using CryptoPrices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoPrices.Services
{
    public class PriceRetrievingService : IPriceRetrievingService
    {
        private ILogger _logger;
        private Timer _timer;
        CancellationToken _cancellationToken;
        private HttpClient _coinbaseClient;

        public PriceRetrievingService(ILogger<PriceRetrievingService> logger)
        {
            _logger = logger;
            _cancellationToken = new CancellationToken();
            _coinbaseClient = new HttpClient() { BaseAddress = new Uri("https://api.coinbase.com/") };
        }

        public IEnumerable<PriceRecord> RefreshPriceRecords()
        {
            List<PriceRecord> priceRecords = new List<PriceRecord>();
            Task<PriceRecord> bitcoin = GettingData(CryptoCurrency.Bitcoin);
            priceRecords.Add(bitcoin.Result);
            return priceRecords;
        }

        async Task<PriceRecord> GettingData(CryptoCurrency type)
        {
            Task<Data> buyPrice = GettingDataRequest(type, true);
            Task<Data> sellPrice = GettingDataRequest(type, false);
            await Task.WhenAll(buyPrice, sellPrice);
            return new PriceRecord() { BuyPrice = buyPrice.Result.Price, SellPrice = sellPrice.Result.Price, Type = type };
        }

        async Task<Data> GettingDataRequest(CryptoCurrency type, bool buy)
        {
            Data data = null;
            try
            {
                HttpResponseMessage response = await _coinbaseClient.GetAsync($"v2/prices/{(type == CryptoCurrency.Bitcoin ? "BTC" : "") }-USD/{(buy ? "buy" : "sell")}"
                                                                             , _cancellationToken);

                response.EnsureSuccessStatusCode();
                using (HttpContent content = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<CoinbaseRoot>(responseBody).data;
                }

            }
            catch (HttpRequestException e)
            {
                _logger.LogError("Exception occured while retrieving data from coinbase: Message :{0} ", e.Message);
            }
            return data;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
