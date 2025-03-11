using System.Windows.Controls;
using CryptoInfoApp.ViewModels;
using System.Diagnostics;

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

        private void OpenInBrowser_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var viewModel = DataContext as CurrencyDetailsViewModel;
            if (!string.IsNullOrEmpty(viewModel.CurrencyDetails?.Homepage))
            {
                Process.Start(new ProcessStartInfo(viewModel.CurrencyDetails.Homepage) { UseShellExecute = true });
            }
        }
    }
}
