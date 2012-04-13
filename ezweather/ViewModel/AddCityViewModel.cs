using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using WP7Contrib.Services.Storage;
using GalaSoft.MvvmLight.Command;
using ezweather.Messages;
using ezweather.Services;
using System.Reactive.Linq;
using ezweather.services.Model;
using ezweather.services.Services;
using City = ezweather.Model.City;

namespace ezweather.ViewModel
{
    public class AddCityViewModel : ViewModelBaseWp7
    {
        private City _selectedCity;
        private ObservableCollection<City> _cities;
        private readonly ICityService _cityService;
        private readonly IWeatherService _weatherService;
        private string _location;
        private RelayCommand _addCityCommand;
        private RelayCommand<string> _deleteCityCommand;
        
        public AddCityViewModel(ICityService cityService, IMessenger messenger, IWeatherService weatherService)
            : base(messenger)
        {
            _cityService = cityService;
            _weatherService = weatherService;
        }

        private void SendMessage(bool shouldRefresh)
        {
            MessengerInstance.Send(new RefreshMessage(shouldRefresh));
        }

        private void AddCity()
        {
            GlobalLoading.Instance.IsLoading = true;

            _weatherService.CheckCity(Location)
                .ObserveOnDispatcher()
                .Subscribe(ProcessCheckCityResult, ProcessCheckCityError, CityCheckComplete);
        }
        private void ProcessCheckCityError(Exception ex)
        {
            GlobalLoading.Instance.IsLoading = false;
            Location = string.Empty;
            MessageBox.Show("an error occured, please try again", "oops", MessageBoxButton.OK);
        }
        private void CityCheckComplete()
        {
            GlobalLoading.Instance.IsLoading = false;
        }

        private void ProcessCheckCityResult(WeatherResult result)
        {
            GlobalLoading.Instance.IsLoading = false;
            if (result.ActualCityName == null)
            {
                Location = string.Empty;
                MessageBox.Show("unknown city / postal code", "oops", MessageBoxButton.OK);
            }
            else
            {
                Location = result.ActualCityName;
                var newCity = new City { CityAndState = Location };
                _cityService.AddCity(newCity);
                Location = string.Empty;
                (App.Current as App).Cities = _cities;
                SendMessage(true);
            }
        }

        private void DeleteCity(string cityName)
        {
            if (MessageBox.Show("delete this city?", "delete", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                var city = _cities.FirstOrDefault(c => c.CityAndState == cityName);
                if (city == null)
                    return;

                _cityService.RemoveCity(city);
                _cities.Remove(city);
                (App.Current as App).Cities = _cities;

                SendMessage(false);
            }
        }


        public RelayCommand<string> DeleteCityCommand
        {
            get { return _deleteCityCommand ?? (_deleteCityCommand = new RelayCommand<string>(DeleteCity)); }
        }

        public RelayCommand AddCityCommand
        {
            get { return _addCityCommand ?? (_addCityCommand = new RelayCommand(AddCity)); }
        }

        public string Location
        {
            get { return _location; }
            set { SetPropertyAndNotify(ref _location, value, "Location"); }
        }

        public ObservableCollection<City> Cities
        {
            get { return (_cities = _cityService.Cities); }
        }

        public City SelectedCity
        {
            get { return _selectedCity; }
            set { SetPropertyAndNotify(ref _selectedCity, value, "SelectedCity"); }
        }

        #region Activated/Deactivated

        protected override void IsBeingActivated(IStorage storage)
        {
        }

        protected override void IsBeingDeactivated(IStorage storage)
        {
        }

        #endregion
    }
}