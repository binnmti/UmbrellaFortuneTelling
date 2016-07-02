using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenWeatherMap;
using Weather;
using WeatherLady;

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
            ViewBag.Result = um.Is ? "いる?" : "いらない?";
            ViewBag.Percent = um.Percent + @"%";
            ViewBag.City = data.City;

            var r = new Random();
            var h = r.Next(100);
            var picture = WeatherLadyUtil.GetData(h%WeatherLadyUtil.Count);
            ViewBag.FileName = picture.FileName;
            ViewBag.Author = picture.Author;
            ViewBag.Quotation = picture.Quotation;
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