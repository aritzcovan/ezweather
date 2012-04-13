using System;
using GalaSoft.MvvmLight;
using ezweather.Model;

namespace ezweather.Services
{
    public interface IWeatherService
    {
        IObservable<WeatherResult> GetWeather(string location);
        IObservable<WeatherResult> CheckCity(string location);
    }
}