using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenWeatherMap;
using WeatherLady;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CreateCitySelect(string.Empty);
            return View();
        }

        private void CreateCitySelect(string getCity)
        {
            //ToDo 国際化は後回し
            const string current = "JP"; //OpenWeatherMapCityUtil.GetCurrentCountry();
            var citys = OpenWeatherMapCityUtil.GetCitys(current).Distinct();
            //kyoto+kyotoをはじく
            foreach (var city in getCity.Split('+'))
            {
                citys = citys.Where(c => c != city);
            }
            var cookieCity = GetCookieValueByKey("City");
            ViewBag.Citys = citys.Select(c => new SelectListItem
            {
                Value = getCity == string.Empty ? c : $"{c}+{getCity}",
                Text = getCity == string.Empty ? c : $"{c}+{getCity}",
                Selected = c == cookieCity
            });
        }

        public ActionResult Fortune()
        {
            var getCity = Request.QueryString["city"];
            if (string.IsNullOrEmpty(getCity))
            {
                return RedirectToAction("Index");
            }
            CreateCitySelect(getCity);
            //クッキーは最初に入れたやつだけ
            SetCookie("City", getCity.Split('+')[getCity.Split('+').Length - 1]);

            var report = new OpenWeatherMapWeatherReport();
            report.Update(getCity);
            var um = report.GetUmbrella(DateTime.Now);
            ViewBag.Umbrella = um.Is;
            ViewBag.Result = um.Is ? "いる?" : "いらない?";
            ViewBag.Percent = um.Percent + @"%";
            ViewBag.City = getCity;
            ViewBag.CitySplit = getCity.Split('+');
            ViewBag.CitySplitCount = getCity.Split('+').Length;

            var r = new Random();
            var h = r.Next(100);
            var picture = WeatherLadyUtil.GetData(h%WeatherLadyUtil.Count);
            ViewBag.FileName = picture.FileName;
            ViewBag.Author = picture.Author;
            ViewBag.Quotation = picture.Quotation;
            var model = report.TodayWeatherData2();
            return View(model);
        }

        private string GetCookieValueByKey(string key) => Request.Cookies[key]?.Value;

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