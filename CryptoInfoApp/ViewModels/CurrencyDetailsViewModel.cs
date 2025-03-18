using CryptoInfoApp.Models;
using CryptoInfoApp.Services;
using LiveCharts;
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

        // Властивості для графіка
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

        // Метод завантаження деталей валюти
        public async void LoadCurrencyDetails(string id)
        {
            CurrencyDetails = await APIService.GetCurrencyDetailsAsync(id);
            // Після завантаження деталей, завантажимо дані для графіка
            await LoadChartData(id);
        }

        // Метод завантаження даних для графіка (24 годин)
        public async Task LoadChartData(string id)
        {
            // Викликаємо альтернативний endpoint для market_chart (ціни за останній день)
            var marketData = await APIService.GetMarketChartDataAsync(id, "usd", 1);
            if (marketData == null || marketData.Prices == null)
                return;

            // Фільтруємо дані за останні 24 годин
            var fourteenHoursAgo = DateTimeOffset.UtcNow.AddHours(-24).ToUnixTimeMilliseconds();
            var filteredData = marketData.Prices.FindAll(p => p[0] >= fourteenHoursAgo);

            var lineValues = new ChartValues<double>();
            var labels = new List<string>();
            foreach (var point in filteredData)
            {
                // point[0] – timestamp (в мілісекундах), point[1] – ціна
                lineValues.Add((double)point[1]);
                // Форматуємо мітку часу (наприклад, "HH:mm")
                labels.Add(DateTimeOffset.FromUnixTimeMilliseconds((long)point[0]).ToString("HH:mm"));
            }

            ChartSeries = new SeriesCollection
            {
                new LineSeries { Values = lineValues, Title = "Price" }
            };
            ChartLabels = labels.ToArray();
        }
    }
}
