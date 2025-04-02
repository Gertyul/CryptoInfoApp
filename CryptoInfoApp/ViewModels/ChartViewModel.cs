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
        private SeriesCollection _candlestickSeries;
        public SeriesCollection CandlestickSeries
        {
            get => _candlestickSeries;
            set { _candlestickSeries = value; OnPropertyChanged(); }
        }

        private string[] _labels;
        public string[] Labels
        {
            get => _labels;
            set { _labels = value; OnPropertyChanged(); }
        }

        public string CurrencyId { get; set; }

        public ICommand LoadChartCommand { get; }

        public ChartViewModel(string currencyId)
        {
            CurrencyId = currencyId;
            LoadChartCommand = new RelayCommand(async () => await LoadChartData());
        }

        public ChartViewModel() : this("bitcoin") { }

        public async Task LoadChartData()
        {
            var ohlcData = await APIService.GetOhlcDataAsync(CurrencyId, "usd", 1);
            if (ohlcData.Count == 0)
                return;

            var candleValues = new ChartValues<OhlcPoint>();
            var labels = new string[ohlcData.Count];

            for (int i = 0; i < ohlcData.Count; i++)
            {
                var item = ohlcData[i];
                candleValues.Add(new OhlcPoint((double)item.Open, (double)item.High, (double)item.Low, (double)item.Close));
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
