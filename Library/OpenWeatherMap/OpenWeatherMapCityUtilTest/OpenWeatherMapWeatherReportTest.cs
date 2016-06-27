using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenWeatherMap;
using Weather;

namespace OpenWeatherMapCityUtilTest
{
    [TestClass]
    public class OpenWeatherMapWeatherReportTest
    {
        private static IWeatherReport _weatherReport;

        [TestMethod]
        public void 無効場所()
        {
            _weatherReport = new OpenWeatherMapWeatherReport();
            var data = _weatherReport.Update("Kato");
            data.City.IsNot("Kato");
        }
        [TestMethod]
        public void 有効場所()
        {
            _weatherReport = new OpenWeatherMapWeatherReport();
            var data = _weatherReport.Update("Kyoto");
            data.Country.Is("JP");
            data.City.Is("Kyoto");
        }
        [TestMethod]
        public void 日づけ取得()
        {
            _weatherReport = new OpenWeatherMapWeatherReport();
            var data = _weatherReport.Update("kyoto");
            var hit = data.WeatherDatas.FirstOrDefault(x => x.Date.ToLongDateString() == DateTime.Now.ToLongDateString());
            hit.IsNotNull();
        }
        [TestMethod]
        public void 雨取得()
        {
            _weatherReport = new OpenWeatherMapWeatherReport();
            _weatherReport.Update("kyoto");
            var umbrealla = _weatherReport.GetUmbrella(DateTime.Now);

        }

    }
}
