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
                // Navigate to the details page of the selected cryptocurrency
                NavigationService.Navigate(new CurrencyDetailsPage(viewModel.SelectedCurrency.Id));
            }
        }
    }
}
