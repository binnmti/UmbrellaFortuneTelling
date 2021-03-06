﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using Codeplex.Data;
using OpenWeatherMap.Inside;
using Weather;

namespace OpenWeatherMap
{
    public class OpenWeatherMapWeatherReport : IWeatherReport
    {
        private const string OpenWeatherMapUrl = "http://api.openweathermap.org/data/2.5/forecast";
        private const string OpenWeatherMapIconUrl = "http://openweathermap.org/img/w/{0}.png";
        private Dictionary<DateTime, Within24HoursWeatherDatas> FotuneDecisionDatas { get; } = new Dictionary<DateTime, Within24HoursWeatherDatas>();

        public List<WeatherReportData> WeatherReportDatas { get; } = new List<WeatherReportData>();

        public IEnumerable<KeyValuePair<DateTime, Within24HoursWeatherDatas>> TodayWeatherDatas() => FotuneDecisionDatas;

        public UmbrellaData GetUmbrella(DateTime day)
        {
            var umbrellaPercent = UmbrellaFortune();
            var data = new UmbrellaData
            {
                Is = umbrellaPercent >= 50,
                Percent = umbrellaPercent
            };
            return data;
        }

        public void Update(string place)
        {
            if (string.IsNullOrEmpty(place)) return;
            SetWeatherReportDatas(place);
            SetFotuneDecisionData();
        }

        private List<WeatherData> GetFirstPlaceTodayData() => WeatherReportDatas[0].WeatherDatas.Where(x => x.Date.Within24Hours()).ToList();

        private void SetFotuneDecisionData()
        {
            var todayData = GetFirstPlaceTodayData();
            for (var i = 0; i < todayData.Count; i++)
            {
                var fotuneData = new Within24HoursWeatherDatas();
                //最初のWeatherDataの時間に、全場所の天気を入れる
                foreach (var data in WeatherReportDatas)
                {
                    var today = data.WeatherDatas.Where(x => x.Date.Within24Hours());
                    fotuneData.Weather.Add(today.ElementAt(i).Weather);
                    fotuneData.Icon.Add(today.ElementAt(i).Icon);
                }
                FotuneDecisionDatas.Add(todayData.ElementAt(i).Date, fotuneData);
            }
        }

        private void SetWeatherReportDatas(string place)
        {
            var appSettings = ConfigurationManager.AppSettings;
            //場所分だけ天気を取得
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
                        WeatherReportDatas.Add(data);
                    }
                }
            }
        }

        private string GetIconUrl(string icon) => string.Format(OpenWeatherMapIconUrl, icon);

        private int UmbrellaFortune()
        {
            var rainPercent = GetRainPercent();
            var fortunePercent = GetFortunePercent();

            var reverse = IsFortuneReverse(rainPercent);
            if (rainPercent > 50)
            {
                rainPercent = reverse ? rainPercent - fortunePercent : rainPercent + fortunePercent;
            }
            else
            {
                rainPercent = reverse ? rainPercent + fortunePercent : rainPercent - fortunePercent;
            }
            rainPercent = Math.Max(rainPercent, 0);
            rainPercent = Math.Min(rainPercent, 100);
            return rainPercent;
        }

        //全体の%ではなくて、1回ずつ出して最大に寄せる。 30%, 70%なら期待値は70%
        private int GetRainPercent()
        {
            return WeatherReportDatas.Max(w =>
            {
                var t = w.WeatherDatas.Where(x => x.Date.Within24Hours());
                return (int)((float)t.Count(r => r.Weather.Contains("RAIN")) / t.Count() * 100);
            });
        }

        /// <summary>
        /// パーセント占い
        /// </summary>
        /// <returns>
        /// 基本は高い数ほど低い確率
        /// 例）0 - 10:40% 11 - 20:30% 21 - 30:20% 31 - 40:8% 41 - 50 2%
        /// </returns>
        private int GetFortunePercent()
        {
            var luck = new Random().Next(100);
            if (luck == 7) return 100;

            var sum = 1;
            var nums = new List<int>();
            for (int i = 0; i <= 50; i++)
            {
                nums.Insert(0, sum);
                if (i % 5 == 0) sum *= 2;
            }
            return RandomUtil.GetRandomIndex(nums.ToArray());
        }

        /// <summary>
        /// 反転占い
        /// 50に近いほど反転しやすく100 or 0に近い場合は反転し辛い
        /// 100 1% 75 25% 50 50% 25 25% 0 1%
        /// </summary>
        /// <param name="rainPercent">雨確率</param>
        /// <returns></returns>
        private bool IsFortuneReverse(int rainPercent)
        {
            //反転するかどうか
            var baseNumber = Math.Abs(rainPercent - 50);
            var r = new Random();
            var randomNumber = r.Next(baseNumber);
            return randomNumber <= 1;
        }
    }
}