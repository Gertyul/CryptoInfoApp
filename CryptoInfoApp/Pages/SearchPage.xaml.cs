using System.Windows.Controls;
using CryptoInfoApp.ViewModels;

namespace CryptoInfoApp.Pages
{
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as SearchViewModel;
            if (viewModel.SelectedCurrency != null)
            {
                // Навигация на страницу деталей выбранной криптовалюты
                NavigationService.Navigate(new CurrencyDetailsPage(viewModel.SelectedCurrency.Id));
            }
        }
    }
}
