using System;
using System.Collections.Generic;

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

    public class WeatherData2
    {
        public List<string> Weather { get; set; } = new List<string>();
        public List<string> Icon { get; set; } = new List<string>();
    }

}