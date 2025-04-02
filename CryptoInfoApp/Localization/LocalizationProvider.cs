using System.ComponentModel;
using System.Globalization;
using CryptoInfoApp.Resources;

namespace CryptoInfoApp.Localization
{
    public class LocalizationProvider : INotifyPropertyChanged
    {
        public static LocalizationProvider Instance { get; } = new LocalizationProvider();

        public event PropertyChangedEventHandler PropertyChanged;

        public string this[string key]
        {
            get
            {
                return Resources.Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);
            }
        }

        public void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }
    }
}
