using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CryptoInfoApp.Models;
using CryptoInfoApp.Services;

namespace CryptoInfoApp.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Currency> _searchResults;
        public ObservableCollection<Currency> SearchResults
        {
            get => _searchResults;
            set { _searchResults = value; OnPropertyChanged(); }
        }

        private Currency _selectedCurrency;
        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set { _selectedCurrency = value; OnPropertyChanged(); }
        }

        public ICommand SearchCommand { get; }

        public SearchViewModel()
        {
            SearchCommand = new RelayCommand(async () => await ExecuteSearch());
        }

        public async Task ExecuteSearch()
        {
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var results = await APIService.SearchCurrencyAsync(SearchQuery);
                SearchResults = new ObservableCollection<Currency>(results);
            }
        }
    }
}
