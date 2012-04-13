using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ezweather.services.tiles
{
	public partial class Snow : UserControl, INotifyPropertyChanged
	{
		public Snow()
		{
			// Required to initialize variables
			InitializeComponent();
		}
	    private string _cityState;
	    private int _temp;
	    public string CityState
	    {
            get { return _cityState; }
            set
            {
                _cityState = value;
                RaisePropertyChanged("CityState");
            }
	    }
	    public int Temp
	    {
            get { return _temp; } 
            set 
            { 
                _temp = value;
                RaisePropertyChanged("Temp");
            }
	    }

	    public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string property)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
	}
}