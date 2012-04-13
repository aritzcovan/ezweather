using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WP7Contrib.Services.Storage;
using GalaSoft.MvvmLight.Command;
using WP7Contrib.Services.Navigation;
using ezweather.Model;
using System.Reactive.Linq;
using ezweather.Services;

namespace ezweather.ViewModel
{
    public class WeatherViewModel : ViewModelBaseWp7
    {
        private readonly INavigationService _navigationService;
        private readonly IWeatherService _service;
        private readonly ICityService _cityService;
        private ObservableCollection<City> _cities; 
        private ObservableCollection<WeatherForecast> _weatherForecast;
        private CurrentWeatherConditions _currentConditions;
        public RelayCommand ToAddCityCommand { get; private set; }
        private City _selectedCity;


        public WeatherViewModel(INavigationService navigation, IWeatherService service, ICityService cityService)
        {
            _navigationService = navigation;
            _service = service;
            _cityService = cityService;
            _weatherForecast = new ObservableCollection<WeatherForecast>();
            _currentConditions = new CurrentWeatherConditions();

            ToAddCityCommand = new RelayCommand(() => _navigationService.Navigate(new Uri("/View/AddCityView.xaml", UriKind.Relative)));
            LoadCities();
            //load the weather
            LoadWeather();
            
        }

        public City SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                SetPropertyAndNotify(ref _selectedCity, value, "SelectedCity");
                LoadWeather();
            }
        }

        private void LoadCities()
        {
            _cities = _cityService.Cities;
            if (_cities.Count == 0)
            {
                //there are no cities, setup with the defaults
                _cityService.AddCity(new City { CityAndState = "New York, NY"});
                _cityService.AddCity(new City { CityAndState = "Chicago, IL"});
                _cities = _cityService.Cities;
            }
            _selectedCity = _cities.First();
        }

        public ObservableCollection<City> Cities
        {
            get { return _cities; }
        }

        public ObservableCollection<WeatherForecast> WeatherForecast
        {
            get { return _weatherForecast; }
            set
            {
                SetPropertyAndNotify(ref _weatherForecast, value, "WeatherForecast");
            }
        }

        public CurrentWeatherConditions CurrentConditions
        {
            get { return _currentConditions; }
            private set
            {
                SetPropertyAndNotify(ref _currentConditions, value, "CurrentConditions");
            }
        }

        private void LoadWeather()
        {
            _weatherForecast.Clear();

            _service.GetWeather(_selectedCity.CityAndState)
                .ObserveOnDispatcher()
                .Subscribe(result =>
                               {
                                   WeatherForecast = result.WeatherForecast;
                                   CurrentConditions = result.WeatherConditions;
                               });
        }

        #region Activate/Deactivate
        

        protected override void IsBeingActivated(IStorage storage)
        {
            //todo code to hydrate
        }

        protected override void IsBeingDeactivated(IStorage storage)
        {
            //todo code to dehydrate
        }

        #endregion
    }
}