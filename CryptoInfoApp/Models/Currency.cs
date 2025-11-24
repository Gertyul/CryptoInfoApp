using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CryptoInfoApp.Models
{
    public class Currency : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("market_cap_rank")]
        public int Rank { get; set; }

        private decimal _currentPrice;
        [JsonProperty("current_price")]
        public decimal CurrentPrice
        {
            get => _currentPrice;
            set { _currentPrice = value; OnPropertyChanged(); }
        }

        private decimal _cmcPrice;
        public decimal CmcPrice
        {
            get => _cmcPrice;
            set { _cmcPrice = value; OnPropertyChanged(); }
        }

        private double _priceChangePercentage1h;
        [JsonProperty("price_change_percentage_1h_in_currency")]
        public double PriceChangePercentage1h
        {
            get => _priceChangePercentage1h;
            set { _priceChangePercentage1h = value; OnPropertyChanged(); }
        }

        private decimal _priceChangePercentage24h;
        [JsonProperty("price_change_percentage_24h")]
        public decimal PriceChangePercentage24h
        {
            get => _priceChangePercentage24h;
            set { _priceChangePercentage24h = value; OnPropertyChanged(); }
        }

        private double _priceChangePercentage7d;
        [JsonProperty("price_change_percentage_7d_in_currency")]
        public double PriceChangePercentage7d
        {
            get => _priceChangePercentage7d;
            set { _priceChangePercentage7d = value; OnPropertyChanged(); }
        }


        [JsonProperty("sparkline_in_7d")]
        public SparklineData SparklineRaw { get; set; }

        public List<double> SparklineIn7D => SparklineRaw?.Price;
    }

    public class SparklineData
    {
        [JsonProperty("price")]
        public List<double> Price { get; set; }
    }
}