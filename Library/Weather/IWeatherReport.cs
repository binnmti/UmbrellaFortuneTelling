using System;
using System.Collections.Generic;

namespace Weather
{
    public interface IWeatherReport
    {
        string Url { get; }

        UmbrellaData GetUmbrella(DateTime day);

        WeatherReportData ReportData { get; }
        List<WeatherData> TodayWeatherData();

        WeatherReportData Update(string place);

    }
}
