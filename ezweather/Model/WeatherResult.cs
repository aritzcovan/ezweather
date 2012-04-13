using System.Collections.ObjectModel;
using WP7Contrib.Common;
using ezweather.services.Model;

namespace ezweather.Model
{
    public class WeatherResult : BaseModel
    {
        private ObservableCollection<WeatherForecast> _forecast;
        private CurrentWeatherConditions _weatherConditions;
        private string _actualCityName;

        public WeatherResult()
        {
            _forecast = new ObservableCollection<WeatherForecast>();
            _weatherConditions = new CurrentWeatherConditions();
        }

        public ObservableCollection<WeatherForecast> WeatherForecast
        {
            get { return _forecast; }
            set
            {
                SetPropertyAndNotify(ref  _forecast, value, "WeatherForecast");
            }
        }
        public CurrentWeatherConditions WeatherConditions
        {
            get { return _weatherConditions; }
            set
            {
                SetPropertyAndNotify(ref _weatherConditions, value, "WeatherConditions");
            }
        }
        public string ActualCityName
        {
            get { return _actualCityName; }
            set
            {
                SetPropertyAndNotify(ref _actualCityName, value, "ActualCityName");
            }
        }

    }
}
