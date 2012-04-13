using System;
using GoogleAnalyticsTracker;
using WP7Contrib.View.Transitions.Animation;

namespace ezweather.View
{
    public partial class AddCityView : AnimatedBasePage
    {
        public AddCityView()
        {
            InitializeComponent();
            AnimationContext = LayoutRoot;
            Track();
        }

        private void Track()
        {
            using (var tracker = new Tracker("UA-29860952-1", "ezweather"))
            {
                tracker.TrackPageView("AddCityView", "AddCityView");
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

        private void btnDone_Click(object sender, System.EventArgs e)
        {
            if(NavigationService.CanGoBack)
                NavigationService.GoBack();

        	NavigationService.Navigate(new Uri("/View/Main.xaml", UriKind.Relative));
        }
    }
}