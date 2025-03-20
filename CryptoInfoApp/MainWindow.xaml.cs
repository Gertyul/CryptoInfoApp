using CryptoInfoApp.Localization;
using CryptoInfoApp.Pages;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace CryptoInfoApp
{
    public partial class MainWindow : Window
    {

        private bool isDark = false;
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Pages.MainPage());
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            var dicts = Application.Current.Resources.MergedDictionaries;
            dicts.Clear();
            if (!isDark)
            {
                dicts.Add(new ResourceDictionary() { Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative) });
                isDark = true;
            }
            else
            {
                dicts.Add(new ResourceDictionary() { Source = new Uri("Themes/LightTheme.xaml", UriKind.Relative) });
                isDark = false;
            }
        }
        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            if (menuItem?.Tag is string cultureCode)
            {
                CultureInfo culture = new CultureInfo(cultureCode);
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                this.Language = XmlLanguage.GetLanguage(culture.IetfLanguageTag);

                // Очищення кешу ресурсного менеджера, щоб змінені рядки підвантажилися заново
                CryptoInfoApp.Resources.Resources.ResourceManager.ReleaseAllResources();

                // Оновлюємо локалізовані прив'язки 
                LocalizationProvider.Instance.Refresh();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.MainPage());
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

    }
}
