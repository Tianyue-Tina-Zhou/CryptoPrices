using CryptoPrices.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoPrices.Services
{
    public class PriceRetrievingService : IPriceRetrievingService
    {
        private ILogger _logger;
        private Timer _timer;
        private CancellationToken _cancellationToken;
        private HttpClient _coinbaseClient;
        private HttpClient _krakenClient;

        public PriceRetrievingService(ILogger<PriceRetrievingService> logger)
        {
            _logger = logger;
            _cancellationToken = new CancellationToken();
            _coinbaseClient = new HttpClient() { BaseAddress = new Uri("https://api.coinbase.com/") };
            _krakenClient = new HttpClient() { BaseAddress = new Uri("https://api.kraken.com/") };
        }

        public async Task<IEnumerable<PriceRecord>> RefreshPriceRecordsAsync()
        {
            List<PriceRecord> priceRecords = new List<PriceRecord>();
            Task<PriceRecord> bitcoinCoinbase = GetCoinbaseData(CryptoCurrency.Bitcoin);
            Task<PriceRecord> ethereumCoinbase = GetCoinbaseData(CryptoCurrency.Ethereum);
            Task<PriceRecord> bitcoinKraken = GetKrakenData(CryptoCurrency.Bitcoin);
            Task<PriceRecord> ethereumKraken = GetKrakenData(CryptoCurrency.Ethereum);
            await Task.WhenAll(bitcoinCoinbase, ethereumCoinbase, bitcoinKraken, ethereumKraken);

            priceRecords.Add(bitcoinCoinbase.Result);
            priceRecords.Add(bitcoinKraken.Result);
            priceRecords.Add(ethereumCoinbase.Result);
            priceRecords.Add(ethereumKraken.Result);
            return priceRecords.Where(x => x != null);
        }

        private async Task<PriceRecord> GetCoinbaseData(CryptoCurrency type)
        {
            Task<Data> buyPrice = RetrieveCoinbaseDataAsync(type, true);
            Task<Data> sellPrice = RetrieveCoinbaseDataAsync(type, false);
            await Task.WhenAll(buyPrice, sellPrice);
            if (buyPrice?.Result == null || sellPrice?.Result == null) return null;
            return new PriceRecord() { BuyPrice = buyPrice.Result.Price, SellPrice = sellPrice.Result.Price, Type = type, Client = "Coinbase" };
        }

        private async Task<PriceRecord> GetKrakenData(CryptoCurrency type)
        {
            KrakenData data = await RetrieveKrakenDataAsync(type);
            if (data == null) return null;
            var value = type == CryptoCurrency.Bitcoin ? data.CurrentPrice.BTZValue : data.CurrentPrice.ETHValue;
            return new PriceRecord() { BuyPrice = value.BuyPrice, SellPrice = value.SellPrice, Type = type, Client = "Kraken" };
        }

        /// <summary>
        /// Gets buying or selling price from Coinbase
        /// </summary>
        /// <param name="type">Type of <see cref="CryptoCurrency"></param>
        /// <param name="buy">Whether the purchase price is needed</param>
        /// <returns></returns>
        private async Task<Data> RetrieveCoinbaseDataAsync(CryptoCurrency type, bool buy)
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
                    data = JsonSerializer.Deserialize<CoinbaseData>(responseBody)?.Data;
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("Exception occured while retrieving data from coinbase: Message :{0} ", e.Message);
            }
            return data;
        }

        /// <summary>
        /// Gets buying or selling price from Kraken
        /// </summary>
        /// <param name="type">Type of <see cref="CryptoCurrency"></param>
        /// <returns></returns>
        private async Task<KrakenData> RetrieveKrakenDataAsync(CryptoCurrency type)
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
    }
}