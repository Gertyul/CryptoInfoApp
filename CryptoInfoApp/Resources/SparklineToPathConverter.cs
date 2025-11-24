using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace CryptoInfoApp.Converters
{
    public class SparklineToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<double> prices && prices.Count > 0)
            {
                return CreatePathGeometry(prices);
            }
            if (value is List<decimal> decimalPrices && decimalPrices.Count > 0)
            {
                var doubles = decimalPrices.Select(d => (double)d).ToList();
                return CreatePathGeometry(doubles);
            }
            return null;
        }

        private Geometry CreatePathGeometry(List<double> prices)
        {
            double min = prices.Min();
            double max = prices.Max();
            double range = max - min;
            if (range == 0) range = 1;

            var geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new System.Windows.Point(0, -(prices[0] - min)), false, false);

                for (int i = 1; i < prices.Count; i++)
                {
                    context.LineTo(new System.Windows.Point(i, -(prices[i] - min)), true, true);
                }
            }
            return geometry;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}