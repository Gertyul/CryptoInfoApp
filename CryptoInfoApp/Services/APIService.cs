using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CryptoInfoApp.Models;
using System.Windows;
using System;
using Polly;
using Polly.Retry;
using System.Net;
using System.Runtime.Caching; 

namespace CryptoInfoApp.Services
{

    public static class APIService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private static readonly MemoryCache Cache = MemoryCache.Default;

        private static readonly AsyncRetryPolicy<HttpResponseMessage> retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode == (HttpStatusCode)429)
            .WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(10, retryAttempt)),
                (result, timeSpan, retryCount, context) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Request failed with 429. Waiting {timeSpan} before retry {retryCount}.");
                });

        public static async Task<List<Currency>> GetTopCurrenciesAsync(int perPage = 10, bool forceRefresh = false)
        {
            string cacheKey = $"TopCurrencies_{perPage}";

            // Перевірка кешу (якщо не примусове оновлення)
            if (!forceRefresh && Cache.Contains(cacheKey))
            {
                return (List<Currency>)Cache.Get(cacheKey);
            }

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; CryptoInfoApp/1.0)");

            // --- УВАГА: ОНОВЛЕНИЙ URL ---
            // Додано: &price_change_percentage=1h,7d
            // Залишено: &sparkline=true (для графіків)
            var url = $"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={perPage}&page=1&sparkline=true&price_change_percentage=1h,7d";

            try
            {
                // Використовуємо політику повторів (retryPolicy), яку ви додали раніше
                var response = await retryPolicy.ExecuteAsync(() => httpClient.GetAsync(url));

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var currencies = JsonConvert.DeserializeObject<List<Currency>>(json);

                    // Оновлюємо кеш (живе 60 секунд, щоб не спамити)
                    Cache.Set(cacheKey, currencies, DateTimeOffset.Now.AddSeconds(60));

                    return currencies;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Response error: {response.StatusCode}");
                    // Якщо помилка, пробуємо повернути старий кеш, якщо він є, або пустий список
                    return Cache.Contains(cacheKey) ? (List<Currency>)Cache.Get(cacheKey) : new List<Currency>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in API call: " + ex.Message);
                return new List<Currency>();
            }
        }

        public static async Task<List<Currency>> GetSimpleCurrenciesAsync(int limit = 11)
        {
            // sparkline=false - це ключовий момент! Дані будуть "легкими"
            var url = $"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={limit}&page=1&sparkline=false";

            try
            {
                var response = await retryPolicy.ExecuteAsync(() => httpClient.GetAsync(url));
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Currency>>(json);
                }
            }
            catch
            {
                // ігноруємо помилки
            }
            return new List<Currency>();
        }

        public static async Task<CurrencyDetails> GetCurrencyDetailsAsync(string id)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/{id}?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);
                var details = new CurrencyDetails
                {
                    Id = data.id,
                    Symbol = data.symbol,
                    Name = data.name,
                    Image = data.image.large,
                    CurrentPrice = data.market_data.current_price.usd,
                    TotalVolume = data.market_data.total_volume.usd,
                    PriceChangePercentage24h = data.market_data.price_change_percentage_24h,
                    Description = data.description.en,
                    Homepage = data.links.homepage[0]
                };
                return details;
            }
            return null;
        }

        public static async Task<List<Currency>> SearchCurrencyAsync(string query)
        {
            var url = $"https://api.coingecko.com/api/v3/search?query={query}";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);
                List<Currency> results = new List<Currency>();
                foreach (var coin in data.coins)
                {
                    results.Add(new Currency
                    {
                        Id = coin.id,
                        Symbol = coin.symbol,
                        Name = coin.name,
                        Image = coin.large
                    });
                }
                return results;
            }
            return new List<Currency>();
        }
        public static async Task<MarketChartData> GetMarketChartDataAsync(string id, string vsCurrency, int days)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/{id}/market_chart?vs_currency={vsCurrency}&days={days}";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<MarketChartData>(json);
                return data;
            }
            return null;
        }

        public static async Task<List<OhlcData>> GetOhlcDataAsync(string id, string vsCurrency = "usd", int days = 1)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/{id}/ohlc?vs_currency={vsCurrency}&days={days}";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                
                var data = JsonConvert.DeserializeObject<List<List<decimal>>>(json);
                var result = new List<OhlcData>();
                foreach (var item in data)
                {
                    result.Add(new OhlcData
                    {
                        Timestamp = (long)item[0],
                        Open = item[1],
                        High = item[2],
                        Low = item[3],
                        Close = item[4]
                    });
                }
                return result;
            }
            return new List<OhlcData>();
        }
    }

}
