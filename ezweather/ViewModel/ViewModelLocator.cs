using System;
using System.Collections.Generic;
using Microsoft.Phone.Shell;

namespace ezweather.ViewModel
{
    public class ViewModelLocator : IDisposable
    {
        private readonly BootStrapper _bootStrapper;
        private bool _disposed;
        private IDictionary<Type, ViewModelInstance> _resolvedInstances = new Dictionary<Type, ViewModelInstance>();

        public ViewModelLocator()
        {
            if (_bootStrapper == null)
            {
                _bootStrapper = new BootStrapper();
            }
        }

        public AddCityViewModel AddCityViewModel
        {
            get
            {
                return this.ResolveInstance<AddCityViewModel>();
            }
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return this.ResolveInstance<MainViewModel>();
            }
        }
        
        public ReviewViewModel ReviewViewModel
        {
            get
            {
                return this.ResolveInstance<ReviewViewModel>();
            }
        }

        public SettingsViewModel SettingsViewModel
        {
            get
            {
                return this.ResolveInstance<SettingsViewModel>();
            }
        }



        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this._resolvedInstances.Clear();
                this._resolvedInstances = null;

                this._bootStrapper.Dispose();
            }
            this._disposed = true;
        }

        private T ResolveInstance<T>() where T : ViewModelBaseWp7
        {
            if (this._resolvedInstances.ContainsKey(typeof(T)))
            {
                return (T)this._resolvedInstances[typeof(T)].ViewModel;
            }

            var viewModel = this._bootStrapper.Container.Resolve<T>();
            this._resolvedInstances.Add(viewModel.GetType(), new ViewModelInstance(viewModel));

            return viewModel;
        }

        internal class ViewModelInstance
        {
            public ViewModelBaseWp7 ViewModel { get; private set; }

            public ViewModelInstance(ViewModelBaseWp7 viewModel)
            {
                this.ViewModel = viewModel;

                if (PhoneApplicationService.Current.StartupMode == StartupMode.Activate)
                {
                    this.ViewModel.Activate();
                }

                PhoneApplicationService.Current.Deactivated += this.HandleDeactivation;
            }

            private void HandleDeactivation(object s, DeactivatedEventArgs e)
            {
                this.ViewModel.Deactivate();
            }
        }
    }
}