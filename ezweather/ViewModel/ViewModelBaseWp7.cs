using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using WP7Contrib.Logging;
using WP7Contrib.Services.Navigation;
using WP7Contrib.Services.Storage;

namespace ezweather.ViewModel
{
    /// <summary>
    /// The view model base wp 7.
    /// This VM is a merge of the ViewModelBase from MVVM Light and the Prism example
    /// Need to change the implementation details of this as the DataContract is being enforced which means that all
    /// props and methods need to delcare a body
    /// </summary>
    public abstract class ViewModelBaseWp7 : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private readonly IStorageService _storageService = new NullStorageService();
        protected readonly ILog Log;

        protected ViewModelBaseWp7()
        {
        }

        protected ViewModelBaseWp7(IMessenger messenger)
            : base(messenger)
        {
        }

        protected ViewModelBaseWp7(IMessenger messenger, ILog log)
            : base(messenger)
        {
            Log = log;
        }

        protected ViewModelBaseWp7(INavigationService navigationService, ILog log)
            : this(navigationService, new NullStorageService(), null, log)
        {
        }

        protected ViewModelBaseWp7(INavigationService navigationService, IStorageService storageService, ILog log)
            : this(navigationService, storageService, null, log)
        {
        }

        protected ViewModelBaseWp7(INavigationService navigationService, IMessenger messenger, ILog log)
            : this(navigationService, new NullStorageService(), messenger, log)
        {
        }

        protected ViewModelBaseWp7(INavigationService navigationService, IStorageService storageService, IMessenger messenger, ILog log)
            : base(messenger)
        {
            this._navigationService = navigationService;
            this._storageService = storageService;
            this.Log = log;
        }

        ~ViewModelBaseWp7()
        {
            this.Dispose();
        }


        public void Activate()
        {
            using (var storage = this._storageService.OpenTransient(this.GetType().Name))
            {
                this.IsBeingActivated(storage);
            }
        }

        public void Deactivate()
        {
            using (var storage = this._storageService.OpenTransient(this.GetType().Name))
            {
                this.IsBeingDeactivated(storage);
            }
        }

        public INavigationService NavigationService
        {
            get
            {
                return this._navigationService;
            }
        }

        protected abstract void IsBeingActivated(IStorage storage);
        protected abstract void IsBeingDeactivated(IStorage storage);

        protected void SetPropertyAndNotify<T>(ref T currentValue, T newValue, string proertyName)
        {
            if (Equals(currentValue, newValue))
            {
                return;
            }

            currentValue = newValue;
            this.RaisePropertyChanged(proertyName);
        }
    }
}