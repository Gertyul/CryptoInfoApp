using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoInfoApp.Services
{
    public static class CoinMarketCapService
    {
        // Ваш ключ API
        private const string API_KEY = "3a811b3900ad4e30baf6b25cfa0eec16";
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Отримує ціни топ-валют з CoinMarketCap і повертає їх як Словник (Символ -> Ціна)
        /// </summary>
        public static async Task<Dictionary<string, decimal>> GetPricesAsync(int limit = 10)
        {
            var prices = new Dictionary<string, decimal>();

            try
            {
                // Формуємо запит
                var url = $"https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?start=1&limit={limit}&convert=USD";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
                request.Headers.Add("Accepts", "application/json");

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JObject.Parse(json);

                    // Розбираємо складну структуру JSON від CMC
                    foreach (var token in data["data"])
                    {
                        string symbol = token["symbol"].ToString().ToUpper(); // Наприклад: BTC
                        decimal price = token["quote"]["USD"]["price"].Value<decimal>();

                        if (!prices.ContainsKey(symbol))
                        {
                            prices.Add(symbol, price);
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"CMC Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CMC Exception: {ex.Message}");
            }

            return prices;
        }
    }
}