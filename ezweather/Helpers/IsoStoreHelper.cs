using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace ezweather.Helpers
{
    public class IsoStoreHelper
    {
        private static IsolatedStorageFile _isoStore;
        public static IsolatedStorageFile IsoStore
        {
            get { return _isoStore ?? (_isoStore = IsolatedStorageFile.GetUserStoreForApplication()); }
        }

        public static void SaveList<T>(string folderName, string dataName, ObservableCollection<T> dataList) where T : class
        {
            if (!IsoStore.DirectoryExists(folderName))
            {
                IsoStore.CreateDirectory(folderName);
            }

            string fileStreamName = string.Format("{0}\\{1}.dat", folderName, dataName);
            try
            {
                using (var stream = new IsolatedStorageFileStream(fileStreamName, FileMode.Create, IsoStore))
                {
                    var dcs = new DataContractSerializer(typeof(ObservableCollection<T>));
                    dcs.WriteObject(stream, dataList);
                }
            }
            catch (Exception e)
            {
            }
        }

        public static ObservableCollection<T> LoadList<T>(string folderName, string dataName) where T : class
        {
            var retval = new ObservableCollection<T>();

            string fileStreamName = string.Format("{0}\\{1}.dat", folderName, dataName);

            var isf = IsoStore;
            try
            {
                var fileStream = IsoStore.OpenFile(fileStreamName, FileMode.OpenOrCreate);
                if (fileStream.Length > 0)
                {
                    var dcs = new DataContractSerializer(typeof(ObservableCollection<T>));
                    retval = dcs.ReadObject(fileStream) as ObservableCollection<T>;
                }
            }
            catch
            {
                retval = new ObservableCollection<T>();
            }

            return retval;
        }
    }
}