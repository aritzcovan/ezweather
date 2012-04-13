using System;
using System.IO.IsolatedStorage;
using GalaSoft.MvvmLight.Messaging;
using WP7Contrib.Services.Storage;
using ezweather.Messages;

namespace ezweather.ViewModel
{
    public class SettingsViewModel : ViewModelBaseWp7
    {
        
        private IsolatedStorageSettings settings;

        private const string ImperialSettingKeyName = "ImperialSetting";
        private const string UseAndCrashKeyName = "UseAndCrashSetting";

        private const bool ImperialSettingDefault = true;
        private const bool UseAndCrashSettingDefault = true;

        public SettingsViewModel(IMessenger messenger) : base(messenger)
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        public void Save()
        {
            settings.Save();
        }

        public string ImperialSettingText
        {
            get { return ImperialSetting == true ? "Farenheit" : "Celcius"; }
        }

        public string UseAndCrashText
        {
            get { return UseAndCrashSetting == true ? "On" : "Off"; }
        }

        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
                // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }


        public bool ImperialSetting
        {
            get
            {
                return GetValueOrDefault<bool>(ImperialSettingKeyName, ImperialSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(ImperialSettingKeyName, value))
                {
                    Save();
                    RaisePropertyChanged("ImperialSettingText");
                    (App.Current as App).UseImperial = value;
                    var vml = new ViewModelLocator();
                    vml.MainViewModel.Cities = (App.Current as App).Cities;
                    MessengerInstance.Send(new RefreshMessage(true));
                }
            }
        }
        
        public bool UseAndCrashSetting
        {
            get
            {
                return GetValueOrDefault<bool>(UseAndCrashKeyName, UseAndCrashSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(UseAndCrashKeyName, value))
                {
                    Save();
                    RaisePropertyChanged("UseAndCrashText");
                    (App.Current as App).SendUseAndCrashData = value;
                }
            }
        }


        #region Active/Deactive

        protected override void IsBeingActivated(IStorage storage)
        {
        }

        protected override void IsBeingDeactivated(IStorage storage)
        {
        }

        #endregion

    }
}
