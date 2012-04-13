using System;
using Microsoft.Phone.Controls;
using WP7Contrib.View.Transitions.Animation;

namespace ezweather.View
{
    
    public partial class ReviewView : AnimatedBasePage
    {    
        public ReviewView()
        {
            InitializeComponent();
            AnimationContext = LayoutRoot;
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
    }
}