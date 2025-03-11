using System.Windows.Controls;
using CryptoInfoApp.ViewModels;

namespace CryptoInfoApp.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            if (viewModel.SelectedCurrency != null)
            {
                // Навигация на страницу деталей выбранной криптовалюты
                NavigationService.Navigate(new CurrencyDetailsPage(viewModel.SelectedCurrency.Id));
            }
        }

        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Навигация на страницу поиска
            NavigationService.Navigate(new SearchPage());
        }
    }
}
