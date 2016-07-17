using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenWeatherMap
{
    public class RandomUtil
    {
        public static IEnumerable<int> GetFibonacciNumber(int max)
        {
            var fibonacciNumber = new List<long> {0, 1};
            for (int i = 0; i < max - 2; i++)
            {
                var n = fibonacciNumber.Count;
                var f = fibonacciNumber[n - 1] + fibonacciNumber[n - 2];
                fibonacciNumber.Add(f);
            }

            var n2 = fibonacciNumber.Select(x => (x/100 == 0) ? 1 : (int)(x / 100));
            return n2;
        }

        private static int co = 0;

        public static int GetRandomIndex(params int[] weightTable)
        {
            int seed = Environment.TickCount;
            var totalWeight = weightTable.Sum();
            var rnd = new Random(seed + co++);
            var value = rnd.Next(1, totalWeight + 1);
            var retIndex = -1;
            for (var i = 0; i < weightTable.Length; ++i)
            {
                if (weightTable[i] >= value)
                {
                    retIndex = i;
                    break;
                }
                value -= weightTable[i];
            }
            return retIndex;
        }
    }
}