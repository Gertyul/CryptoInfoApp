using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CryptoInfoApp.Models;
using CryptoInfoApp.Services;

namespace CryptoInfoApp.ViewModels
{
    public class ConverterViewModel : BaseViewModel
    {
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

        private decimal _usdEquivalent;
        public decimal USDEquivalent
        {
            get => _usdEquivalent;
            set { _usdEquivalent = value; OnPropertyChanged(); }
        }

        public ICommand ConvertCommand { get; }
        public ICommand LoadCurrenciesCommand { get; }

        public ConverterViewModel()
        {
            ConvertCommand = new RelayCommand(() => ConvertCurrency(), () => FromCurrency != null && ToCurrency != null);
            LoadCurrenciesCommand = new RelayCommand(async () => await LoadCurrencies());
            LoadCurrenciesCommand.Execute(null);
        }

        public async Task LoadCurrencies()
        {
            var currencies = await APIService.GetSimpleCurrenciesAsync(11);
            AllCurrencies = new ObservableCollection<Currency>(currencies);
            OnPropertyChanged(nameof(AllCurrencies));
        }

        public void ConvertCurrency()
        {
            if (FromCurrency != null && ToCurrency != null && ToCurrency.CurrentPrice != 0)
            {
                decimal usdValue = Amount * FromCurrency.CurrentPrice;
                USDEquivalent = usdValue;
                Result = usdValue / ToCurrency.CurrentPrice;
                OnPropertyChanged(nameof(Result));
                OnPropertyChanged(nameof(USDEquivalent));
            }
        }
    }
}