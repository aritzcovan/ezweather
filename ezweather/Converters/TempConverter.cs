using System;
using System.Globalization;
using System.Windows.Data;

namespace ezweather.Converters
{
    public class TempConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((App.Current as App).UseImperial)
                return value;
            
            return ConvertFahrenheitToCelcius(System.Convert.ToDouble(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }


        private double ConvertFahrenheitToCelcius(double fahren)
        {
            return Math.Floor((fahren - 32) * 5 / 9);
        }
    }
}
