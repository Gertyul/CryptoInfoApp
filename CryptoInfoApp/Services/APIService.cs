// Services/APIService.cs
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CryptoInfoApp.Models;

namespace CryptoInfoApp.Services
{
    public static class APIService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        // Получение списка топ криптовалют (по рыночной капитализации)
        public static async Task<List<Currency>> GetTopCurrenciesAsync(int perPage = 10)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={perPage}&page=1&sparkline=false";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var currencies = JsonConvert.DeserializeObject<List<Currency>>(json);
                return currencies;
            }
            return new List<Currency>();
        }

        // Получение детальной информации о криптовалюте
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

        // Поиск криптовалют по запросу
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
    }
}
