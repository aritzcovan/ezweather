using System;
using System.Windows;
using System.Linq;
using ezweather.Model;
using ezweather.services;
using GoogleAnalyticsTracker;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using WP7Contrib.View.Transitions.Animation;
using ezweather.Helpers;

namespace ezweather.View {
    public partial class Main : AnimatedBasePage {
        public Main() {
            InitializeComponent();
            //Message.Visibility = Visibility.Collapsed;
            AnimationContext = LayoutRoot;
            Loaded += (s, e) => {
                          using (var tracker = new Tracker("UA-29860952-1", "ezweather")) {
                              tracker.TrackPageView("MainPage", "Main");
                          }
                      };
        }

        protected override AnimatorHelperBase GetAnimation(AnimationType animationType, Uri toOrFrom) {
            if (animationType == AnimationType.NavigateForwardOut) {
                return new SlideLeftFadeOutAnimator {RootElement = LayoutRoot};
            }

            if (animationType == AnimationType.NavigateBackwardOut) {
                return new SlideRightFadeOutAnimator {RootElement = LayoutRoot};
            }

            if (animationType == AnimationType.NavigateForwardIn) {
                return new SlideLeftFadeInAnimator {RootElement = LayoutRoot};
            }

            return new SlideRightFadeInAnimator {RootElement = LayoutRoot};
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
            (App.Current as App).UseImperial = AppSettingsHelper.ShouldUseImperial();

            //check if this person is coming from a pinned tile 
            string val = null;
            NavigationContext.QueryString.TryGetValue("SI", out val);
            if (val != null)
               WeatherPivot.SelectedIndex = Convert.ToInt32(val);

            if (ReviewNag.CheckForNag() && (App.Current as App).NaggedDuringThisRun == false) {
                NavigationService.Navigate(new Uri("/View/ReviewView.xaml", UriKind.Relative));
            }
        }

        private void AboutMenuItem_Click(object sender, EventArgs e) {
            //NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
            NavigationService.Navigate(new Uri("/View/About.xaml", UriKind.Relative));
        }

        private void SettingsMenuItem_Click(object sender, EventArgs e) {
            NavigationService.Navigate(new Uri("/View/SettingsView.xaml", UriKind.Relative));
        }

        private void MyCities_Click(object sender, EventArgs e) {
            NavigationService.Navigate(new Uri("/View/AddCityView.xaml", UriKind.Relative));
        }

        private void PinCity(object sender, EventArgs e) {

            //check to see if there is already a tile for this pivotIndex
            var theTile = ShellTile.ActiveTiles.SingleOrDefault(r => r.NavigationUri.ToString().Contains(WeatherPivot.SelectedIndex.ToString()));
            if (theTile != null)
                return;

            var city = WeatherPivot.SelectedItem as City;
            var tile = new StandardTileData {
                                                Title = city.CityAndState
                                            };
            ShellTile.Create(new Uri(String.Format("/View/Main.xaml?SI={0}&city={1}", WeatherPivot.SelectedIndex, city.CityAndState) , UriKind.Relative), tile);


            ScheduledAction action = null;
            action = ScheduledActionService.Find("ez-weather tile");
            if(action != null)
                ScheduledActionService.Remove("ez-weather tile");

            var t  = new PeriodicTask("ez-weather tile");
            t.Description = "ez-weather tile";
            ScheduledActionService.Add(t);
            ScheduledActionService.LaunchForTest("ez-weather tile",new TimeSpan(0,0,0,2));
        }
    }
}