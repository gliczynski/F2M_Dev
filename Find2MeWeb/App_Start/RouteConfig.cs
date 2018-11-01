using Find2Me.Infrastructure.DbModels;
using Find2Me.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Find2MeWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "ProfilePageRoute",
                url: "{lang}/{id}",
                defaults: new {
                    controller = "Profile",
                    action = "Index",
                    lang = "da"
                },
                constraints: new hasProfilePage()
                );

            // Localization route - it will be used as a route of the first priority 
            routes.MapRoute(
                name: "DefaultLocalized",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional,
                    lang = "da"
                });



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }


        public class hasProfilePage : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                // If parameter matches any known page, 
                // then we have found a match.
                string url = values[parameterName] as string;

                if (string.IsNullOrEmpty(url))
                {
                    // check our database
                    using(ApplicationDbContext dbContext = new ApplicationDbContext())
                    {
                        return new UserAccountService(dbContext).UserExists(url);
                    }
                }

                return false;
            }
        }
    }
}
