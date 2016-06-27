using System;
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

            var current = OpenWeatherMapCityUtil.GetCurrentCountry();
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
            ViewBag.Percent = um.Percent + @"%";
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