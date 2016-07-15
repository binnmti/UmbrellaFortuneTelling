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
using System.Net.Http;

namespace OpenWeatherMap
{
    public class OpenWeatherMapWeatherReport : IWeatherReport
    {
        private const string OpenWeatherMapUrl = "http://api.openweathermap.org/data/2.5/forecast";
        private const string OpenWeatherMapIconUrl = "http://openweathermap.org/img/w/{0}.png";
        public List<WeatherReportData> ReportData { get; } = new List<WeatherReportData>();



        public Dictionary<DateTime, WeatherData2> FotuneDatas { get; } = new Dictionary<DateTime, WeatherData2>();


        public UmbrellaData GetUmbrella(DateTime day)
        {
            var days = TodayWeatherData2().ToList();
            var fortune = Fortune();
            var percent = (int)((float)days.Count(x => x.Value.Weather.Contains("RAIN")) / days.Count * 100);
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

        //public IEnumerable<WeatherData> TodayWeatherData() => ReportData.WeatherDatas
        //    .OrderBy(x => x.Date)
        //    .Where(x => x.Date >= DateTime.Now && x.Date <= DateTime.Now.AddDays(1)).ToList();

        public IEnumerable<KeyValuePair<DateTime, WeatherData2>> TodayWeatherData2() => FotuneDatas;

        public void Update(string place)
        {
            if (string.IsNullOrEmpty(place)) return;

            var appSettings = ConfigurationManager.AppSettings;
            foreach (var p in place.Split('+'))
            {
                using (var wc = new HttpClient())
                {
                    var accessUrl = $"{OpenWeatherMapUrl}?q={p}&appid={appSettings["OpenWeatherMapApp"]}";
                    using (var st = wc.GetStreamAsync(accessUrl))
                    {
                        var json = (OpenWeatherMapJson.Rootobject)DynamicJson.Parse(st.Result);
                        var data = new WeatherReportData()
                        {
                            City = json.city.name,
                            Country = json.city.country,
                        };
                        data.WeatherDatas.AddRange(json.list
                            .Select(x => new WeatherData(DateTime.Parse(x.dt_txt),
                                x.weather.ElementAt(0).description.ToUpper(),
                                GetIconUrl(x.weather.ElementAt(0).icon))));
                        ReportData.Add(data);
                    }
                }
            }
            //同じdateの場合、addするような処理
            var fortuneDatas = ReportData[0].WeatherDatas.Where(x => x.Date >= DateTime.Now && x.Date <= DateTime.Now.AddDays(1));
            for (int i = 0; i < fortuneDatas.Count(); i++)
            {
                var data = new WeatherData2();
                for (var j = 0; j < ReportData.Count; j++)
                {
                    data.Weather.Add(ReportData[j].WeatherDatas[i].Weather);
                    data.Icon.Add(ReportData[j].WeatherDatas[i].Icon);
                }
                FotuneDatas.Add(fortuneDatas.ElementAt(i).Date, data);
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