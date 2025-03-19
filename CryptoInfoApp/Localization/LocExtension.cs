using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace CryptoInfoApp.Localization
{
    public class LocExtension : MarkupExtension
    {
        public string Key { get; set; }

        public LocExtension() { }

        public LocExtension(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key))
                return string.Empty;

            // Створюємо Binding до індексатора LocalizationProvider.Instance
            var binding = new Binding($"[{Key}]")
            {
                Source = LocalizationProvider.Instance,
                Mode = BindingMode.OneWay
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}
