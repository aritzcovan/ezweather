using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using ezweather.Helpers;
using ezweather.Messages;
using ezweather.Model;

namespace ezweather.Services
{
    public class CityService : ICityService
    {
        private readonly IMessenger _messenger;

        private ObservableCollection<City> _cities;
        private bool _loaded;
        public CityService(IMessenger messenger)
        {
            _messenger = messenger;
            _cities = new ObservableCollection<City>();
            LoadCities();
        }

        public ObservableCollection<City> Cities
        {
            get
            {
                if (!_loaded)
                    LoadCities();

                if(_cities.Count == 0)
                    AddDefault();

                return _cities;
            }
        }

        private void AddDefault()
        {
            _cities.Add(new City {CityAndState = "Redmond, WA"});
        }

        private void LoadCities()
        {
            _cities = IsoStoreHelper.LoadList<City>("city", "data");
            _loaded = true;
        }

        public void AddCity(City city)
        {
            if (city == null)
                return;

            if (_cities.Contains(city))
                return;

            _cities.Add(city);
            WriteToIsoStore();

        }

        public void RemoveCity(City city)
        {
            if (city == null)
                return;

            if (_cities.Contains(city))
                _cities.Remove(city);

            if (_cities.Count == 0)
            {
                AddDefault();
                _messenger.Send(new RefreshMessage(true));
            }

            WriteToIsoStore();
        }

        private void WriteToIsoStore()
        {
            IsoStoreHelper.SaveList("city", "data", _cities);
        }

        public bool IsCitySaved(City city)
        {
            throw new NotImplementedException();
        }
    }
}
