using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CryptoInfoApp.Models;
using CryptoInfoApp.Services;

namespace CryptoInfoApp.ViewModels
{
    public class ConverterViewModel : BaseViewModel
    {
        // List of currencies to choose from (for simplicity we use top 50)
        public ObservableCollection<Currency> AllCurrencies { get; set; }

        private Currency _fromCurrency;
        public Currency FromCurrency
        {
            get => _fromCurrency;
            set { _fromCurrency = value; OnPropertyChanged(); }
        }

        private Currency _toCurrency;
        public Currency ToCurrency
        {
            get => _toCurrency;
            set { _toCurrency = value; OnPropertyChanged(); }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(); }
        }

        private decimal _result;
        public decimal Result
        {
            get => _result;
            set { _result = value; OnPropertyChanged(); }
        }

        public ICommand ConvertCommand { get; }
        public ICommand LoadCurrenciesCommand { get; }

        public ConverterViewModel()
        {
            ConvertCommand = new RelayCommand(() => ConvertCurrency(), () => FromCurrency != null && ToCurrency != null);
            LoadCurrenciesCommand = new RelayCommand(async () => await LoadCurrencies());
            // Load the list of currencies.
            LoadCurrenciesCommand.Execute(null);
        }

        public async Task LoadCurrencies()
        {
            // Get top 50 currencies (for demo purposes)
            var currencies = await APIService.GetTopCurrenciesAsync(50);
            AllCurrencies = new ObservableCollection<Currency>(currencies);
            OnPropertyChanged(nameof(AllCurrencies));
        }

        public void ConvertCurrency()
        {
            // Assuming both currencies have their price in USD.
            // Conversion: amount in USD = Amount * FromCurrency.CurrentPrice,
            // then result = (amount in USD) / ToCurrency.CurrentPrice.
            if (FromCurrency != null && ToCurrency != null && ToCurrency.CurrentPrice != 0)
            {
                decimal usdValue = Amount * FromCurrency.CurrentPrice;
                Result = usdValue / ToCurrency.CurrentPrice;
                OnPropertyChanged(nameof(Result));
            }
        }
    }
}