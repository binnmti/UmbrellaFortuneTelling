using System.Collections.Generic;
using System.Linq;

namespace WeatherLady
{
    public class WeatherLadyUtil
    {
        private static readonly IEnumerable<WeatherLadyData> WeatherLadyDatas = new List<WeatherLadyData>()
        {
            new WeatherLadyData
            {
                Author = "David Photo Studio",
                FileName = "Need.png",
                Quotation = "https://www.flickr.com/photos/jackal1203/14801855910/"
            },
            new WeatherLadyData
            {
                Author = "David Photo Studio",
                FileName = "Need.png",
                Quotation = "https://www.flickr.com/photos/jackal1203/14801855910/"
            },
            new WeatherLadyData
            {
                Author = "gregt99",
                FileName = "Need2.png",
                Quotation = "https://www.flickr.com/photos/80592618@N06/16118194721/in/set-72157649551014970/"
            },
            new WeatherLadyData
            {
                Author = "gregt99",
                FileName = "Need3.png",
                Quotation = "https://www.flickr.com/photos/80592618@N06/15527469693/in/set-72157647693591113"
            },
            new WeatherLadyData
            {
                Author = "胖胖豬",
                FileName = "Need4.png",
                Quotation = "https://www.flickr.com/photos/nengwei/12132068234/in/set-72157640133070716/"
            },
            new WeatherLadyData
            {
                Author = "swanky",
                FileName = "Need5.png",
                Quotation = "https://www.flickr.com/photos/swanky-hsiao/936676913/in/set-72157601074343374/"
            },
            new WeatherLadyData
            {
                Author = "胖胖豬",
                FileName = "Need6.png",
                Quotation = "https://www.flickr.com/photos/nengwei/14189370508/in/set-72157645069854082/"
            },
            new WeatherLadyData
            {
                Author = "しゃれこーべ",
                FileName = "Need7.png",
                Quotation = "https://www.flickr.com/photos/fragileruins/16457665672/"
            },
        };

        public static WeatherLadyData GetData(int index) => WeatherLadyDatas.ElementAt(index);
        public static int Count => WeatherLadyDatas.Count();
    }
}