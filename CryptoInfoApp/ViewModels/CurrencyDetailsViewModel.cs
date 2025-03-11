using CryptoInfoApp.Models;
using CryptoInfoApp.Services;

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

        public async void LoadCurrencyDetails(string id)
        {
            CurrencyDetails = await APIService.GetCurrencyDetailsAsync(id);
        }
    }
}