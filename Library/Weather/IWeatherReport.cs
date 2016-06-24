using System;

namespace Weather
{
    public interface IWeatherReport
    {
        string Url { get; }

        bool GetUmbrella(DateTime day);

        WeatherReportData ReportData { get; }

        WeatherReportData Update(string place);

    }
}
