using System;
using WP7Contrib.Common;

namespace ezweather.Model
{
    public class WeatherForecast : BaseModel
    {
        private string _dayOfWeek;
        private string _iconUri;
        private string _conditionDescription;
        private double _low;
        private double _high;
        public string DayOfWeek
        {
            get { return _dayOfWeek; }
            set {SetPropertyAndNotify(ref _dayOfWeek, value, () => DayOfWeek);}
        }
        public string IconUri
        {
            get { return _iconUri; }
            set { SetPropertyAndNotify(ref _iconUri, value, () => IconUri); }
        }
        public string ConditionDescription
        {
            get { return _conditionDescription; }
            set { SetPropertyAndNotify(ref _conditionDescription, value, () => ConditionDescription); }
        }
        public double Low
        {
            get
            {
                //if((App.Current as App).UseImperial)
                    return _low;
                
                //return ConvertFahrenheitToCelcius(_low);
            }
            set { SetPropertyAndNotify(ref _low, value, () => Low);}
        }
        public double High
        {
            get
            {
                //if ((App.Current as App).UseImperial)
                    return _high;

                //return ConvertFahrenheitToCelcius(_high);
            }
            set { SetPropertyAndNotify(ref _high, value, () => High);}
        }

        private double ConvertFahrenheitToCelcius(double fahren)
        {
            return Math.Floor((fahren - 32) * 5 / 9);
        }
    }
}