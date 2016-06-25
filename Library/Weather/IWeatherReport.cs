using System;
using System.Collections.Generic;

namespace Weather
{
    public interface IWeatherReport
    {
        UmbrellaData GetUmbrella(DateTime day);

        WeatherReportData ReportData { get; }

        IEnumerable<WeatherData> TodayWeatherData();

        WeatherReportData Update(string place);
    }
}