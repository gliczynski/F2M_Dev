using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Find2Me.Infrastructure
{
  public static class Extensions
  {
    public static Claim GetClaim(this IIdentity identity, string claimType)
    {
      if (identity.IsAuthenticated)
      {
        ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
        return claimsIdentity.FindFirst(x => x.Type == claimType);
      }
      return null;
    }

    public static T GetOAuthUserData<T>(this IIdentity identity) where T : class
    {
      if (identity.IsAuthenticated)
      {
        ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
        Claim userDataClaim = claimsIdentity.FindFirst(ClaimTypes.UserData);
        if (userDataClaim != null)
        {
          try
          {
            return JsonConvert.DeserializeObject<T>(userDataClaim.Value);
          }
          catch (Exception err) { }
        }
      }
      return null;
    }

    public static string GetExternalProviderType(this IIdentity identity)
    {
      if (identity.IsAuthenticated)
      {
        ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
        Claim providerClaim = claimsIdentity.FindFirst(_ClaimTypes.ExternalProviderType);
        if (providerClaim != null)
        {
          return providerClaim.Value;
        }
      }
      return null;
    }

    public static string GetExternalProviderUsername(this IIdentity identity)
    {
      if (identity.IsAuthenticated)
      {
        ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
        Claim providerClaim = claimsIdentity.FindFirst(_ClaimTypes.ExternalProviderUsername);
        if (providerClaim != null)
        {
          return providerClaim.Value.Trim().ToLower();
        }
      }
      return null;
    }

    public static string GetUrlUserName(this IIdentity identity)
    {
      if (identity.IsAuthenticated)
      {
        ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
        Claim providerClaim = claimsIdentity.FindFirst(_ClaimTypes.UrlUserName);
        if (providerClaim != null)
        {
          return providerClaim.Value.Trim().ToLower();
        }
      }
      return null;
    }

    public static void CopyExternalOAuthClaims(this IIdentity identity, IIdentity externalIdentity, IAuthenticationManager authenticationManager)
    {
      if (externalIdentity.IsAuthenticated)
      {
        ClaimsIdentity externalClaimsIdentity = externalIdentity as ClaimsIdentity;
        ClaimsIdentity userClaimsIdentity = identity as ClaimsIdentity;
        Claim userDataClaim = externalClaimsIdentity.FindFirst(ClaimTypes.UserData);
        if (userDataClaim != null)
        {
          userClaimsIdentity.AddClaim(userDataClaim);
        }

        Claim TokenClaim = externalClaimsIdentity.FindFirst(_ClaimTypes.ExternalProviderAccessToken);
        if (TokenClaim != null)
        {
          userClaimsIdentity.AddClaim(TokenClaim);
        }

        authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userClaimsIdentity);
      }
    }

    public static void AddOrUpdateClaims(this IIdentity identity, List<Claim> claims, IAuthenticationManager authenticationManager)
    {
      if (identity.IsAuthenticated)
      {
        ClaimsIdentity userClaimsIdentity = identity as ClaimsIdentity;

        foreach (var claim in claims)
        {
          var existedClaim = userClaimsIdentity.Claims.FirstOrDefault(x => x.Type == claim.Type);
          if (existedClaim != null)
          {
            userClaimsIdentity.RemoveClaim(existedClaim);
          }

          userClaimsIdentity.AddClaim(claim);
        }

        authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userClaimsIdentity);
      }
    }
  }
}
