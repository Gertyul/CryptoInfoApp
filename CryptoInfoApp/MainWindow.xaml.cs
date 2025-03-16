using CryptoInfoApp.Pages;
using System;
using System.Globalization;
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
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {

            // Remove the current theme dictionary and add the other one.
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
                // Set the culture for the application.
                CultureInfo culture = new CultureInfo(cultureCode);
                // Change the language for the current thread
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                // Update the language for the application resources.
                this.Language = XmlLanguage.GetLanguage(culture.IetfLanguageTag);
                // Optionally force reload of windows or use a localization framework.
                MessageBox.Show($"Language changed to {culture.DisplayName}. Restart the app to see all changes.");
            }
        }

    }
}
