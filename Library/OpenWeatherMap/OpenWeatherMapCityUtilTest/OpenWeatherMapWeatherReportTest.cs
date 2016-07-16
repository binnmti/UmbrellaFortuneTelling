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
            _weatherReport.Update("Kato");
            _weatherReport.WeatherReportDatas[0].City.IsNot("Kato");
        }
        [TestMethod]
        public void 有効場所()
        {
            _weatherReport = new OpenWeatherMapWeatherReport();
            _weatherReport.Update("Kyoto");
            _weatherReport.WeatherReportDatas[0].City.Is("Kyoto");
        }
        [TestMethod]
        public void 日づけ取得()
        {
            _weatherReport = new OpenWeatherMapWeatherReport();
            _weatherReport.Update("Kyoto");
            var hit = _weatherReport.WeatherReportDatas
                .FirstOrDefault(x => x.WeatherDatas.ElementAt(0).Date.ToLongDateString() == DateTime.Now.ToLongDateString());
            hit.IsNotNull();
        }

        [TestMethod]
        public void 雨取得()
        {
            _weatherReport = new OpenWeatherMapWeatherReport();
            _weatherReport.Update("kyoto");
            var umbrealla = _weatherReport.GetUmbrella(DateTime.Now);

        }

        [TestMethod]
        public void IsFortuneReverse()
        {
            var w = new OpenWeatherMapWeatherReport();
            //_weatherReport.Update("kyoto");
            w.AsDynamic().IsFortuneReverse(0);
        }

    }
}
