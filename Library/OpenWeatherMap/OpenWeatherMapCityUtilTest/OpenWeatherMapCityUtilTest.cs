using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenWeatherMap;

namespace OpenWeatherMapCityUtilTest
{
    [TestClass]
    public class OpenWeatherMapCityUtilTest
    {
        [TestMethod]
        public void 現在のosの国()
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
