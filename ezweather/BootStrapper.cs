using Funq;
using WP7Contrib.Communications;
using WP7Contrib.Logging;
using WP7Contrib.Messaging;
using ezweather.Helpers;
using ezweather.Services;
using ezweather.ViewModel;
using WP7Contrib.Services.Navigation;
using GalaSoft.MvvmLight.Messaging;
using ezweather.services.Services;

namespace ezweather
{
    public class BootStrapper
    {
        public BootStrapper()
        {
            this.Container = new Container();
            this.ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            Container.Register<IMessenger>(c => new LastMessageReplayMessenger());

            Container.Register<ILog>(c => new DebugLog());
            Container.Register<ILogger>(c => new AMRDebugLog());

            Container.Register<IHandleResourcesFactory>(c => new ResourceClientFactory(c.Resolve<ILog>()));
            Container.Register<INavigationService>(c => new ApplicationFrameNavigationService((App.Current as App).RootFrame));
            Container.Register<IWeatherService>(c => new WeatherService(c.Resolve<IHandleResourcesFactory>()));
            Container.Register<ICityService>(c => new CityService(c.Resolve<IMessenger>()));
            Container.Register(c => new SettingsViewModel(c.Resolve<IMessenger>()));
            Container.Register(c => new ReviewViewModel(c.Resolve<INavigationService>()));
            Container.Register(c => new AddCityViewModel(c.Resolve<ICityService>(), c.Resolve<IMessenger>(), c.Resolve<IWeatherService>()));
            Container.Register(c => new MainViewModel(c.Resolve<ICityService>(), c.Resolve<IWeatherService>(), c.Resolve<IMessenger>()));
        }

        public void Dispose()
        {
            if (this.Container != null)
            {
                this.Container.Dispose();
                this.Container = null;
            }
        }

        public Container Container { get; private set; }
    }
}