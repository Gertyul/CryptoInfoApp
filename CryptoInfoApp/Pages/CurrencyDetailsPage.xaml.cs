using System.Windows.Controls;
using CryptoInfoApp.ViewModels;
using System.Diagnostics;
using System.Windows;

namespace CryptoInfoApp.Pages
{
    public partial class CurrencyDetailsPage : Page
    {
        public CurrencyDetailsPage(string currencyId)
        {
            InitializeComponent();
            var viewModel = DataContext as CurrencyDetailsViewModel;
            viewModel.LoadCurrencyDetails(currencyId);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CurrencyDetailsViewModel vm)
            {
                vm.StartTimer();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CurrencyDetailsViewModel vm)
            {
                vm.StopTimer();
            }
        }

        private void OpenInBrowser_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CurrencyDetailsViewModel;
            if (!string.IsNullOrEmpty(viewModel.CurrencyDetails?.Homepage))
            {
                Process.Start(new ProcessStartInfo(viewModel.CurrencyDetails.Homepage) { UseShellExecute = true });
            }
        }
    }
}

