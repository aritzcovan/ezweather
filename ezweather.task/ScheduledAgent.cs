using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using ezweather.services;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using ezweather.services.Resources;
using System;
using System.Linq;
using ezweather.services.tiles;


namespace ezweather.task {
    public class ScheduledAgent : ScheduledTaskAgent {
        private static volatile bool _classInitialized;
        private string _cityState;
        private int _counter = 0;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent() {
            if (!_classInitialized) {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(
                    delegate { Application.Current.UnhandledException += ScheduledAgent_UnhandledException; });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e) {
            if (System.Diagnostics.Debugger.IsAttached) {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task) {
            var activeTiles = ShellTile.ActiveTiles;

            foreach (var activeTile in activeTiles) {
                if (activeTile.NavigationUri.ToString().Contains("SI")) {
                    //this is a secondary tile which means that it was created by the user and it should have its weather data updated.  

                    var idx = activeTile.NavigationUri.ToString().LastIndexOf('=');
                    _cityState = activeTile.NavigationUri.ToString().Substring(idx + 1);
                    
                    GetWeather(ProcessResult, activeTile.NavigationUri);
                }
            }
        }

        public void GetWeather(Action<string, Uri> result, Uri uri)
        {
            DownloadStringCompletedEventHandler handler = null;
            var client = new WebClient();
            handler = (s, e) =>
            {
                client.DownloadStringCompleted -= handler;
                result(e.Result, uri);
            };

            client.DownloadStringCompleted += handler;
            _counter = _counter + 1;
            client.DownloadStringAsync(LocationUri);
        }

        private void ProcessResult(string data, Uri uri)
        {
            var sr = new StringReader(data);
            var serializer = new XmlSerializer(typeof(xml_api_reply));
            var reply = (xml_api_reply)serializer.Deserialize(sr);

            try {
                
                var useImperial = AppSettingsHelper.ShouldUseImperial();

                var tile = new StandardTileData();
                if (useImperial)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                                  {
                                                                      var ctl = new Snow();
                                                                      ctl.CityState = "Test, NY";
                                                                      ctl.Temp = Convert.ToInt32(reply.weather[0].current_conditions[0].temp_f[0].data);
                                                                      ctl.Measure(new Size(173, 173));
                                                                      ctl.Arrange(new Rect(0, 0, 173, 173));
                                                                      var bmp = new WriteableBitmap(173, 173);
                                                                      bmp.Render(ctl, null);
                                                                      bmp.Invalidate();
                                                                      var iss =IsolatedStorageFile.GetUserStoreForApplication();
                                                                      var filename = "/Shared/ShellContent/tileTest.jpg";
                                                                      using (var stm = iss.CreateFile(filename))
                                                                      {
                                                                          bmp.SaveJpeg(stm, 173, 173, 0, 80);
                                                                      }
                                                                      tile.BackgroundImage = new Uri("isostore:" + filename, UriKind.Absolute);
                                                                      var tileToUpdate = ShellTile.ActiveTiles.FirstOrDefault(r => r.NavigationUri == uri);
                                                                      tileToUpdate.Update(tile);      
                                                                  });
                    
                    //tile.Title = String.Format("{0} {1}", tile.Title, reply.weather[0].current_conditions[0].temp_f[0].data);
                }
                else
                    tile.Title = String.Format("{0} {1}", tile.Title, reply.weather[0].current_conditions[0].temp_c[0].data);

              
                _counter = _counter - 1;
                
                if(_counter == 0)
                    NotifyComplete();

            }
            catch(Exception ex)
            {  
                Debug.WriteLine(ex.Message);
            }
        }


        public Uri LocationUri
        {
            get { return new Uri(string.Format("http://www.google.com/ig/api?weather={0}", _cityState)); }
        }
    }
}