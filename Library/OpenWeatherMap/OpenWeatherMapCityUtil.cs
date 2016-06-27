using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Codeplex.Data;
using OpenWeatherMap.Inside;
using OpenWeatherMap.Properties;

namespace OpenWeatherMap
{
    public static class OpenWeatherMapCityUtil
    {
        //ToDo 名前としてはjpよりJapanとかにして変換の方がやりたい
        public static IEnumerable<string> GetCountrys() => OpenWeatherMapCountry.Names;

        public static IEnumerable<string> GetCitys(string country) => Resources.city_list.Split('\n')
            .Where(l => l.Contains($"\"country\":\"{country}\""))   //ToDo 高速化だけど美しくない
            .Select(line => (OpenWeatherMapJson.City)DynamicJson.Parse(line))
            .Select(json => json.name)
            .OrderBy(x => x);

        public static string GetCurrentCountry() => GetOsLocale().Substring(3);

        private static string GetOsLocale()
        {
            using (var mc = new System.Management.ManagementClass("Win32_OperatingSystem"))
            {
                using (var moc = mc.GetInstances())
                {
                    foreach (var mo in moc)
                    {
                        return new CultureInfo(GetCulture(mo["Locale"].ToString())).ToString();
                    }
                }
            }
            return string.Empty;
        }

        private static int GetCulture(string locale) => Convert.ToInt32("0x" + locale, 16);
    }
}