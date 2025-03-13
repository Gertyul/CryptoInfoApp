using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
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

        public MainViewModel()
        {
            LoadTopCurrencies();
        }

        public async Task LoadTopCurrencies()
        {
            try
            {
                var currencies = await APIService.GetTopCurrenciesAsync(10);
                TopCurrencies = new ObservableCollection<Currency>(currencies);
                if (TopCurrencies.Count == 0)
                {
                    MessageBox.Show($"Loaded {currencies.Count} currencies");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading currencies: " + ex.Message);
            }
        }
    }
}