using Serialization;

namespace ezweather.services.Model
{
    [Serializer(typeof(City))]
    public class City : ISerializeObject
    {
        public string CityAndState { get; set; }
        public CityWeather Weather { get; set; }

        public object[] Serialize(object target)
        {
            var city = (City) target;
            return new object[]
                       {
                           city.CityAndState,
                           city.Weather
                       };
        }

        public object Deserialize(object[] data)
        {
            var city = new City()
                           {
                               CityAndState = (string)data[0],
                               Weather = (CityWeather)data[1]
                           };
            return city;
        }
    }
}