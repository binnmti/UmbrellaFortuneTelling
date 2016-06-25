using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Codeplex.Data;
using OpenWeatherMap.Inside;
using Weather;

namespace OpenWeatherMap
{
    public class OpenWeatherMapWeatherReport : IWeatherReport
    {
        private const string OpenWeatherMapUrl = "http://api.openweathermap.org/data/2.5/forecast";
        private const string AppId = "3d9a8eaf26eb211844ea28e7535c8894";
        public WeatherReportData ReportData { get; } = new WeatherReportData();

        public UmbrellaData GetUmbrella(DateTime day)
        {
            var days = TodayWeatherData().ToList();
            var data = new UmbrellaData
            {
                Is = days.Any(x => x.Weather.Contains("RAIN")),
                Percent = (int)((float)days.Count(x => x.Weather.Contains("RAIN")) / days.Count * 100)
            };
            return data;
        }

        public IEnumerable<WeatherData> TodayWeatherData() => ReportData.WeatherDatas
            .Where(x => x.Date >= DateTime.Now && x.Date <= DateTime.Now.AddDays(1)).ToList();

        public WeatherReportData Update(string place)
        {
            if (string.IsNullOrEmpty(place)) return null;

            var accessUrl = $"{OpenWeatherMapUrl}?q={place}&appid={AppId}";
            using (var wc = new WebClient())
            {
                try
                {
                    using (var st = wc.OpenRead(accessUrl))
                    {
                        if (st == null) return null;
                        using (var sr = new StreamReader(st))
                        {
                            var html = sr.ReadToEnd();
                            var json = (OpenWeatherMapJson.Rootobject)DynamicJson.Parse(html);

                            ReportData.City = json.city.name;
                            ReportData.Country = json.city.country;
                            ReportData.WeatherDatas.AddRange(json.list
                                .Select(x => new WeatherData(DateTime.Parse(x.dt_txt),
                                x.weather.ElementAt(0).description.ToUpper(),
                                x.weather.ElementAt(0).icon)));
                            return ReportData;
                        }
                    }
                }
                //存在しないURLなど
                catch (WebException)
                {
                    return null;
                }
            }
        }
    }
}