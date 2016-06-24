using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Weather;

namespace WeatherTest
{
    [TestClass]
    public class OpenWeatherMapCityUtilTest
    {
        [TestMethod]
        public void 現在のOSの国取得()
        {
            OpenWeatherMapCityUtil.GetCurrentCountry().Is("JP");
        }
        [TestMethod]
        public void 現在の町取得()
        {
            var country = OpenWeatherMapCityUtil.GetCurrentCountry();
            var citys = OpenWeatherMapCityUtil.GetCitys(country);
            citys.Count().IsNot(0);
        }
    }
}
