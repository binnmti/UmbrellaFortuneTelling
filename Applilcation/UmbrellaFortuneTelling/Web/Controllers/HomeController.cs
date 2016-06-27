using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenWeatherMap;
using Weather;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var city = GetCookieValueByKey("City");

            //ToDo 国際化は後回し
            var current = "JP"; //OpenWeatherMapCityUtil.GetCurrentCountry();
            ViewBag.Citys = OpenWeatherMapCityUtil.GetCitys(current).Select(citys => new SelectListItem
            {
                Value = citys,
                Text = citys,
                Selected = citys == city
            });
            return View();
        }



        //ToDo リファクタリング対象
        public class PictureData
        {
            public string Auther { get; set; }
            public string FileName { get; set; }
            public string Link { get; set; }
        }
        List<PictureData> PictureDatas = new List<PictureData>()
        {
            new PictureData() { Auther = "David Photo Studio",FileName = "Need.png", Link = "https://www.flickr.com/photos/jackal1203/14801855910/"},
            new PictureData() { Auther = "gregt99", FileName = "Need2.png", Link = "https://www.flickr.com/photos/80592618@N06/16118194721/in/set-72157649551014970/"},
            new PictureData() { Auther = "gregt99", FileName = "Need3.png", Link = "https://www.flickr.com/photos/80592618@N06/15527469693/in/set-72157647693591113"},
            new PictureData() { Auther = "胖胖豬", FileName = "Need4.png", Link = "https://www.flickr.com/photos/nengwei/12132068234/in/set-72157640133070716/"},
            new PictureData() { Auther = "swanky", FileName = "Need5.png", Link = "https://www.flickr.com/photos/swanky-hsiao/936676913/in/set-72157601074343374/"},
            new PictureData() { Auther = "胖胖豬", FileName = "Need6.png", Link = "https://www.flickr.com/photos/nengwei/14189370508/in/set-72157645069854082/"},
            new PictureData() { Auther = "しゃれこーべ", FileName = "Need7.png", Link = "https://www.flickr.com/photos/fragileruins/16457665672/"},
        };

        public ActionResult Fortune(string value, WeatherReportData data)
        {
            if (data.City == null)
            {
                return RedirectToAction("Index");
            }
            SetCookie("City", data.City);
            var report = new OpenWeatherMapWeatherReport();
            report.Update(data.City);
            var model = report.TodayWeatherData();
            var um = report.GetUmbrella(DateTime.Now);
            ViewBag.Umbrella = um.Is;
            ViewBag.Percent = um.Percent + @"%";

            var r = new Random();
            var h = r.Next(100);
            var picture = PictureDatas[h % PictureDatas.Count];
            ViewBag.FileName = picture.FileName;
            ViewBag.Link = picture.Link;
            ViewBag.Author = picture.Auther;
            return View(model);
        }


        /// <summary>
        /// キーから、リクエストのクッキーを取得します
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetCookieValueByKey(string key) => Request.Cookies[key]?.Value;

        /// <summary>
        /// レスポンスにクッキーを設定します
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        /// <returns></returns>
        private void SetCookie(string key, string value)
        {
            var cookie = new HttpCookie(key)
            {
                Value = value,
                Expires = DateTime.Now.AddYears(50)
            };
            Response.Cookies.Add(cookie);
        }
    }
}