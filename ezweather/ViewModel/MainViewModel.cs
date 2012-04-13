using System;
using System.Collections.ObjectModel;
using ezweather.services;
using GalaSoft.MvvmLight.Messaging;
using WP7Contrib.Services.Storage;
using ezweather.Helpers;
using ezweather.Messages;
using ezweather.Model;
using ezweather.Services;
using ezweather.services.Services;

namespace ezweather.ViewModel
{
    public class MainViewModel : ViewModelBaseWp7
    {
        private readonly ICityService _cityService;
        private readonly IWeatherService _weatherService;
        private readonly IMessenger _messenger;

        public MainViewModel(ICityService cityService, IWeatherService weatherService, IMessenger messenger)
        {
            IncrementRunCount();
            CheckForPreviousException();
            _cityService = cityService;
            _weatherService = weatherService;
            _messenger = messenger;
            _messenger.Register<RefreshMessage>(this, HandleRefreshMessage);
            Cities = _cityService.Cities;
            LoadWeather();
        }

        
        private void IncrementRunCount()
        {
            ReviewNag.IncrementRunCount();
        }

        private void CheckForPreviousException()
        {
            if(AppSettingsHelper.AllowCrashReports())
                LittleWatson.CheckForPreviousException();
        }

        private void HandleRefreshMessage(RefreshMessage message)
        {
            if (message.ShouldRefresh)
                LoadWeather();

            RaisePropertyChanged("Cities");
        }

        private void LoadWeather()
        {
            try
            {
                foreach (var city in Cities)
                {
                    var cw = new CityWeather(city.CityAndState, _weatherService);
                    city.Weather = cw;
                }
            }
            catch (Exception)
            {
            }
        }    

        public ObservableCollection<City> Cities
        {
            get { return (App.Current as App).Cities; }
            set { (App.Current as App).Cities = value; }
        }




        #region active/deactive

        protected override void IsBeingActivated(IStorage storage)
        {
        }

        protected override void IsBeingDeactivated(IStorage storage)
        {
        }

        #endregion

    }
}