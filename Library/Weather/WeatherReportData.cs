using System;
using System.Collections.Generic;

namespace Weather
{
    public class WeatherReportData
    {
        public string Country { get; set; }
        public string City { get; set; }

        public List<WeatherData> WeatherDatas { get; } = new List<WeatherData>();
        //public Dictionary<DateTime,WeatherData2> WeatherDatas2 { get; } = new Dictionary<DateTime, WeatherData2>();
    }
}