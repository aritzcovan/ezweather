using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WP7Contrib.Communications;
using ezweather.services.Model;
using ezweather.services.Resources;

namespace ezweather.services.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IHandleResourcesFactory _handlerFactory;
        private readonly WeatherResult _weatherResult;
        private const string GoogleUrlTemplate = "http://www.google.com/ig/api?weather={0}";

        public WeatherService()
        {
            _handlerFactory = new ResourceClientFactory();
            _weatherResult = new WeatherResult();
        }

        public WeatherService(IHandleResourcesFactory handlerFactory)
        {            
            _handlerFactory = handlerFactory;
            _weatherResult = new WeatherResult();
        }

        public IObservable<WeatherResult> GetWeather(string location)
        {
                 var subject = new Subject<WeatherResult>();
                 var resourceClient = _handlerFactory.Create();
        
                resourceClient.ForType(ResourceType.Xml)
                    .UseUrlForGet(GoogleUrlTemplate)
                    .Get<xml_api_reply>(location)
                    .ObserveOn(Scheduler.ThreadPool)
                    .Catch((Exception ex) => Observable.Empty<xml_api_reply>())
                    .Subscribe(result =>
                                   {
                                       var weatherResult = MapWeatherForecast(result);
                                       weatherResult = MapCurrentConditions(result, weatherResult);
                                       subject.OnNext(weatherResult);
                                   },
                                    subject.OnError,
                                    subject.OnCompleted);
            
            return subject;
        }

        public IObservable<WeatherResult> CheckCity(string location)
        {
            var subject = new Subject<WeatherResult>();
            var resourceClient = _handlerFactory.Create();

            resourceClient.ForType(ResourceType.Xml)
                .UseUrlForGet(GoogleUrlTemplate)
                .Get<xml_api_reply>(location)
                .ObserveOn(Scheduler.ThreadPool)
                .Subscribe(result =>
                {
                    var weatherResult = MapCurrentConditions(result, _weatherResult);
                    subject.OnNext(weatherResult);
                },
                                subject.OnError,
                                subject.OnCompleted);

            return subject;
        }

        public IObservable<CurrentWeatherConditions> GetCurrentConditions(string location)
        {
            var subject = new Subject<CurrentWeatherConditions>();
            var resourceClient = _handlerFactory.Create();

            resourceClient.ForType(ResourceType.Xml)
                .UseUrlForGet(GoogleUrlTemplate)
                .Get<xml_api_reply>(location)
                .ObserveOn(Scheduler.ThreadPool)
                .Subscribe(result =>
                                {
                                   var currCond = MapCurrentConditions(result);
                                   subject.OnNext(currCond);
                                },
                                subject.OnError,
                                subject.OnCompleted);

            return subject;
        }

        private WeatherResult MapWeatherForecast(xml_api_reply result)
        {
            var wr = new WeatherResult();
            
            try
            {
                int count = result.weather[0].forecast_conditions.Count();
                for (int i = 0; i < count; i++)
                {
                    var wc = new WeatherForecast
                                 {
                                     ConditionDescription = result.weather[0].forecast_conditions[i].condition[0].data,
                                     DayOfWeek = result.weather[0].forecast_conditions[i].day_of_week[0].data,
                                     High = Convert.ToDouble(result.weather[0].forecast_conditions[i].high[0].data),
                                     Low = Convert.ToDouble(result.weather[0].forecast_conditions[i].low[0].data),
                                     IconUri = result.weather[0].forecast_conditions[i].icon[0].data.Replace("gif", "png")
                                 };
                    wr.WeatherForecast.Add(wc);
                }
            }
            catch (ArgumentNullException e)
            {      
            }
            return wr;

        }



        private WeatherResult MapCurrentConditions(xml_api_reply r, WeatherResult wr)
        {
            try
            {
                wr.ActualCityName = r.weather[0].forecast_information[0].city[0].data;

                var currentConditions = new CurrentWeatherConditions()
                                            {
                                                City = r.weather[0].forecast_information[0].city[0].data,
                                                PostalCode = r.weather[0].forecast_information[0].postal_code[0].data,
                                                ForecastDate = Convert.ToDateTime(r.weather[0].forecast_information[0].forecast_date[0].data),
                                                CurrentCondition = r.weather[0].current_conditions[0].condition[0].data,
                                                CurrentHumidity = r.weather[0].current_conditions[0].humidity[0].data,
                                                CurrentWind = r.weather[0].current_conditions[0].wind_condition[0].data,
                                                CurrentIconUri = r.weather[0].current_conditions[0].icon[0].data.Replace("gif", "png"),
                                                CurrentF = Convert.ToDouble(r.weather[0].current_conditions[0].temp_f[0].data),
                                                CurrentC = Convert.ToDouble(r.weather[0].current_conditions[0].temp_c[0].data)
                                            };
                wr.WeatherConditions = currentConditions;
            }
            catch (Exception e)
            {
            }
            return wr;
        }

        private CurrentWeatherConditions MapCurrentConditions(xml_api_reply r)
        {
            CurrentWeatherConditions currentConditions = null;
            try
            {
                currentConditions = new CurrentWeatherConditions()
                                            {
                                                City = r.weather[0].forecast_information[0].city[0].data,
                                                PostalCode = r.weather[0].forecast_information[0].postal_code[0].data,
                                                ForecastDate = Convert.ToDateTime(r.weather[0].forecast_information[0].forecast_date[0].data),
                                                CurrentCondition = r.weather[0].current_conditions[0].condition[0].data,
                                                CurrentHumidity = r.weather[0].current_conditions[0].humidity[0].data,
                                                CurrentWind = r.weather[0].current_conditions[0].wind_condition[0].data,
                                                CurrentIconUri = r.weather[0].current_conditions[0].icon[0].data.Replace("gif", "png"),
                                                CurrentF = Convert.ToDouble(r.weather[0].current_conditions[0].temp_f[0].data),
                                                CurrentC = Convert.ToDouble(r.weather[0].current_conditions[0].temp_c[0].data)
                                            };
            }
            catch (Exception e)
            {
            }
            return currentConditions ;
        }
    }
}
