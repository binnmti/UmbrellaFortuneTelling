using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Codeplex.Data;
using OpenWeatherMap.Inside;
using Weather;
using System.Configuration;

namespace OpenWeatherMap
{
    public class OpenWeatherMapWeatherReport : IWeatherReport
    {
        private const string OpenWeatherMapUrl = "http://api.openweathermap.org/data/2.5/forecast";
        private const string OpenWeatherMapIconUrl = "http://openweathermap.org/img/w/{0}.png";
        public WeatherReportData ReportData { get; } = new WeatherReportData();

        public UmbrellaData GetUmbrella(DateTime day)
        {
            var days = TodayWeatherData().ToList();
            var fortune = Fortune();
            var percent = (int)((float)days.Count(x => x.Weather.Contains("RAIN")) / days.Count * 100);
            var reverse = IsReverse(percent);
            if (percent > 50)
            {
                percent = reverse ? percent - fortune : percent + fortune;
            }
            else
            {
                percent = reverse ? percent + fortune : percent - fortune;
            }
            percent = Math.Max(percent, 0);
            percent = Math.Min(percent, 100);
            var data = new UmbrellaData
            {
                Is = percent >= 50,
                Percent = percent
            };
            return data;
        }

        public string GetIconUrl(string icon) => string.Format(OpenWeatherMapIconUrl, icon);

        public IEnumerable<WeatherData> TodayWeatherData() => ReportData.WeatherDatas
            .Where(x => x.Date >= DateTime.Now && x.Date <= DateTime.Now.AddDays(1)).ToList();

        public WeatherReportData Update(string place)
        {
            if (string.IsNullOrEmpty(place)) return null;

            var appSettings = ConfigurationManager.AppSettings;
            var accessUrl = $"{OpenWeatherMapUrl}?q={place}&appid={appSettings["OpenWeatherMapApp"]}";
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
                                GetIconUrl(x.weather.ElementAt(0).icon))));
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

        /// <summary>
        /// 占い
        /// </summary>
        /// <returns>基本は高い数ほど低い確率で0 - 50を返す。たまに100</returns>
        private int Fortune()
        {
            //ToDo 乱数を内包しているのもどうだろうか？
            var luck = new Random().Next(100);
            if (luck == 7) return 100;
            for (var i = 0; i < 50; i++)
            {
                Thread.Sleep(1);    //Seedをずらす
                var r = new Random();
                var randomNumber = r.Next(100 - i);
                if (randomNumber <= i)
                {
                    return i;
                }
            }
            return 50;
        }

        /// <summary>
        /// 反転する
        /// 50に近いほど反転しやすく100 or 0に近い場合は反転し辛い
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        private bool IsReverse(int percent)
        {
            //反転するかどうか
            var r = new Random();
            var randomNumber = r.Next(101);
            var baseNumber = Math.Abs((percent - 50) * 2);
            //マイナスなら反転
            return baseNumber - randomNumber < 0;
        }
    }
}