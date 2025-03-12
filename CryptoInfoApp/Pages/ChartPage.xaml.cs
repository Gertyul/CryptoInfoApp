using System.Windows.Controls;
using CryptoInfoApp.ViewModels;

namespace CryptoInfoApp.Pages
{
    public partial class ChartPage : Page
    {
        public ChartPage(string currencyId)
        {
            InitializeComponent();
            var vm = DataContext as ChartViewModel;
            vm.CurrencyId = currencyId;
            vm.LoadChartCommand.Execute(null);
        }
    }
}

