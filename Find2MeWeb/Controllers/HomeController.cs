using Find2Me.Infrastructure;
using Find2MeWeb.ActionFilters;
using Find2Me.Resources;
using Find2MeWeb.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace Find2MeWeb.Controllers
{
    //[Authorize]
    [ProfileWizardCompletionCheck]
    public class HomeController : Controller
    {
        [Internationalization]
        public ActionResult Index()
        {
            if (TempData["SignUpError"] != null)
            {
                IdentityResult identityResult = (IdentityResult)TempData["SignUpError"];
                if (identityResult != null)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        string errorMessage = error;
                        if (error.ToLower().StartsWith("email") && error.EndsWith("is already taken"))
                        {
                            ViewData["PopupError"] = true;
                            ViewData["PopupErrorMessage"] = "EmailTaken";
                        }
                    }
                }
            }

            if (TempData["ResponseResult"] != null)
            {
                ResponseResult<object> responseResult = (ResponseResult<object>)TempData["ResponseResult"];
                //ViewData["PopupError"] = responseResult.Success;
                //ViewData["PopupErrorMessage"] = responseResult.Message;
            }

            // Get string from strongly typed localzation resources
            var vm = new FullViewModel { LocalisedString = Strings.SomeLocalisedString };
            return View(vm);

            // return View();
        }

        // Localize string without any external impact with caching
        [OutputCache(Duration = 3600)]
        public ActionResult CachedIndex()
        {
            var vm = new FullViewModel { LocalisedString = Strings.SomeLocalisedString };
            return View("Index", vm);
        }

        // Get language from quuery string (by binder)
        public ActionResult LangFromQueryString(string lang)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);

            var vm = new FullViewModel { LocalisedString = Strings.SomeLocalisedString };
            return View("Index", vm);
        }

        // Get language as a parameter from route data
        public ActionResult LangFromRouteValues(string lang)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);

            var vm = new FullViewModel { LocalisedString = Strings.SomeLocalisedString };
            return View("Index", vm);
        }

        // Get language in action filter (from route parameter)
        [Internationalization]
        public ActionResult LangFromRouteInActionFilter()
        {
            var vm = new FullViewModel { LocalisedString = Strings.SomeLocalisedString };
            return View("Index", vm);
        }

        // Get language in action filter (from route parameter) with caching result
        [Internationalization]
        [OutputCache(Duration = 3600)]
        public ActionResult CachedLangFromRouteInActionFilter()
        {
            var vm = new FullViewModel { LocalisedString = Strings.SomeLocalisedString };
            return View("Index", vm);
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
