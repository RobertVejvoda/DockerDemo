using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DockerDemo.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            ViewBag.Environment = ConfigurationManager.AppSettings["ASPNET_ENVIRONMENT"];
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}