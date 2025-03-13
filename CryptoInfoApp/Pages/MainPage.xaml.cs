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
                // Navigate to the details page of the selected cryptocurrency
                NavigationService.Navigate(new CurrencyDetailsPage(viewModel.SelectedCurrency.Id));
            }
        }

        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Navigate to the search page
            NavigationService.Navigate(new SearchPage());
        }

        private void ViewChart_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModels.MainViewModel;
            if (viewModel.SelectedCurrency != null)
                NavigationService.Navigate(new ChartPage(viewModel.SelectedCurrency.Id));
                //((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ChartPage(viewModel.SelectedCurrency.Id));
            else
            {
                // Например, выдать сообщение, если не выбрана валюта
                MessageBox.Show("Please select a currency first.");
            }
        }

        private void CurrencyConverter_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.CurrencyConverterPage());
        }
    }
}
