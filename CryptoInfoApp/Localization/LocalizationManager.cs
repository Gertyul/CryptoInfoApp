//using System;
//using System.Globalization;

//namespace CryptoInfoApp.Localization
//{
//    public class LocalizationManager
//    {
//        private static LocalizationManager _instance = new LocalizationManager();
//        public static LocalizationManager Instance => _instance;

//        public event EventHandler CultureChanged;

//        private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

//        public CultureInfo CurrentCulture
//        {
//            get => _currentCulture;
//            set
//            {
//                if (!_currentCulture.Equals(value))
//                {
//                    _currentCulture = value;
//                    // Оновлюємо культуру для поточних потоків
//                    CultureInfo.DefaultThreadCurrentUICulture = value;
//                    CultureChanged?.Invoke(this, EventArgs.Empty);
//                }
//            }
//        }
//    }
//}
