using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using WP7Contrib.Common;
using ezweather.services.Services;
using ezweather.services.Model;

namespace ezweather.Model
{
    [DataContract]
    public class CityWeather : BaseModel
    {
        private readonly IWeatherService _weatherService;
        private CurrentWeatherConditions _wc;
        private ObservableCollection<WeatherForecast> _wf; 
        private  string _cityAndState;
        
        public CityWeather(string cityAndState, IWeatherService weatherService)
        {
            _wf = new ObservableCollection<WeatherForecast>();
            _wc = new CurrentWeatherConditions();
            _weatherService = weatherService;
            _cityAndState = cityAndState;
            LoadWeather();
        }

        private void LoadWeather()
        {
            _weatherService.GetWeather(_cityAndState)
                .ObserveOnDispatcher()
                .Subscribe(result =>
                                {
                                    Forecast = result.WeatherForecast;
                                    CurrentWeather = result.WeatherConditions;
                                });
        }

        [DataMember]
        public string CityAndState
        {
            get { return _cityAndState; }
            set { _cityAndState = value; }
        }

        public ObservableCollection<WeatherForecast> Forecast
        {
            get { return _wf; }
            private set
            {
                SetPropertyAndNotify(ref _wf, value, "Forecast");
            }
        }

        public CurrentWeatherConditions CurrentWeather
        {
            get { return _wc; }
            private set
            {
                SetPropertyAndNotify(ref _wc, value, "CurrentWeather");
            }
        }
    }
}