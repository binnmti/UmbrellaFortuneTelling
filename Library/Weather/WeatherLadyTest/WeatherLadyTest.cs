using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherLady;

namespace WeatherLadyTest
{
    [TestClass]
    public class WeatherLadyTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var data = WeatherLadyUtil.GetData(0);
            Assert.AreEqual(data.Author, "David Photo Studio");
        }
    }
}
