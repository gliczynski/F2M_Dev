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
using Owin.Security.Providers.Instagram.Provider;
using Owin.Security.Providers.Instagram;
using Microsoft.Owin.Security;
using TweetSharp;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Web;

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
                CallbackPath = new PathString("/en/signin-facebook"),
                //CallbackPath = new PathString("/en/Account/ExternalLoginCallback"),
            };

            options.Scope.Add("email");
            options.Scope.Add("public_profile");
            options.Fields.Add("email");
            options.Fields.Add("name");
            options.Fields.Add("picture.width(500).height(500)");
            options.Fields.Add("first_name");
            options.Fields.Add("middle_name");
            options.Fields.Add("last_name");
            //options.UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email,first_name,last_name";
            options.Provider = new FacebookAuthenticationProvider
            {
                OnAuthenticated = context =>
                {
                    // Retrieve the OAuth access token to store for subsequent API calls
                    //string accessToken = context.AccessToken;

                    // Retrieve the username
                    //string facebookUserName = context.UserName;

                    // You can even retrieve the full JSON-serialized user
                    //OAuthReponseUserFacebook User = context.User.ToObject<OAuthReponseUserFacebook>();

                    //Adding User Data Claims
                    context.Identity.AddClaim(new Claim(_ClaimTypes.ExternalProviderAccessToken, context.AccessToken));
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
                CallbackPath = new PathString("/en/signin-google"),
                //CallbackPath = new PathString("/en/Account/ExternalLoginCallback")
            };

            options.Provider = new GoogleOAuth2AuthenticationProvider
            {
                OnAuthenticated = context =>
                {
                    // Retrieve the OAuth access token to store for subsequent API calls
                    //string accessToken = context.AccessToken;

                    // Retrieve the name of the user in Google
                    //string googleName = context.Name;

                    // Retrieve the user's email address
                    //string googleEmailAddress = context.Email;

                    // You can even retrieve the full JSON-serialized user
                    //var serializedUser = context.User;
                    context.Identity.AddClaim(new Claim(_ClaimTypes.ExternalProviderAccessToken, context.AccessToken));
                    context.Identity.AddClaim(new Claim(ClaimTypes.UserData, context.User.ToString()));

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

            var options = new InstagramAuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["INSTAGRAM_CLIENT_ID"],
                ClientSecret = ConfigurationManager.AppSettings["INSTAGRAM_CLIENT_SECRET"],
                CallbackPath = new PathString("/en/signin-instagram"),
                //CallbackPath = new PathString("/en/Account/ExternalLoginCallback"),
            };

            options.Provider = new InstagramAuthenticationProvider
            {
                OnAuthenticated = context =>
                {
                    try
                    {
                        // Retrieve the OAuth access token to store for subsequent API calls
                        //string accessToken = context.AccessToken;

                        // Retrieve the username
                        //string userName = context.UserName;

                        // Retrieve the user's full name
                        //string fullName = context.Name;

                        // You can even retrieve the full JSON-serialized user
                        //var serializedUser = context.User;
                        context.Identity.AddClaim(new Claim(_ClaimTypes.ExternalProviderAccessToken, context.AccessToken));
                        context.Identity.AddClaim(new Claim(ClaimTypes.UserData, context.User.ToString()));
                    }
                    catch (Exception e)
                    {
                        File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logs.txt"), DateTime.UtcNow.ToShortDateString() + " - " + HttpContext.Current.Request.Browser.Type + "\n\n" + e.Message + "\n\n===============================\n\n");
                    }
                    return Task.FromResult(true);
                }
            };
            app.UseInstagramInAuthentication(options);

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
                ConsumerKey = ConfigurationManager.AppSettings["TWITTER_API_KEY"],
                ConsumerSecret = ConfigurationManager.AppSettings["TWITTER_API_SECRET"],
                //BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(new[] {
                //    "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA – G2
                //    "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA – G3
                //    "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority – G5
                //    "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA – G4
                //    "5168FF90AF0207753CCCD9656462A212B859723B", //DigiCert SHA2 High Assurance Server C‎A
                //    "B13EC36903F8BF4701D498261A0802EF63642BC3" //DigiCert High Assurance EV Root CA
                //}),
                //CallbackPath = new PathString("/en/Account/LoginCallback"),
                CallbackPath = new PathString("/en/signin-twitter"),
            };

            options.Provider = new TwitterAuthenticationProvider
            {
                OnAuthenticated = context =>
                {
                    try
                    {
                        // Retrieve the OAuth access token to store for subsequent API calls
                        //string accessToken = context.AccessToken;

                        // Retrieve the screen name (e.g. @jerriepelser)
                        //string twitterScreenName = context.ScreenName;

                        // Retrieve the user ID
                        //var twitterUserId = context.UserId;
                        var userData = new OAuthReponseUserTwitter
                        {
                            //ProfilePicture = string.Format("https://twitter.com/{0}/profile_image?size=original", twitterScreenName)
                        };
                        OAuthReponseUserTwitter response = TwitterLogin(context.AccessToken, context.AccessTokenSecret, options.ConsumerKey, options.ConsumerSecret);
                        if (response != null)
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Email, response.Email));
                            context.Identity.AddClaim(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(response)));
                        }
                        context.Identity.AddClaim(new Claim(_ClaimTypes.ExternalProviderAccessToken, context.AccessToken));
                    }
                    catch (Exception e)
                    {
                        File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logs.txt"), DateTime.UtcNow.ToShortDateString() + " - " + HttpContext.Current.Request.Browser.Type + "\n\n" + e.Message + "\n\n===============================\n\n");
                    }
                    return Task.FromResult(true);
                }
            };

            app.UseTwitterAuthentication(options);

        }

        public OAuthReponseUserTwitter TwitterLogin(string oauth_token, string oauth_token_secret, string oauth_consumer_key, string oauth_consumer_secret)
        {
            // oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            // unique request details
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            var resource_url = "https://api.twitter.com/1.1/account/verify_credentials.json";
            var request_query = "include_email=true";
            // create oauth signature
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

            var baseString = string.Format(baseFormat,
                                        oauth_consumer_key,
                                        oauth_nonce,
                                        oauth_signature_method,
                                        oauth_timestamp,
                                        oauth_token,
                                        oauth_version
                                        );

            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url) + "&" + Uri.EscapeDataString(request_query), "%26", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            // create the request header
            var headerFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", oauth_timestamp=\"{4}\", oauth_token=\"{5}\", oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_version)
                            );


            // make the request

            ServicePointManager.Expect100Continue = false;
            resource_url += "?include_email=true";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            return JsonConvert.DeserializeObject<OAuthReponseUserTwitter>(new StreamReader(response.GetResponseStream()).ReadToEnd());
        }
    }
}
