using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
            var currencies = await APIService.GetTopCurrenciesAsync(10);
            TopCurrencies = new ObservableCollection<Currency>(currencies);
        }
    }
}