using System.Windows;
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
                NavigationService.Navigate(new CurrencyDetailsPage(viewModel.SelectedCurrency.Id));
            }
        }

        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new SearchPage());
        }

        private void CurrencyConverter_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.CurrencyConverterPage());
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.StartTimer();
            }
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.StopTimer();
            }
        }

    }
}
