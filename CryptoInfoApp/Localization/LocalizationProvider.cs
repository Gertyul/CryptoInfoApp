using System.ComponentModel;
using System.Globalization;
using CryptoInfoApp.Resources;

namespace CryptoInfoApp.Localization
{
    public class LocalizationProvider : INotifyPropertyChanged
    {
        public static LocalizationProvider Instance { get; } = new LocalizationProvider();

        public event PropertyChangedEventHandler PropertyChanged;

        // Індексатор: повертає локалізований рядок за ключем із ресурсного файлу,
        // використовуючи поточну CultureInfo.CurrentUICulture.
        public string this[string key]
        {
            get
            {
                return Resources.Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);
            }
        }

        // Викликаємо цей метод після зміни культури, щоб оновити прив’язки.
        public void Refresh()
        {
            // Оновлюємо всі прив’язки, що використовують індексатор ("Item[]" – стандартна назва для індексатора)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }
    }
}
