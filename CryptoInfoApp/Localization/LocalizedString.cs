//using System;
//using System.ComponentModel;
//using System.Globalization;
//using CryptoInfoApp.Resources;

//namespace CryptoInfoApp.Localization
//{
//    public class LocalizedString : INotifyPropertyChanged
//    {
//        public event PropertyChangedEventHandler PropertyChanged;
//        private readonly string _key;

//        public LocalizedString(string key)
//        {
//            _key = key;
//            LocalizationManager.Instance.CultureChanged += OnCultureChanged;
//        }

//        private void OnCultureChanged(object sender, EventArgs e)
//        {
//            // Для відлагодження:
//            // System.Diagnostics.Debug.WriteLine($"Culture changed: {CultureInfo.CurrentUICulture.Name}");
//            OnPropertyChanged(nameof(Value));
//        }

//        public string Value => Resources.Resources.ResourceManager.GetString(_key, CultureInfo.CurrentUICulture);

//        protected void OnPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}
