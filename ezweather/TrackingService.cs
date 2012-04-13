using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition.Hosting;
using Google.WebAnalytics;
using Microsoft.WebAnalytics.Hosting;


namespace ezweather
{
    public class TrackingService : IApplicationService
    {
        public void StartService(ApplicationServiceContext context)
        {
            CompositionHostEx.Initialize(
                new AssemblyCatalog(Application.Current.GetType().Assembly),
                new AssemblyCatalog(typeof(Microsoft.WebAnalytics.AnalyticsEvent).Assembly),
                new AssemblyCatalog(typeof(GoogleAnalytics).Assembly));
        }

        public void StopService()
        {
            throw new NotImplementedException();
        }
    }
}
