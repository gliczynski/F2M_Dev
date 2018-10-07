using Find2Me.Infrastructure;
using Find2Me.Infrastructure.DbModels;
using Find2Me.Infrastructure.ViewModels;
using Find2Me.Services;
using Find2MeWeb.ActionFilters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Find2MeWeb.Controllers
{
    [Internationalization]
    public class ProfileController : Controller
    {
        private ApplicationDbContext _dbContext;

        // GET: Profile
        public ActionResult Index([Bind(Prefix = "id")] string username)
        {


            _dbContext = new ApplicationDbContext();
            //Update the User Profile Details
            UserAccountService userAccountService = new UserAccountService(_dbContext);

            UserProfileVM userProfileVM = null;
            if (!string.IsNullOrEmpty(username))
            {
                userProfileVM = userAccountService.GetUserProfile(username);
            }

            if (User.Identity.IsAuthenticated)
            {
                userProfileVM = userAccountService.GetUserProfileById(User.Identity.GetUserId());
            }

            if (userProfileVM == null)
            {
                return HttpNotFound();
            }

            return View(userProfileVM);
        }


        [HttpGet]
        [Authorize]
        public ActionResult Step1()
        {
            _dbContext = new ApplicationDbContext();
            //Get Current User's Profile
            string currentUserId = User.Identity.GetUserId();
            UserAccountService userAccountService = new UserAccountService(_dbContext);
            UserProfilePictureVM userProfilePictureVM = userAccountService.GetUserProfilePicture(currentUserId);

            if (userProfilePictureVM == null) { return HttpNotFound(); }

            return View(userProfilePictureVM);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Step1(UserProfilePictureVM userProfilePictureVM)
        {

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Step2()
        {
            _dbContext = new ApplicationDbContext();

            //Get Current User's Profile
            string currentUserId = User.Identity.GetUserId();
            UserAccountService userAccountService = new UserAccountService(_dbContext);
            UserProfileVM userProfileVM = userAccountService.GetUserProfileById(currentUserId);

            if (userProfileVM == null) { return HttpNotFound(); }
            if (string.IsNullOrEmpty(userProfileVM.UrlUsername))
            {
                userProfileVM.UrlUsername = User.Identity.GetExternalProviderUsername();
            }

            ViewBag.YearsOfBirth = UtilityExtension.GetYearsList();
            ViewBag.LanguagesList = UtilityExtension.GetLanguagesList();

            CurrencyService currencyService = new CurrencyService(_dbContext);
            ViewBag.CurrencyList = currencyService.GetAllCurrencies();

            return View(userProfileVM);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Step2(UserProfileVM userProfileVM, string SubmitAction = "")
        {
            //If User does no exists, just update the UserData
            try
            {
                _dbContext = new ApplicationDbContext();

                //If Next Button is clicked, perform the Form Validation, otherwise do not perform Validations
                if (!SubmitAction.ToLower().Equals("back"))
                {
                    if (!ModelState.IsValid)
                    {
                        goto FormValidationFailed_GetData;
                    }
                }

                //Update the User Profile Details
                UserAccountService userAccountService = new UserAccountService(_dbContext);

                //Check If the Username is unique
                userProfileVM.Id = User.Identity.GetUserId();
                if (!SubmitAction.ToLower().Equals("back"))
                {
                    if (userAccountService.UserExists(userProfileVM.Id, userProfileVM.UrlUsername))
                    {
                        ModelState.AddModelError("UrlUsername", "A user exists with the same username. Please choose a different one.");
                        goto FormValidationFailed_GetData;
                    }
                }

                //Update the User
                if (userAccountService.UpdateUserProfile(userProfileVM) == null)
                {
                    ModelState.AddModelError("", "No such user found!");
                    goto FormValidationFailed_GetData;
                }

                //If User is Updated Successfully, Change UrlUserName Claim Also
                Claim UserNameClaim = User.Identity.GetClaim("UrlUserName");
                if (UserNameClaim != null)
                {
                    //If the Username is changed, only then update the Claim
                    if (UserNameClaim.Value.ToLower().Equals(userProfileVM.UrlUsername.ToLower()))
                    {
                        var owinContext = HttpContext.GetOwinContext();
                        var UserManager = owinContext.GetUserManager<ApplicationUserManager>();

                        UserManager.RemoveClaim(userProfileVM.Id, UserNameClaim);
                        UserManager.AddClaim(userProfileVM.Id, new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername));
                        User.Identity.AddOrUpdateClaims(new List<Claim>
                        {
                            new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername),
                        }, owinContext.Authentication);
                    }
                }
                else
                {
                    var owinContext = HttpContext.GetOwinContext();
                    var UserManager = owinContext.GetUserManager<ApplicationUserManager>();

                    UserManager.AddClaim(userProfileVM.Id, new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername));
                    User.Identity.AddOrUpdateClaims(new List<Claim>
                    {
                        new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername),
                    }, owinContext.Authentication);
                }

                //User update was successfull
                //Now Check If Next button is clicked, goto Step 3
                //If Back button is clicked, goto Step 1
                if (SubmitAction.ToLower().Equals("back"))
                {
                    return RedirectToAction("Step1");
                }
                else
                {
                    return RedirectToAction("Step3");
                }
            }
            catch (Exception err)
            {
                ModelState.AddModelError("", err);
                goto FormValidationFailed_GetData;
            }

            //Goto Statement If form validation is failed, goto View but first get the required data for View
            FormValidationFailed_GetData:
            ViewBag.YearsOfBirth = UtilityExtension.GetYearsList();
            ViewBag.LanguagesList = UtilityExtension.GetLanguagesList();

            CurrencyService currencyService = new CurrencyService(_dbContext);
            ViewBag.CurrencyList = currencyService.GetAllCurrencies();
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Step3()
        {
            //Remove the Cookie If the Profile Wizard is completed
            if (Request.Cookies[_CookieNameStrings.IsProfileWizardCompleted] != null)
            {
                Response.Cookies[_CookieNameStrings.IsProfileWizardCompleted].Value = "1";
                Response.Cookies[_CookieNameStrings.IsProfileWizardCompleted].Expires = DateTime.Now.AddDays(-1);
            }
            return View();
        }

        public ActionResult UploadProfileImage(HttpPostedFileBase profileimage, string returnUrl)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase ProfileImage = profileimage;//Request.Files[0];
                if (ProfileImage != null)
                {
                    if (ProfileImage.ContentLength > 0)
                    {
                        _dbContext = new ApplicationDbContext();
                        var currentUserId = User.Identity.GetUserId();

                        //Get the User First
                        UserAccountService userAccountService = new UserAccountService(_dbContext);
                        UserProfilePictureVM userProfile = userAccountService.GetUserProfilePicture(currentUserId);
                        if (userProfile == null) { return HttpNotFound(); }

                        //Save New Profile Image and Remove Old Image
                        string profileImageName = DateTime.UtcNow.ToString("yyyyddmmhhmmss") + "_profile_original.jpg";
                        string profileImagePath = Server.MapPath("~" + _FileSavingPaths.ProfileImage + "/" + profileImageName);

                        //Save New Image
                        if (System.IO.File.Exists(profileImagePath) == false)
                        {
                            ProfileImage.SaveAs(profileImagePath);
                        }

                        //Remove Old Image
                        try
                        {
                            //Delete Original and Selected Images 
                            if (System.IO.File.Exists(Server.MapPath("~" + _FileSavingPaths.ProfileImage + "/" + userProfile.ProfileImageOriginal)))
                            {
                                System.IO.File.Delete(Server.MapPath("~" + _FileSavingPaths.ProfileImage + "/" + userProfile.ProfileImageOriginal));
                            }
                            if (System.IO.File.Exists(Server.MapPath("~" + _FileSavingPaths.ProfileImage + "/" + userProfile.ProfileImageSelected)))
                            {
                                System.IO.File.Delete(Server.MapPath("~" + _FileSavingPaths.ProfileImage + "/" + userProfile.ProfileImageSelected));
                            }
                        }
                        catch { }

                        userProfile.ProfileImageOriginal = profileImageName;
                        userProfile.ProfileImageSelected = profileImageName;
                        userAccountService.UpdateUserProfileImage(userProfile, false);
                    }
                }
            }


            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [Authorize]
        public ActionResult CropProfileImage()
        {
            ResponseResult<object> responseResult = new ResponseResult<object>
            {
                Success = true
            };

            try
            {
                if (Request.Files.Count > 0)
                {
                    _dbContext = new ApplicationDbContext();
                    var currentUserId = User.Identity.GetUserId();

                    //Get the User First
                    UserAccountService userAccountService = new UserAccountService(_dbContext);
                    UserProfilePictureVM userProfile = userAccountService.GetUserProfilePicture(currentUserId);
                    if (userProfile == null) { return HttpNotFound(); }

                    //Upload the Profile Image
                    HttpPostedFileBase croppedProfileImage = Request.Files[0];
                    if (croppedProfileImage != null)
                    {
                        //Save New Profile Image and Remove Old Image
                        string profileImageName = DateTime.UtcNow.ToString("yyyyddmmhhmmss") + "_profile_cropped.jpg";
                        string profileImagePath = Server.MapPath("~" + _FileSavingPaths.ProfileImage + "/" + profileImageName);

                        //Save New Image
                        if (System.IO.File.Exists(profileImagePath) == false)
                        {
                            croppedProfileImage.SaveAs(profileImagePath);
                        }

                        //Remove Old Image
                        try
                        {
                            //If Original and Selected Images are different, only then delete the Seledcted Image
                            if (userProfile.ProfileImageOriginal.ToLower().Equals(userProfile.ProfileImageSelected.ToLower()) == false)
                            {
                                if (System.IO.File.Exists(Server.MapPath("~" + _FileSavingPaths.ProfileImage + "/" + userProfile.ProfileImageSelected)))
                                {
                                    System.IO.File.Delete(Server.MapPath("~" + _FileSavingPaths.ProfileImage + "/" + userProfile.ProfileImageSelected));
                                }
                            }
                        }
                        catch { }

                        userProfile.ProfileImageSelected = profileImageName;

                        //Get the Cropping JS Data
                        if (Request.Form.Count > 0)
                        {
                            CropperJsCropData croppedData = JsonConvert.DeserializeObject<CropperJsCropData>(Request.Form[0]);
                            userProfile.ProfileImageData = new UserProfileImageDataVM();

                            //Set Profile Image Data
                            userProfile.ProfileImageData.CanvasData_Height = croppedData.ImageCanvasData.height;
                            userProfile.ProfileImageData.CanvasData_Left = croppedData.ImageCanvasData.left;
                            userProfile.ProfileImageData.CanvasData_NaturalHeight = croppedData.ImageCanvasData.naturalHeight;
                            userProfile.ProfileImageData.CanvasData_NaturalWidth = croppedData.ImageCanvasData.naturalWidth;
                            userProfile.ProfileImageData.CanvasData_Top = croppedData.ImageCanvasData.top;
                            userProfile.ProfileImageData.CanvasData_Width = croppedData.ImageCanvasData.width;
                            userProfile.ProfileImageData.CropBoxData_Height = croppedData.ImageCropBoxData.height;
                            userProfile.ProfileImageData.CropBoxData_Left = croppedData.ImageCropBoxData.left;
                            userProfile.ProfileImageData.CropBoxData_Top = croppedData.ImageCropBoxData.top;
                            userProfile.ProfileImageData.CropBoxData_Width = croppedData.ImageCropBoxData.width;
                            userProfile.ProfileImageData.UserId = currentUserId;
                        }

                        userAccountService.UpdateUserProfileImage(userProfile);
                    }
                    else
                    {
                        responseResult.Success = false;
                        responseResult.Message = "There is no cropped image found. Please try again!";
                    }
                }
                else
                {
                    responseResult.Success = false;
                    responseResult.Message = "There is no cropped image found. Please try again!";
                }
            }
            catch (Exception err)
            {
                responseResult.Success = false;
                responseResult.Message = "An error occured while uploading profile image. Please try again.";
            }

            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }


        private bool disposed = false;

        public object ProfileImage { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_dbContext != null)
                    {
                        _dbContext.Dispose();
                    }
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}