using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Find2MeWeb.Models;
using Find2Me.Infrastructure.DbModels;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Twitter;
using Newtonsoft.Json;
using Find2Me.Infrastructure;
using System.Security.Claims;
using System.Collections.Generic;
using System.Configuration;

namespace Find2MeWeb
{
    public partial class Startup
    {
        // Enable the application to use OAuthAuthorization. You can then secure your Web APIs
        //static Startup()
        //{
        //    PublicClientId = "web";

        //    OAuthOptions = new OAuthAuthorizationServerOptions
        //    {
        //        TokenEndpointPath = new PathString("/Token"),
        //        AuthorizeEndpointPath = new PathString("/Account/Authorize"),
        //        Provider = new ApplicationOAuthProvider(PublicClientId),
        //        AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
        //        AllowInsecureHttp = true
        //    };
        //}

        //public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        //public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(TimeSpan.FromMinutes(30),
                            (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Enable the application to use bearer tokens to authenticate users
            //app.UseOAuthBearerTokens(OAuthOptions);


            /*---------------------------------------------------------------------------------
             * Configuration of the various providers
             *--------------------------------------------------------------------------------- */
            // Configure Facebook
            ConfigureFacebook(app);

            // Configure Google
            ConfigureGoogle(app);

            // Configure Instagram
            ConfigureInstagram(app);

            // Configure Twitter
            ConfigureTwitter(app);
            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "test",
            //    consumerSecret: "test");

            //app.UseFacebookAuthentication(
            //    appId: "test",
            //    appSecret: "test");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "test",
            //    ClientSecret = "test"
            //});

        }

        private void ConfigureFacebook(IAppBuilder app)
        {
            /* -------------------------------------------------------------------------------
             * Normal configuration
             * ------------------------------------------------------------------------------- */

            //app.UseFacebookAuthentication("Your App ID", "Your App Secret");

            /* -------------------------------------------------------------------------------
             * Request extra permissions
             * ------------------------------------------------------------------------------- */

            //var options = new FacebookAuthenticationOptions
            //{
            //    AppId = "1208835509271215",
            //    AppSecret = "1abfaaf382ca042b8a91ffd74e5ff18a",
            //};
            //options.Scope.Add("user_friends");
            //app.UseFacebookAuthentication(options);

            /* -------------------------------------------------------------------------------
             * Specify an alternate callback path. In this case you need to make sure that
             * the redirect URI you specify when registering the application in Facebook
             * matches this exactly
             * ------------------------------------------------------------------------------- */

            //var options = new FacebookAuthenticationOptions
            //{
            //    AppId = "Your App ID",
            //    AppSecret = "Your App Secret",
            //    CallbackPath = new PathString("/oauth-redirect/facebook")
            //};
            //app.UseFacebookAuthentication(options);

            /* -------------------------------------------------------------------------------
             * Retrieve the access token and other user information
             * ------------------------------------------------------------------------------- */
            /*
           * ========== STEPS FOR GOOGLE OAUTH ========== 
           *  1 - Create Facebook App and Get the App ID and App Secret
           *  2 - Set the OAuth Redirect URLs
           *      For localhost enable SSL. provide URLs as https://localhost:44300/signin-facebook
           */
            var options = new FacebookAuthenticationOptions
            {
                //AppId = "1208835509271215",
                //AppSecret = "1abfaaf382ca042b8a91ffd74e5ff18a",
                AppId = ConfigurationManager.AppSettings["FACEBOOK_APP_ID"],
                AppSecret = ConfigurationManager.AppSettings["FACEBOOK_APP_SECRET"],
            };
            options.Scope.Add("email");
            options.Scope.Add("public_profile");
            options.Fields.Add("email");
            options.Fields.Add("name");
            options.Fields.Add("picture.width(500).height(500)");
            options.Fields.Add("first_name");
            options.Fields.Add("middle_name");
            options.Fields.Add("last_name");
            options.CallbackPath = new PathString("/account/ExternalLoginCallback");
            //options.UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email,first_name,last_name";
            options.Provider = new FacebookAuthenticationProvider
            {
                OnAuthenticated = context =>
                {
                    // Retrieve the OAuth access token to store for subsequent API calls
                    string accessToken = context.AccessToken;

                    // Retrieve the username
                    string facebookUserName = context.UserName;

                    // You can even retrieve the full JSON-serialized user
                    OAuthReponseUserFacebook User = context.User.ToObject<OAuthReponseUserFacebook>();

                    //Adding User Data Claims
                    context.Identity.AddClaim(new Claim(ClaimTypes.UserData, context.User.ToString()));

                    return Task.FromResult(true);
                },
            };
            app.UseFacebookAuthentication(options);
        }

        private void ConfigureGoogle(IAppBuilder app)
        {
            /* -------------------------------------------------------------------------------
             * Normal configuration
             * ------------------------------------------------------------------------------- */

            //app.UseGoogleAuthentication("Your client ID", "Your client secret");

            /* -------------------------------------------------------------------------------
             * Request extra permissions
             * ------------------------------------------------------------------------------- */

            //var options = new GoogleOAuth2AuthenticationOptions
            //{
            //    ClientId = "Your client ID",
            //    ClientSecret = "Your client secret",
            //};
            //options.Scope.Add("https://www.googleapis.com/auth/books");
            //app.UseGoogleAuthentication(options);

            /* -------------------------------------------------------------------------------
             * Specify an alternate callback path. In this case you need to make sure that
             * the redirect URI you specify when registering the application in GitHub
             * matches this exactly
             * ------------------------------------------------------------------------------- */

            //var options = new GoogleOAuth2AuthenticationOptions
            //{
            //    ClientId = "Your client ID",
            //    ClientSecret = "Your client secret",
            //    CallbackPath = new PathString("/oauth-redirect/google")
            //};
            //app.UseGoogleAuthentication(options);

            /* -------------------------------------------------------------------------------
             * Retrieve the access token and other user information
             * ------------------------------------------------------------------------------- */
            /*
             * ========== STEPS FOR GOOGLE OAUTH ========== 
             *  1 - Create Client OAuth ID for Web Applications
             *  2 - Enable Google+ API.
             *  3 - Set the OAuth Redirect URLs
             *      For localhost enable SSL. provide URLs as Javascript Origin: https://localhost:44300, Redirec URL: https://localhost:44300/signin-google
             */

            var options = new GoogleOAuth2AuthenticationOptions
            {
                //ClientId = "286386756957-i596ri7ik4rc5ub8bnhihltc6godlsq5.apps.googleusercontent.com",
                //ClientSecret = " pJ-st1q3yQRoIHmaDLTR4lcu",
                ClientId = ConfigurationManager.AppSettings["GOOGLE_CLIENT_ID"],
                ClientSecret = ConfigurationManager.AppSettings["GOOGLE_CLIENT_SECRET"],
            };

            options.CallbackPath = new PathString("/Account/ExternalLoginCallback");
            options.Provider = new GoogleOAuth2AuthenticationProvider
            {
                OnAuthenticated = context =>
                {
                    // Retrieve the OAuth access token to store for subsequent API calls
                    string accessToken = context.AccessToken;

                    // Retrieve the name of the user in Google
                    string googleName = context.Name;

                    // Retrieve the user's email address
                    string googleEmailAddress = context.Email;

                    // You can even retrieve the full JSON-serialized user
                    var serializedUser = context.User;
                    return Task.FromResult(true);
                }
            };
            app.UseGoogleAuthentication(options);
        }

        public void ConfigureInstagram(IAppBuilder app)
        {
            /* -------------------------------------------------------------------------------
             * Normal configuration
             * ------------------------------------------------------------------------------- */

            //app.UseInstagramInAuthentication("Your client id", "Your client secret");

            /* -------------------------------------------------------------------------------
             * Request extra permissions
             * ------------------------------------------------------------------------------- */

            //var options = new InstagramAuthenticationOptions()
            //{
            //    ClientId = "Your client id",
            //    ClientSecret = "Your client secret"
            //};
            //options.Scope.Add("likes");
            //app.UseInstagramInAuthentication(options);

            /* -------------------------------------------------------------------------------
             * Specify an alternate callback path. In this case you need to make sure that
             * the redirect URI you specify when registering the application in Instagram
             * matches this exactly
             * ------------------------------------------------------------------------------- */

            //var options = new InstagramAuthenticationOptions()
            //{
            //    ClientId = "Your client id",
            //    ClientSecret = "Your client secret",
            //    CallbackPath = new PathString("/oauth-redirect/instagram")
            //};
            //app.UseInstagramInAuthentication(options);

            /* -------------------------------------------------------------------------------
             * Retrieve the access token and other user information
             * ------------------------------------------------------------------------------- */

            //var options = new InstagramAuthenticationOptions()
            //{
            //    ClientId = "Your client id",
            //    ClientSecret = "Your client secret",
            //    Provider = new InstagramAuthenticationProvider
            //    {
            //        OnAuthenticated = async context =>
            //        {
            //            // Retrieve the OAuth access token to store for subsequent API calls
            //            string accessToken = context.AccessToken;

            //            // Retrieve the username
            //            string userName = context.UserName;

            //            // Retrieve the user's full name
            //            string fullName = context.Name;

            //            // You can even retrieve the full JSON-serialized user
            //            var serializedUser = context.User;
            //        }
            //    }
            //};
            //app.UseInstagramInAuthentication(options);

        }

        private void ConfigureTwitter(IAppBuilder app)
        {
            /* -------------------------------------------------------------------------------
             * Normal configuration
             * ------------------------------------------------------------------------------- */

            //app.UseTwitterAuthentication("Your consumer key", "Your consumer secret");

            /* -------------------------------------------------------------------------------
             * Specify an alternate callback path
             * ------------------------------------------------------------------------------- */

            //var options = new TwitterAuthenticationOptions
            //{
            //    ConsumerKey = "Your consumer key",
            //    ConsumerSecret = "Your consumer secret",
            //    CallbackPath = new PathString("/oauth-redirect/twitter")
            //};
            //app.UseTwitterAuthentication(options);

            /* -------------------------------------------------------------------------------
             * Retrieve the access token and other user information
             * ------------------------------------------------------------------------------- */

            var options = new TwitterAuthenticationOptions
            {
                ConsumerKey = "Your Consumer Key",
                ConsumerSecret = "Your Consumer Secret",
            };

            options.Provider = new TwitterAuthenticationProvider
            {
                OnAuthenticated = context =>
                {
                    // Retrieve the OAuth access token to store for subsequent API calls
                    string accessToken = context.AccessToken;

                    // Retrieve the screen name (e.g. @jerriepelser)
                    string twitterScreenName = context.ScreenName;

                    // Retrieve the user ID
                    var twitterUserId = context.UserId;
                    return Task.FromResult(true);
                }
            };

            app.UseTwitterAuthentication(options);

        }

    }
}
