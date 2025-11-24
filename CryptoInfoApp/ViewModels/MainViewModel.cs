using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using CryptoInfoApp.Models;
using CryptoInfoApp.Services;

namespace CryptoInfoApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Currency> _topCurrencies;
        public ObservableCollection<Currency> TopCurrencies
        {
            get => _topCurrencies;
            set { _topCurrencies = value; OnPropertyChanged(); }
        }

        private Currency _selectedCurrency;
        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set { _selectedCurrency = value; OnPropertyChanged(); }
        }

        private readonly DispatcherTimer _timer;

        public MainViewModel()
        {
            TopCurrencies = new ObservableCollection<Currency>();

            LoadDataAsync();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(60);
            _timer.Tick += async (s, e) => await UpdateDataAsync();
            _timer.Start();
        }

        public async void LoadDataAsync()
        {
            var geckoData = await APIService.GetTopCurrenciesAsync(10);

            var cmcData = await CoinMarketCapService.GetPricesAsync(10);

            foreach (var coin in geckoData)
            {
                string symbolKey = coin.Symbol.ToUpper();

                if (cmcData.ContainsKey(symbolKey))
                {
                    coin.CmcPrice = cmcData[symbolKey];
                }
            }

            TopCurrencies = new ObservableCollection<Currency>(geckoData);
        }

        private async Task UpdateDataAsync()
        {
            if (System.Windows.Application.Current == null || System.Windows.Application.Current.MainWindow == null)
            {
                StopTimer();
                return;
            }

            var geckoData = await APIService.GetTopCurrenciesAsync(10, forceRefresh: true);

            var cmcData = await CoinMarketCapService.GetPricesAsync(10);

            if (geckoData == null || geckoData.Count == 0) return;

            foreach (var freshCoin in geckoData)
            {
                var existingCoin = TopCurrencies.FirstOrDefault(c => c.Id == freshCoin.Id);

                if (existingCoin != null)
                {
                    existingCoin.CurrentPrice = freshCoin.CurrentPrice;
                    existingCoin.PriceChangePercentage24h = freshCoin.PriceChangePercentage24h;
                    existingCoin.PriceChangePercentage1h = freshCoin.PriceChangePercentage1h;
                    existingCoin.PriceChangePercentage7d = freshCoin.PriceChangePercentage7d;
                    existingCoin.SparklineRaw = freshCoin.SparklineRaw;

                    existingCoin.OnPropertyChanged(nameof(existingCoin.SparklineIn7D));

                    string symbolKey = existingCoin.Symbol.ToUpper();
                    if (cmcData.ContainsKey(symbolKey))
                    {
                        existingCoin.CmcPrice = cmcData[symbolKey];
                    }
                }
            }
        }
        public void StopTimer()
        {
            _timer?.Stop();
        }

        public void StartTimer()
        {
            if (_timer != null && !_timer.IsEnabled)
            {
                _timer.Start();
                UpdateDataAsync();
            }
        }
    }
}