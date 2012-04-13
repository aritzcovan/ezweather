using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace ezweather.Converters
{
    public class HttpPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var retval = String.Format("http://www.google.com{0}", value);
            Debug.WriteLine(retval);

            return retval;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
