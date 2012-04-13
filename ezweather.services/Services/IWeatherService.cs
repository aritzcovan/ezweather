using System;
using ezweather.services.Model;

namespace ezweather.services.Services
{
    public interface IWeatherService
    {
        IObservable<WeatherResult> GetWeather(string location);
        IObservable<WeatherResult> CheckCity(string location);
    }
}