using System;
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
        private const string AppId = "3d9a8eaf26eb211844ea28e7535c8894";

        public OpenWeatherMapWeatherReport(string url)
        {
            Url = url;
        }

        public string Url { get; }
        public WeatherReportData ReportData { get; } = new WeatherReportData();

        public bool GetUmbrella(DateTime day) => ReportData.WeatherDatas.Where(x => x.Date >= day && x.Date <= day.AddDays(1)).Any(x => x.Weather.Contains("RAIN"));

        public WeatherReportData Update(string place)
        {
            if (string.IsNullOrEmpty(place)) return null;

            var accessUrl = $"{Url}?q={place}&appid={AppId}";
            using (var wc = new WebClient())
            {
                try
                {
                    using (var st = wc.OpenRead(accessUrl))
                    {
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