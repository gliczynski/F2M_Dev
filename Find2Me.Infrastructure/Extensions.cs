using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure
{
    public static class Extensions
    {
        public static OAuthReponseUserFacebook GetFacebookUserData(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
                Claim userDataClaim = claimsIdentity.FindFirst(ClaimTypes.UserData);
                if (userDataClaim != null)
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<OAuthReponseUserFacebook>(userDataClaim.Value);
                    }
                    catch { }
                }
            }
            return null;
        }
    }
}
