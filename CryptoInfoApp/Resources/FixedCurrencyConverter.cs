﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace CryptoInfoApp.Converters
{
    public class FixedCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("C", new CultureInfo("en-US"));
            }
            if (value is double doubleValue)
            {
                return doubleValue.ToString("C", new CultureInfo("en-US"));
            }

            if (value != null && decimal.TryParse(value.ToString(), out decimal parsed))
            {
                return parsed.ToString("C", new CultureInfo("en-US"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
