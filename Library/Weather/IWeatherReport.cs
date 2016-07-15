using System;
using System.Collections.Generic;

namespace Weather
{
    public interface IWeatherReport
    {
        UmbrellaData GetUmbrella(DateTime day);

        List<WeatherReportData> ReportData { get; }

        //IEnumerable<WeatherData> TodayWeatherData();
        IEnumerable<KeyValuePair<DateTime, WeatherData2>> TodayWeatherData2();

        void Update(string place);
    }
}