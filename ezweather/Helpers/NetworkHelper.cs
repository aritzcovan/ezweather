using System;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using NetworkInterface = System.Net.NetworkInformation.NetworkInterface;

namespace ezweather.Helpers
{
    public class NetworkHelper : ViewModelBase
    {
        private readonly DispatcherTimer myDispatcherTimer;
        private Visibility _isNetworkAvailable;

        
        public NetworkHelper()
        {
            myDispatcherTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 2, 0)};
            myDispatcherTimer.Tick += CheckNetwork;
            myDispatcherTimer.Start();
        }

        public void CheckNetwork(object o, EventArgs sender)
        {
            IsNetworkAvailable = NetworkInterface.GetIsNetworkAvailable() ? Visibility.Collapsed : Visibility.Visible;
            RaisePropertyChanged("IsNetworkAvailable");
        }

        public Visibility IsNetworkAvailable
        {
            get { return _isNetworkAvailable; }
            set
            {
                _isNetworkAvailable = value;
                RaisePropertyChanged("IsNetworkAvailable");
            }
        }


    }
}