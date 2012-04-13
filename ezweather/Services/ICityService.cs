using System.Collections.ObjectModel;
using ezweather.Model;

namespace ezweather.Services
{
    public interface ICityService
    {
        ObservableCollection<City> Cities { get; }
        void AddCity(City city);
        void RemoveCity(City city);
        bool IsCitySaved(City city);
    }
}