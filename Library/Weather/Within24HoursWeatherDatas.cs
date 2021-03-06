using System;
using System.Collections.Generic;

namespace Weather
{
    //24時間以内の複数個所の天気
    public class Within24HoursWeatherDatas
    {
        public List<string> Weather { get; set; } = new List<string>();
        public List<string> Icon { get; set; } = new List<string>();
    }

    public static class DateTimeExtention
    {
        /// <summary>
        /// 24時間以内かどうか
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool Within24Hours(this DateTime date)
        {
            //DateTime.NowがAzureだと正常に動かない
            var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), myTimeZone);
            return date >= currentDateTime && date < currentDateTime.AddDays(1);
        }
    }
}