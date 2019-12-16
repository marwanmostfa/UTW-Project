using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;

namespace regestraionandlogin.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult English()
        {
            return View();
        }
        public ActionResult Arabic()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Change(string LanguageAbbr)
        {
            if (LanguageAbbr != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbr);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbr);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbr;
            Response.Cookies.Add(cookie);
            return View("Index");
        }
    }
}