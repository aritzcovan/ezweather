using System;
using System.IO.IsolatedStorage;

namespace ezweather.services
{
    public static class AppSettingsHelper
    {
        public static bool ShouldUseImperial()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("ImperialSetting"))
                return Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings["ImperialSetting"]);
            
            IsolatedStorageSettings.ApplicationSettings["ImperialSetting"] = true;
            return true;
        }

        public static bool AllowCrashReports()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("UseAndCrashSetting"))
                return Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings["UseAndCrashSetting"]);

            IsolatedStorageSettings.ApplicationSettings["UseAndCrashSetting"] = true;
            return true;
        }

    }
}
