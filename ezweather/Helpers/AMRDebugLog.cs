using System;
using System.Diagnostics;

namespace ezweather.Helpers
{
    public  class AMRDebugLog : ILogger
    {
        private const string DateFormat = "dd-MM-yyyy HH:mm:ss.fff";

        public  void Write(string message)
        {
            Debug.WriteLine("{0} @ {1}", message, DateTime.Now.ToString(DateFormat));
        }

        public void Write(string message, params object[] args)
        {
            var messageWithParameters = string.Format(message, args);
            Debug.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString(DateFormat), messageWithParameters));
        }
    }
}
