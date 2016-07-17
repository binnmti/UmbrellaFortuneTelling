using System;
using System.Collections.Generic;

namespace Weather
{
    public interface IWeatherReport
    {
        UmbrellaData GetUmbrella(DateTime day);

        List<WeatherReportData> WeatherReportDatas { get; }

        IEnumerable<KeyValuePair<DateTime, Within24HoursWeatherDatas>> TodayWeatherDatas();

        void Update(string place);
    }
}