using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenWeatherMap;

namespace OpenWeatherMapCityUtilTest
{
    [TestClass]
    public class RandomUtilTest
    {
        [TestMethod]
        public void TestFibonacciNumber()
        {
            var f = RandomUtil.GetFibonacciNumber(51);
        }
        [TestMethod]
        public void TestFibonacciNumber2()
        {
            var sum = 1;
            var nums = new List<int>();
            for (int i = 0; i < 50; i++)
            {
                nums.Insert(0, sum);
                if(i % 5 == 0) sum *= 2;
            }


            var results = new List<int>();
            for (int i = 0; i < 1000; i++)
            {
                var f = nums;
                //var f = RandomUtil.GetFibonacciNumber(51);
                //var f = Enumerable.Range(1, 51).Reverse().ToArray();
                var n = RandomUtil.GetRandomIndex(f.ToArray());
                results.Add(n);
            }
        }
    }
}
