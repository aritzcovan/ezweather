using System;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Tasks;
using WP7Contrib.Services.Navigation;
using WP7Contrib.Services.Storage;
using ezweather.Helpers;

namespace ezweather.ViewModel
{
    public class ReviewViewModel : ViewModelBaseWp7
    {
        private readonly INavigationService _navigationService;

        public ReviewViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private RelayCommand _okReviewCommand;
        private RelayCommand _noReviewCommand;
        private RelayCommand _maybeLaterCommand;

        public bool NotAgain { get; set; }

        public RelayCommand OkReviewCommand
        {
            get { return _okReviewCommand ?? (_okReviewCommand = new RelayCommand(DoReview)); }
        }

        public RelayCommand NoReviewCommand
        {
            get { return _noReviewCommand ?? (_noReviewCommand = new RelayCommand(NoReview)); }  
        }

        public RelayCommand MaybeLaterCommand
        {
            get { return _maybeLaterCommand ?? (_maybeLaterCommand = new RelayCommand(MaybeLater)); }
        }

        private void MaybeLater()
        {
            (App.Current as App).NaggedDuringThisRun = true;
            _navigationService.Navigate(new Uri("/View/Main.xaml", UriKind.Relative));
        }

        private void NoReview()
        {
            if(NotAgain)
                ReviewNag.NoMoreNags();
            else
                (App.Current as App).NaggedDuringThisRun = true;    
            

            _navigationService.Navigate(new Uri("/View/Main.xaml", UriKind.Relative));
        }

        private void DoReview()
        {
            ReviewNag.NoMoreNags();
            var mpr = new MarketplaceReviewTask();
            mpr.Show();
        }

        #region Active/Deactive

        protected override void IsBeingActivated(IStorage storage)
        {}

        protected override void IsBeingDeactivated(IStorage storage)
        {}

        #endregion

    }
}