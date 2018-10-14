using Find2Me.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Find2MeWeb.ActionFilters
{
    public class ProfileWizardCompletionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Also Check On each request whether the User is Authenticated or not
            //If User is authenticated, check the Profile wizard is completed or not.
            //If not completed, send user to profile wizard
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                Claim hasProfileWizardCompleted = filterContext.HttpContext.User.Identity.GetClaim(_ClaimTypes.HasCompletedProfileWizard);
                if (hasProfileWizardCompleted != null)
                {
                    //If Not Completed, Send to Wizard Step 1
                    if (hasProfileWizardCompleted.Value == false.ToString())
                    {
                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "Controller", "Profile" },
                            { "Action", "Step1" },
                        });
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}