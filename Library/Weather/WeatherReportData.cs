using System;
using System.Collections.Generic;

namespace Weather
{
    public class WeatherReportData
    {
        public string Country { get; set; }
        public string City { get; set; }

        public List<WeatherData> WeatherDatas { get; } = new List<WeatherData>();
    }

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