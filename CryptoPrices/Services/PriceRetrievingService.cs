using CryptoPrices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        private HttpClient _krakenClient;

        public PriceRetrievingService(ILogger<PriceRetrievingService> logger)
        {
            _logger = logger;
            _cancellationToken = new CancellationToken();
            _coinbaseClient = new HttpClient() { BaseAddress = new Uri("https://api.coinbase.com/") };
            _krakenClient = new HttpClient() { BaseAddress = new Uri("https://api.kraken.com/") };
        }

        public IEnumerable<PriceRecord> RefreshPriceRecords()
        {
            List<PriceRecord> priceRecords = new List<PriceRecord>();
            Task<PriceRecord> bitcoinCoinbase = GetCoinbaseData(CryptoCurrency.Bitcoin);
            Task<PriceRecord> bitcoinKraken = GetKrakenData(CryptoCurrency.Bitcoin);
            Task.WhenAll(bitcoinCoinbase, bitcoinKraken);

            priceRecords.Add(bitcoinCoinbase.Result);
            priceRecords.Add(bitcoinKraken.Result);
            return priceRecords;
        }

        async Task<PriceRecord> GetCoinbaseData(CryptoCurrency type)
        {
            Task<Data> buyPrice = RetrieveCoinbaseDataAsync(type, true);
            Task<Data> sellPrice = RetrieveCoinbaseDataAsync(type, false);
            await Task.WhenAll(buyPrice, sellPrice);
            return new PriceRecord() { BuyPrice = buyPrice.Result.Price, SellPrice = sellPrice.Result.Price, Type = type, Client = "Coinbase" };
        }

        async Task<PriceRecord> GetKrakenData(CryptoCurrency type)
        {
            KrakenData data = await RetrieveKrakenDataAsync(type);
            var value = type == CryptoCurrency.Bitcoin ? data.CurrentPrice.BTZValue : data.CurrentPrice.BTZValue;
            return new PriceRecord() { BuyPrice = value.BuyPrice, SellPrice = value.SellPrice, Type = type, Client = "Kraken" };
        }

        async Task<Data> RetrieveCoinbaseDataAsync(CryptoCurrency type, bool buy)
        {
            Data data = null;
            try
            {
                HttpResponseMessage response = await _coinbaseClient.GetAsync($"v2/prices/{(type == CryptoCurrency.Bitcoin ? "BTC" : "ETH") }-USD/{(buy ? "buy" : "sell")}"
                                                                             , _cancellationToken);

                response.EnsureSuccessStatusCode();
                using (HttpContent content = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    data = JsonSerializer.Deserialize<CoinbaseData>(responseBody).Data;
                }

            }
            catch (HttpRequestException e)
            {
                _logger.LogError("Exception occured while retrieving data from coinbase: Message :{0} ", e.Message);
            }
            return data;
        }

        async Task<KrakenData> RetrieveKrakenDataAsync(CryptoCurrency type)
        {
            KrakenData data = null;
            try
            {
                HttpResponseMessage response = await _krakenClient.GetAsync($"0/public/Ticker?pair={(type == CryptoCurrency.Bitcoin ? "XBT" : "ETH")}USD"
                                                                             , _cancellationToken);

                response.EnsureSuccessStatusCode();
                using (HttpContent content = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    data = JsonSerializer.Deserialize<KrakenData>(responseBody);
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
