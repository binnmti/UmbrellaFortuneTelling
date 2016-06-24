using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Codeplex.Data;
using OpenWeatherMap.Inside;

namespace OpenWeatherMap
{
    public static class OpenWeatherMapCityUtil
    {
        private const string JsonFileName = "city.list.json";

        public static IEnumerable<string> GetCountrys() => OpenWeatherMapCountry.Names;

        public static IEnumerable<string> GetCitys(string country) => File.ReadLines(JsonFileName)
            .Where(l => l.Contains($"\"country\":\"{country}\""))
            .Select(line => (OpenWeatherMapJson.City)DynamicJson.Parse(line))
            .Select(json => json.name);

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
