using System;
using System.Device.Location;
using WP7Contrib.Common;

namespace ezweather.Model
{
    public class CurrentWeatherConditions : BaseModel
    {
        public CurrentWeatherConditions()
        {
            _centrePoint = GeoCoordinate.Unknown;
        }

        private GeoCoordinate _centrePoint;
        private string _city;
        private string _postalCode;
        private DateTime _forcastDate;
        private string _currentCondition;
        private double _currentF;
        private double _currentC;
        private string _currentHumidity;
        private string _currentWind;
        private string _currentIconUri;
        
        public string City
        {
            get { return _city; }
            set {SetPropertyAndNotify(ref _city, value, () => City);}
        }
        public string PostalCode
        {
            get { return _postalCode; }
            set {SetPropertyAndNotify(ref _postalCode, value, () => PostalCode);}
        }
        public DateTime ForecastDate
        {
            get { return _forcastDate; }
            set {SetPropertyAndNotify(ref _forcastDate, value, () => ForecastDate);}
        }
        public string CurrentCondition
        {
            get { return _currentCondition; }
            set {SetPropertyAndNotify(ref _currentCondition, value, () => CurrentCondition);}
        }

        public double CurrentTemp
        {
            get
            {
                //if ((App.Current as App).UseImperial)
                    return CurrentF;
                //return CurrentC;
            }
        }

        public double CurrentF
        {
            get { return _currentF; }
            set
            {
                SetPropertyAndNotify(ref _currentF, value, () => CurrentF);
                RaisePropertyChanged("CurrentTemp");
            }
        }
        public double CurrentC
        {
            get { return _currentC; }
            set
            {
                SetPropertyAndNotify(ref _currentC, value, () => CurrentC);
                RaisePropertyChanged("CurrentTemp");
            }
        }
        public string CurrentHumidity
        {
            get { return _currentHumidity; }
            set { SetPropertyAndNotify(ref _currentHumidity, value, () => CurrentHumidity); }
        }
        public string CurrentWind
        {
            get { return _currentWind; }
            set { SetPropertyAndNotify(ref _currentWind, value, () => CurrentWind); }
        }
        public string CurrentIconUri
        {
            get { return _currentIconUri; }
            set { SetPropertyAndNotify(ref _currentIconUri, value, () => CurrentIconUri); }
        }
    }
}
