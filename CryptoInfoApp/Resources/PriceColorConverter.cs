using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CryptoInfoApp.Converters
{
    public class PriceColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {

                return decimalValue >= 0 ? Brushes.LimeGreen : Brushes.Red;
            }
            if (value is double doubleValue)
            {
                return doubleValue >= 0 ? Brushes.LimeGreen : Brushes.Red;
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}