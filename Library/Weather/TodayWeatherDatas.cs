using System.Collections.Generic;

namespace Weather
{
    //Todo todayというか24時間以内
    public class TodayWeatherDatas
    {
        public List<string> Weather { get; set; } = new List<string>();
        public List<string> Icon { get; set; } = new List<string>();
    }
}