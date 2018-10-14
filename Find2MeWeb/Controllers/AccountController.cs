using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Find2MeWeb.Models;
using Find2Me.Infrastructure.DbModels;
using Find2Me.Infrastructure;
using Find2Me.Services;
using System.Collections.Generic;

namespace Find2MeWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _dbContext;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // The Authorize Action is the end point which gets called when you access any
        // protected Web API. If the user is not logged in then they will be redirected to 
        // the Login page. After a successful login you can call a Web API.
        [HttpGet]
        public ActionResult Authorize()
        {
            var claims = new ClaimsPrincipal(User).Claims.ToArray();
            var identity = new ClaimsIdentity(claims, "Bearer");
            AuthenticationManager.SignIn(identity);
            return new EmptyResult();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            string challangeUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var result = new ChallengeResult(provider, challangeUrl);
            return result;
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    //TempData External Identity, After login, we will need some of these Claims
                    TempData["ExternalLoginInfo"] = loginInfo.ExternalIdentity;
                    return RedirectToAction("AddingClaims", new { returnUrl = returnUrl });

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account

                    //Create a New User and SignIn
                    return await CreateExternalProviderUserAsync(loginInfo);

            }
        }

        private async Task<ActionResult> CreateExternalProviderUserAsync(ExternalLoginInfo loginInfo)
        {
            //Create a New User and SignIn
            var user = new ApplicationUser();

            //If External Provider has email, set the email here. Otherwise we will take it on Profile Wizard
            user.Email = loginInfo.Email;
            //If email is set, check whether a User with same email exists or not
            if (!string.IsNullOrEmpty(user.Email))
            {
                if (UserManager.FindByEmail(user.Email) != null)
                {
                    TempData["SignUpError"] = new IdentityResult(new List<string>
                    {
                        "Email is already taken",
                    });
                    return RedirectToAction("Index", "Home");
                }
            }


            string profilePictureUrl = null;
            //Check the Login Provider. We need to Extract the user data based on the LoginProvider
            switch (loginInfo.Login.LoginProvider)
            {
                case "Facebook":
                    //Get Facebook User Data from the Saved Claim
                    OAuthReponseUserFacebook facbookUserData = loginInfo.ExternalIdentity.GetOAuthUserData<OAuthReponseUserFacebook>();
                    if (facbookUserData != null)
                    {
                        //Save the Profile Image
                        if (facbookUserData.Picture.Data != null)
                        {
                            profilePictureUrl = facbookUserData.Picture.Data.URL;
                        }

                        //Get the other Data
                        user.FullName = facbookUserData.Name;
                        user.UserName = "user_" + facbookUserData.ID;
                    }
                    break;

                case "Google":
                    //Get Google User Data from the Saved Claim
                    OAuthReponseUserGoogle googleUserData = loginInfo.ExternalIdentity.GetOAuthUserData<OAuthReponseUserGoogle>();
                    if (googleUserData != null)
                    {
                        //Save the Profile Image
                        if (googleUserData.Image != null)
                        {
                            profilePictureUrl = googleUserData.Image.URL;
                        }

                        //Get the other Data
                        user.FullName = googleUserData.DisplayName;
                        user.UserName = "user_" + googleUserData.ID;
                    }
                    break;

                case "Instagram":
                    //Get Google User Data from the Saved Claim
                    OAuthReponseUserInstagram instagramUserData = loginInfo.ExternalIdentity.GetOAuthUserData<OAuthReponseUserInstagram>();
                    if (instagramUserData != null)
                    {
                        //Save the Profile Image
                        profilePictureUrl = instagramUserData.ProfilePicture;

                        //Get the other Data
                        user.FullName = instagramUserData.FullName;
                        user.UserName = "user_" + instagramUserData.ID;
                    }
                    break;

                case "Twitter":
                    //Get Google User Data from the Saved Claim
                    OAuthReponseUserTwitter twitterUserData = loginInfo.ExternalIdentity.GetOAuthUserData<OAuthReponseUserTwitter>();
                    if (twitterUserData != null)
                    {
                        //Save the Profile Image
                        profilePictureUrl = twitterUserData.ProfileImageUrl;

                        //Get the other Data
                        user.FullName = twitterUserData.Name;
                        user.UserName = "user_" + twitterUserData.ID;
                    }
                    break;
            }

            //Set the User Profile Create and Update Date
            user.CreatedOn = DateTime.UtcNow;
            user.UpdatedOn = DateTime.UtcNow;

            //Create a User
            var createUserResult = await UserManager.CreateAsync(user);
            if (createUserResult.Succeeded)
            {
                //Add User Roles
                UserManager.AddToRole(user.Id, _UserRolesType.User);

                //Add Login Infor based on the Login Provider
                createUserResult = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                if (createUserResult.Succeeded)
                {
                    //Download User Profile Image and Update User
                    string profileImageName = DateTime.UtcNow.ToString("yyyyddmmhhmmss") + "_profile_original.jpg";
                    string profileImagePath = Server.MapPath("~" + _FileSavingPaths.ProfileImage + "/" + profileImageName);
                    if (UtilityExtension.DownloadImage(profilePictureUrl, profileImagePath))
                    {
                        user.ProfileImageOriginal = profileImageName;
                        user.ProfileImageSelected = profileImageName;
                    }
                    await UserManager.UpdateAsync(user);

                    //Add Provider Claim
                    UserManager.AddClaim(user.Id, new Claim(_ClaimTypes.ExternalProviderType, loginInfo.Login.LoginProvider));
                    UserManager.AddClaim(user.Id, new Claim(_ClaimTypes.ExternalProviderUsername, loginInfo.DefaultUserName));
                    UserManager.AddClaim(user.Id, new Claim(_ClaimTypes.UrlUserName, loginInfo.DefaultUserName));
                    UserManager.AddClaim(user.Id, new Claim(_ClaimTypes.HasCompletedProfileWizard, false.ToString()));

                    //Login the current User
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    //TempData External Identity, After login, we will need some of these Claims
                    TempData["ExternalLoginInfo"] = loginInfo.ExternalIdentity;

                    //Add claims and Redirect to Profile Wizard Step 1
                    return RedirectToAction("AddingClaims");
                }
            }

            AddErrors(createUserResult);
            TempData["SignUpError"] = createUserResult;
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> AddingClaims(string returnUrl)
        {
            if (TempData["ExternalLoginInfo"] != null)
            {
                var externalIdentity = (ClaimsIdentity)TempData["ExternalLoginInfo"];
                if (externalIdentity != null)
                {
                    //Remove Old Data, and Add new Data
                    Claim claimUserData = User.Identity.GetClaim(ClaimTypes.UserData);
                    if (claimUserData != null) { UserManager.RemoveClaim(User.Identity.GetUserId(), claimUserData); }

                    Claim claimUserDataExt = externalIdentity.GetClaim(ClaimTypes.UserData);
                    if (claimUserDataExt != null) { UserManager.AddClaim(User.Identity.GetUserId(), claimUserDataExt); }

                    User.Identity.CopyExternalOAuthClaims(externalIdentity, AuthenticationManager);
                }
            }

            return RedirectToLocal(returnUrl);
        }
        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserExists([Bind(Prefix = "id")]string username)
        {
            _dbContext = new ApplicationDbContext();
            UserAccountService userAccountService = new UserAccountService(_dbContext);

            //If User exists, create use suggestions
            if (userAccountService.UserExists(User.Identity.GetUserId(), username))
            {
                //Get User Suggestions and return
                return Json(new UserValidationSuggestions
                {
                    UserExists = true,
                    UserSuggestion = userAccountService.GetUserSuggestion(username)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Otherwise there is no need for suggestions
                return Json(new UserValidationSuggestions
                {
                    UserExists = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
