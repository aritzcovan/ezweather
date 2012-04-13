using System;
using System.IO.IsolatedStorage;

namespace ezweather.Helpers
{
    public class ReviewNag
    {
        internal static void IncrementRunCount()
        {
            int runCount;
            var isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
            if (isolatedStorageSettings.Contains("runCount"))
            {
                runCount = Convert.ToInt32(isolatedStorageSettings["runCount"]);
                runCount += 1;
            }
            else
            {
                runCount = 1;
                isolatedStorageSettings["allowNag"] = "yes";
            }

            isolatedStorageSettings["runCount"] = runCount;
        }


        internal static bool CheckForNag()
        {
            var isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
            var numRuns = Convert.ToInt32(isolatedStorageSettings["runCount"]);
            
            if ( numRuns >= 10 &&
                isolatedStorageSettings["allowNag"].ToString().Equals("yes") &&
                numRuns % 2 == 0)
                return true;
            
            return false;
        }

        internal static void NoMoreNags()
        {
            var isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
            isolatedStorageSettings["allowNag"] = "no";
        }
      }   
    }