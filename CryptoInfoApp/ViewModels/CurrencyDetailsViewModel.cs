using CryptoInfoApp.Models;
using CryptoInfoApp.Services;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace CryptoInfoApp.ViewModels
{
    public class CurrencyDetailsViewModel : BaseViewModel
    {
        private int _chartDays = 1;
        private readonly DispatcherTimer _timer; // Таймер
        private string _currentId; // Щоб знати, яку валюту оновлювати

        private CurrencyDetails _currencyDetails;
        public CurrencyDetails CurrencyDetails
        {
            get => _currencyDetails;
            set { _currencyDetails = value; OnPropertyChanged(); }
        }

        private SeriesCollection _chartSeries;
        public SeriesCollection ChartSeries
        {
            get => _chartSeries;
            set { _chartSeries = value; OnPropertyChanged(); }
        }

        private string[] _chartLabels;
        public string[] ChartLabels
        {
            get => _chartLabels;
            set { _chartLabels = value; OnPropertyChanged(); }
        }

        private bool _isCandlestickChart = true;
        public bool IsCandlestickChart
        {
            get => _isCandlestickChart;
            set
            {
                if (_isCandlestickChart != value)
                {
                    _isCandlestickChart = value;
                    OnPropertyChanged();
                    UpdateChartSeriesAsync();
                }
            }
        }

        public ICommand ChangePeriodCommand { get; }

        public CurrencyDetailsViewModel()
        {
            ChangePeriodCommand = new RelayCommand((param) =>
            {
                if (int.TryParse(param.ToString(), out int days))
                {
                    _chartDays = days;
                    UpdateChartSeriesAsync();
                }
            });

            // Налаштовуємо таймер на 60 секунд
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(60);
            _timer.Tick += async (s, e) => await RefreshPriceAsync();
        }

        public async void LoadCurrencyDetails(string id)
        {
            _currentId = id;

            // Завантажуємо дані перший раз
            CurrencyDetails = await APIService.GetCurrencyDetailsAsync(id);
            _chartDays = 1;
            await UpdateChartSeriesAsync();

            // Запускаємо таймер
            _timer.Start();
        }

        // Метод для оновлення тільки тексту (ціни), не чіпаючи графік
        private async Task RefreshPriceAsync()
        {
            if (string.IsNullOrEmpty(_currentId)) return;

            var freshDetails = await APIService.GetCurrencyDetailsAsync(_currentId);

            if (freshDetails != null)
            {
                // Оновлюємо об'єкт, інтерфейс підхопить зміни автоматично
                CurrencyDetails = freshDetails;
            }
        }

        public async Task UpdateChartSeriesAsync()
        {
            if (CurrencyDetails == null) return;

            // Очищаємо графік перед завантаженням, щоб користувач бачив процес
            ChartSeries = null;

            if (IsCandlestickChart)
            {
                // Передаємо _chartDays у запит
                var ohlcData = await APIService.GetOhlcDataAsync(CurrencyDetails.Id, "usd", _chartDays);
                if (ohlcData == null || ohlcData.Count == 0) return;

                var candleValues = new ChartValues<OhlcPoint>();
                var labels = new List<string>(); // Використовуємо List для динамічності

                foreach (var item in ohlcData)
                {
                    candleValues.Add(new OhlcPoint((double)item.Open, (double)item.High, (double)item.Low, (double)item.Close));
                    // Форматуємо дату в залежності від періоду
                    var time = DateTimeOffset.FromUnixTimeMilliseconds(item.Timestamp);
                    labels.Add(_chartDays == 1 ? time.ToString("HH:mm") : time.ToString("dd/MM HH:mm"));
                }

                var candleSeries = new CandleSeries
                {
                    Values = candleValues,
                    IncreaseBrush = System.Windows.Media.Brushes.LimeGreen,
                    DecreaseBrush = System.Windows.Media.Brushes.Red,
                    Stroke = System.Windows.Media.Brushes.Gray,
                    StrokeThickness = 1,
                };

                ChartSeries = new SeriesCollection { candleSeries };
                ChartLabels = labels.ToArray();
            }
            else // Лінійний графік
            {
                // Передаємо _chartDays у запит
                var marketData = await APIService.GetMarketChartDataAsync(CurrencyDetails.Id, "usd", _chartDays);
                if (marketData == null || marketData.Prices == null) return;

                var lineValues = new ChartValues<double>();
                var labelsList = new List<string>();

                foreach (var point in marketData.Prices)
                {
                    lineValues.Add((double)point[1]);
                    var time = DateTimeOffset.FromUnixTimeMilliseconds((long)point[0]);
                    labelsList.Add(_chartDays == 1 ? time.ToString("HH:mm") : time.ToString("dd/MM"));
                }

                ChartSeries = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = lineValues,
                        Title = "Price",
                        PointGeometry = null,
                        Stroke = System.Windows.Media.Brushes.DodgerBlue,
                        Fill = System.Windows.Media.Brushes.Transparent
                    }
                };
                ChartLabels = labelsList.ToArray();
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
                // Можна одразу оновити ціну, щоб користувач бачив, що це працює
                Task.Run(async () => await RefreshPriceAsync());
            }
        }
    }
}