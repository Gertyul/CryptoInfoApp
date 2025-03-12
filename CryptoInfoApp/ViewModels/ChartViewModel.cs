using CryptoInfoApp.Services;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CryptoInfoApp.ViewModels
{
    public class ChartViewModel : BaseViewModel
    {
        // The chart series for candlesticks.
        private SeriesCollection _candlestickSeries;
        public SeriesCollection CandlestickSeries
        {
            get => _candlestickSeries;
            set { _candlestickSeries = value; OnPropertyChanged(); }
        }

        // Labels for the X-Axis (for example, time in HH:mm)
        private string[] _labels;
        public string[] Labels
        {
            get => _labels;
            set { _labels = value; OnPropertyChanged(); }
        }

        public string CurrencyId { get; set; }

        public ICommand LoadChartCommand { get; }

        // Параметризованный конструктор
        public ChartViewModel(string currencyId)
        {
            CurrencyId = currencyId;
            LoadChartCommand = new RelayCommand(async () => await LoadChartData());
        }

        // Конструктор без параметров для XAML
        public ChartViewModel() : this("bitcoin") { }

        public async Task LoadChartData()
        {
            // Fetch OHLC data for the last 7 days
            var ohlcData = await APIService.GetOhlcDataAsync(CurrencyId, "usd", 7);
            if (ohlcData.Count == 0)
                return;

            // Prepare data for candlestick series.
            // LiveCharts CandleSeries expects ChartValues<OhlcPoint>
            var candleValues = new ChartValues<OhlcPoint>();
            var labels = new string[ohlcData.Count];

            for (int i = 0; i < ohlcData.Count; i++)
            {
                var item = ohlcData[i];
                // OhlcPoint takes open, high, low, close as doubles.
                candleValues.Add(new OhlcPoint((double)item.Open, (double)item.High, (double)item.Low, (double)item.Close));
                // Format the timestamp to a readable label.
                labels[i] = DateTimeOffset.FromUnixTimeMilliseconds(item.Timestamp).ToString("MM-dd");
            }

            CandlestickSeries = new SeriesCollection
            {
                new CandleSeries { Values = candleValues }
            };

            Labels = labels;
        }
    }
}
