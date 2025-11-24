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
            // Ініціалізуємо пустий список, щоб не було помилок null
            TopCurrencies = new ObservableCollection<Currency>();

            // Перше завантаження
            LoadDataAsync();

            // Таймер на 60 секунд (безпечний інтервал для безкоштовних API)
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(60);
            _timer.Tick += async (s, e) => await UpdateDataAsync();
            _timer.Start();
        }

        // Первинне завантаження
        public async void LoadDataAsync()
        {
            // 1. Отримуємо дані з CoinGecko
            var geckoData = await APIService.GetTopCurrenciesAsync(10);

            // 2. Отримуємо дані з CoinMarketCap
            var cmcData = await CoinMarketCapService.GetPricesAsync(10);

            // 3. Об'єднуємо (мерджимо) дані
            foreach (var coin in geckoData)
            {
                // CoinGecko дає символ 'btc', а CMC 'BTC'. Приводимо до верхнього регістру.
                string symbolKey = coin.Symbol.ToUpper();

                if (cmcData.ContainsKey(symbolKey))
                {
                    coin.CmcPrice = cmcData[symbolKey];
                }
            }

            TopCurrencies = new ObservableCollection<Currency>(geckoData);
        }

        // Оновлення за таймером (без миготливого перезавантаження списку)
        private async Task UpdateDataAsync()
        {
            if (System.Windows.Application.Current == null || System.Windows.Application.Current.MainWindow == null)
            {
                StopTimer();
                return;
            }

            // 1. Свіжі дані з CoinGecko (forceRefresh: true ігнорує старий кеш)
            var geckoData = await APIService.GetTopCurrenciesAsync(10, forceRefresh: true);

            // 2. Свіжі дані з CoinMarketCap
            var cmcData = await CoinMarketCapService.GetPricesAsync(10);

            if (geckoData == null || geckoData.Count == 0) return;

            // 3. Оновлюємо існуючі об'єкти в списку
            foreach (var freshCoin in geckoData)
            {
                var existingCoin = TopCurrencies.FirstOrDefault(c => c.Id == freshCoin.Id);

                if (existingCoin != null)
                {
                    // Оновлюємо основну ціну і зміни
                    existingCoin.CurrentPrice = freshCoin.CurrentPrice;
                    existingCoin.PriceChangePercentage24h = freshCoin.PriceChangePercentage24h;
                    existingCoin.PriceChangePercentage1h = freshCoin.PriceChangePercentage1h;
                    existingCoin.PriceChangePercentage7d = freshCoin.PriceChangePercentage7d;
                    existingCoin.SparklineRaw = freshCoin.SparklineRaw; // Оновлюємо графік
                    // Важливо викликати повідомлення про зміну графіку, якщо воно не в сеттері
                    existingCoin.OnPropertyChanged(nameof(existingCoin.SparklineIn7D));

                    // Оновлюємо ціну CMC
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
            // Перевіряємо, чи таймер взагалі існує і чи він вже не запущений
            if (_timer != null && !_timer.IsEnabled)
            {
                _timer.Start();
                // Можна одразу оновити дані при поверненні на сторінку, щоб не чекати хвилину
                UpdateDataAsync();
            }
        }
    }
}