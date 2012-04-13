using System;
using GoogleAnalyticsTracker;
using Microsoft.Phone.Controls;
using WP7Contrib.View.Transitions.Animation;
using ezweather.Helpers;

namespace ezweather.View
{
    public partial class SettingsView : AnimatedBasePage
    {
        public SettingsView()
        {
            InitializeComponent();
            AnimationContext = LayoutRoot;
            Track();
        }

        private void Track()
        {
            using (var tracker = new Tracker("UA-29860952-1", "ezweather"))
            {
                tracker.TrackPageView("Settings", "Settings");
            }
        }

        protected override AnimatorHelperBase GetAnimation(AnimationType animationType, Uri toOrFrom)
        {
            if (animationType == AnimationType.NavigateForwardOut)
            {
                return new SlideLeftFadeOutAnimator { RootElement = LayoutRoot };
            }

            if (animationType == AnimationType.NavigateBackwardOut)
            {
                return new SlideRightFadeOutAnimator { RootElement = LayoutRoot };
            }

            if (animationType == AnimationType.NavigateForwardIn)
            {
                return new SlideLeftFadeInAnimator { RootElement = LayoutRoot };
            }

            return new SlideRightFadeInAnimator { RootElement = this.LayoutRoot };
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if(NavigationService.CanGoBack)
                NavigationService.GoBack();

            NavigationService.Navigate(new Uri("/View/Main.xaml", UriKind.Relative));
        }
    }
}