using System;
using ezweather.Helpers;
using GoogleAnalyticsTracker;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using WP7Contrib.View.Transitions.Animation;

namespace ezweather.View
{
    /// <summary>
    /// Description for About.
    /// </summary>
    public partial class About : AnimatedBasePage
    {
        /// <summary>
        /// Initializes a new instance of the About class.
        /// </summary>
        public About()
        {
            InitializeComponent();
            AnimationContext = LayoutRoot;

            Loaded += (s, e) =>
            {
                using (var tracker = new Tracker("UA-29860952-1", "ezweather"))
                {
                    tracker.TrackPageView("AboutPage", "About");
                }
            };
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

            return new SlideRightFadeInAnimator { RootElement = LayoutRoot };
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReviewNag.NoMoreNags();
            var task = new MarketplaceReviewTask();
            task.Show();
        }
    }
}