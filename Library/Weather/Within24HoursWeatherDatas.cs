using System;
using System.Collections.Generic;

namespace Weather
{
    //24ŠÔˆÈ“à‚Ì•¡”ŒÂŠ‚Ì“V‹C
    public class Within24HoursWeatherDatas
    {
        public List<string> Weather { get; set; } = new List<string>();
        public List<string> Icon { get; set; } = new List<string>();
    }

    public static class DateTimeExtention
    {
        /// <summary>
        /// 24ŠÔˆÈ“à‚©‚Ç‚¤‚©
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool Within24Hours(this DateTime date)
        {
            //DateTime.Now‚ªAzure‚¾‚Æ³í‚É“®‚©‚È‚¢
            var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), myTimeZone);
            return date >= currentDateTime && date < currentDateTime.AddDays(1);
        }
    }
}