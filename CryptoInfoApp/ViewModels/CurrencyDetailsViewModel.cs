using CryptoInfoApp.Models;
using CryptoInfoApp.Services;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInfoApp.ViewModels
{
    public class CurrencyDetailsViewModel : BaseViewModel
    {
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

        public CurrencyDetailsViewModel()
        {
        }

        public async void LoadCurrencyDetails(string id)
        {
            CurrencyDetails = await APIService.GetCurrencyDetailsAsync(id);
            await UpdateChartSeriesAsync();
        }

        public async Task UpdateChartSeriesAsync()
        {
            if (CurrencyDetails == null)
                return;

            if (IsCandlestickChart)
            {
                var ohlcData = await APIService.GetOhlcDataAsync(CurrencyDetails.Id, "usd", 1);
                if (ohlcData == null || ohlcData.Count == 0)
                    return;

                var candleValues = new ChartValues<OhlcPoint>();
                var labels = new string[ohlcData.Count];
                for (int i = 0; i < ohlcData.Count; i++)
                {
                    var item = ohlcData[i];
                    candleValues.Add(new OhlcPoint((double)item.Open, (double)item.High, (double)item.Low, (double)item.Close));
                    labels[i] = DateTimeOffset.FromUnixTimeMilliseconds(item.Timestamp).ToString("HH:mm");
                }
                ChartSeries = new SeriesCollection
                {
                    new CandleSeries { Values = candleValues }
                };
                ChartLabels = labels;
            }
            else
            {
                var marketData = await APIService.GetMarketChartDataAsync(CurrencyDetails.Id, "usd", 1);
                if (marketData == null || marketData.Prices == null)
                    return;

                var twentyFourHoursAgo = DateTimeOffset.UtcNow.AddHours(-24).ToUnixTimeMilliseconds();
                var filteredData = marketData.Prices.FindAll(p => p[0] >= twentyFourHoursAgo);

                var lineValues = new ChartValues<double>();
                var labelsList = new List<string>();
                foreach (var point in filteredData)
                {
                    lineValues.Add((double)point[1]);
                    labelsList.Add(DateTimeOffset.FromUnixTimeMilliseconds((long)point[0]).ToString("HH:mm"));
                }
                ChartSeries = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = lineValues,
                        Title = "Price",
                        PointGeometry = DefaultGeometries.Circle,
                        PointGeometrySize = 5
                    }
                };
                ChartLabels = labelsList.ToArray();
            }
        }
    }
}
