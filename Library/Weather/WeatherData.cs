using System;

namespace Weather
{
    public class WeatherData
    {
        public WeatherData(DateTime date, string weather, string icon)
        {
            Date = date;
            Weather = weather;
            Icon = icon;
        }

        public DateTime Date { get; set; }
        public string Weather { get; set; }
        public string Icon { get; set; }
    }
}