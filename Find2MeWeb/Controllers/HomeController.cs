using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Find2MeWeb.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

    }
}
