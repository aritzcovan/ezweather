namespace ezweather.Messages
{
    public class CityNameMessage
    {
        public CityNameMessage(string cityName)
        {
            this.CityName = cityName;
        }
        public string CityName { get; set; }     
    }
}