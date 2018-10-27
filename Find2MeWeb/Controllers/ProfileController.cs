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

            string currentUserId = "";
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = User.Identity.GetUserId();
            }

            if (userProfileVM == null && User.Identity.IsAuthenticated)
            {
                userProfileVM = userAccountService.GetUserProfileById(currentUserId);
            }

            if (userProfileVM == null) { return HttpNotFound(); }

            //Check whether the User is seeing his/her own Profile
            //If it is his/her ownn profile, return Edit Profile Page.
            //else return the Public Profile View
            ViewBag.HasUserFollowed = false;
            if (currentUserId.Equals(userProfileVM.Id))
            {
                ViewBag.YearsOfBirth = UtilityExtension.GetYearsList();
                ViewBag.LanguagesList = UtilityExtension.GetLanguagesList();

                CurrencyService currencyService = new CurrencyService(_dbContext);
                ViewBag.CurrencyList = currencyService.GetAllCurrencies();

                userProfileVM.ProfileImageData = userAccountService.GetUserProfileImageData(userProfileVM.Id);

                return View("UpdateProfile", userProfileVM);
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    ViewBag.HasUserFollowed = userAccountService.GetUserFollower(currentUserId, userProfileVM.Id) != null;
                }
                return View("PublicProfile", userProfileVM);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(UserProfileVM userProfileVM)
        {
            //If User does no exists, just update the UserData
            try
            {
                _dbContext = new ApplicationDbContext();
                if (!ModelState.IsValid)
                {
                    goto FormValidationFailed_GetData;
                }

                //Update the User Profile Details
                UserAccountService userAccountService = new UserAccountService(_dbContext);

                //Update the User
                userProfileVM.Id = User.Identity.GetUserId();
                var userUpdateResponse = userAccountService.UpdateUserProfile(userProfileVM, false);
                if (userUpdateResponse.Success == false)
                {
                    if (userUpdateResponse.MessageCode == ResponseResultMessageCode.EmailExists)
                    {
                        ModelState.AddModelError("Email", "A user already exists with the same email address. Please choose a different one.");
                        ViewBag.DisableEmailTextbox = false;
                    }
                    else if (userUpdateResponse.MessageCode == ResponseResultMessageCode.UserNameExists)
                    {
                        ModelState.AddModelError("UrlUsername", "A user already exists with the same username. Please choose a different one.");
                    }
                    else
                    {
                        ModelState.AddModelError("", ResponseResultMessageCode.GetMessageFromCode(userUpdateResponse.MessageCode));
                    }
                    goto FormValidationFailed_GetData;
                }

                //If User is Updated Successfully, Change UrlUserName Claim Also
                if (userUpdateResponse.SuccessCode == ResponseResultMessageCode.UserNameUpdated)
                {
                    var owinContext = HttpContext.GetOwinContext();
                    var UserManager = owinContext.GetUserManager<ApplicationUserManager>();

                    //New Claims List, for current Identity
                    List<Claim> newClaimsList = new List<Claim>();

                    Claim UserNameClaim = User.Identity.GetClaim(_ClaimTypes.UrlUserName);
                    if (UserNameClaim != null)
                    {
                        //If the Username is changed, only then update the Claim
                        if (!UserNameClaim.Value.ToLower().Equals(userProfileVM.UrlUsername.ToLower()))
                        {
                            UserManager.RemoveClaim(userProfileVM.Id, UserNameClaim);
                            UserManager.AddClaim(userProfileVM.Id, new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername));

                            newClaimsList.Add(new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername));
                        }
                    }
                    else
                    {
                        UserManager.AddClaim(userProfileVM.Id, new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername));

                        newClaimsList.Add(new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername));
                    }

                    //Update Current Identity Claim and Login User Again
                    if (newClaimsList.Count > 0)
                    {
                        User.Identity.AddOrUpdateClaims(newClaimsList, owinContext.Authentication);
                    }
                }

                //User update was successfull
                //Now Check If Next button is clicked, goto Step 3
                //If Back button is clicked, goto Step 1
                return RedirectToAction("Index", new { id = User.Identity.GetUrlUserName() });
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
        public ActionResult Step2()
        {
            ViewBag.DisableEmailTextbox = true;
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
            if (string.IsNullOrEmpty(userProfileVM.Email))
            {
                ViewBag.DisableEmailTextbox = false;
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
                ViewBag.DisableEmailTextbox = true;
                _dbContext = new ApplicationDbContext();
                bool skipValidation = false;

                //If Next Button is clicked, perform the Form Validation, otherwise do not perform Validations
                if (!SubmitAction.ToLower().Equals("back"))
                {
                    if (!ModelState.IsValid)
                    {
                        goto FormValidationFailed_GetData;
                    }
                }
                else
                {
                    skipValidation = true;
                }

                //Update the User Profile Details
                UserAccountService userAccountService = new UserAccountService(_dbContext);

                //Check If the Username is unique
                userProfileVM.Id = User.Identity.GetUserId();
                //Update the User
                var userUpdateResponse = userAccountService.UpdateUserProfile(userProfileVM, skipValidation);
                if (userUpdateResponse.Success == false)
                {
                    if (userUpdateResponse.MessageCode == ResponseResultMessageCode.EmailExists)
                    {
                        ModelState.AddModelError("Email", "A user already exists with the same email address. Please choose a different one.");
                        ViewBag.DisableEmailTextbox = false;
                    }
                    else if (userUpdateResponse.MessageCode == ResponseResultMessageCode.UserNameExists)
                    {
                        ModelState.AddModelError("UrlUsername", "A user already exists with the same username. Please choose a different one.");
                    }
                    else
                    {
                        ModelState.AddModelError("", ResponseResultMessageCode.GetMessageFromCode(userUpdateResponse.MessageCode));
                    }
                    goto FormValidationFailed_GetData;
                }

                //If User is Updated Successfully, Change UrlUserName Claim Also
                var owinContext = HttpContext.GetOwinContext();
                var UserManager = owinContext.GetUserManager<ApplicationUserManager>();

                //New Claims List, for current Identity
                List<Claim> newClaimsList = new List<Claim>();

                Claim UserNameClaim = User.Identity.GetClaim(_ClaimTypes.UrlUserName);
                if (UserNameClaim != null)
                {
                    //If the Username is changed, only then update the Claim
                    if (!UserNameClaim.Value.ToLower().Equals(userProfileVM.UrlUsername.ToLower()))
                    {
                        UserManager.RemoveClaim(userProfileVM.Id, UserNameClaim);
                        UserManager.AddClaim(userProfileVM.Id, new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername));

                        newClaimsList.Add(new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername));
                    }
                }
                else
                {
                    UserManager.AddClaim(userProfileVM.Id, new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername));

                    newClaimsList.Add(new Claim(_ClaimTypes.UrlUserName, userProfileVM.UrlUsername));
                }

                //If User is Updated then the Wizard is Completed, Change HasCompletedProfileWizard Claim Also
                Claim HasCompletedProfileWizardClaim = User.Identity.GetClaim(_ClaimTypes.HasCompletedProfileWizard);
                if (HasCompletedProfileWizardClaim != null)
                {
                    UserManager.RemoveClaim(userProfileVM.Id, HasCompletedProfileWizardClaim);
                }

                UserManager.AddClaim(userProfileVM.Id, new Claim(_ClaimTypes.HasCompletedProfileWizard, true.ToString()));
                newClaimsList.Add(new Claim(_ClaimTypes.HasCompletedProfileWizard, true.ToString()));

                //Update Current Identity Claim and Login User Again
                if (newClaimsList.Count > 0)
                {
                    User.Identity.AddOrUpdateClaims(newClaimsList, owinContext.Authentication);
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
            return View(userProfileVM);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Step3()
        {
            return View();
        }

        public ActionResult UploadProfileImage(HttpPostedFileBase profileimage, string returnUrl)
        {
            string newProfileImageFilename = "";
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
                        newProfileImageFilename = profileImageName;
                        userAccountService.UpdateUserProfileImage(userProfile, false);
                    }
                }
            }


            if (Request.IsAjaxRequest())
            {
                return Json(new ResponseResult<string>
                {
                    Success = true,
                    Data = newProfileImageFilename
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index");
                }
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

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Follow(string UserId, bool DoFollow= true)
        {
            ResponseResult<object> responseResult = new ResponseResult<object>
            {
                Success = true
            };

            try
            {
                if (!String.IsNullOrEmpty(UserId))
                {
                    var currentUserId = User.Identity.GetUserId();

                    _dbContext = new ApplicationDbContext();
                    //Update the User Profile Details
                    UserAccountService userAccountService = new UserAccountService(_dbContext);
                    ResponseResult<UserFollowerVM> responseResultUserFollow = userAccountService.FollowUser(new UserFollowerVM
                    {
                        FollowByUserId = currentUserId,
                        FollowedUserId = UserId,
                        FollowedOn = DateTime.UtcNow
                    }, DoFollow);

                    return Json(responseResultUserFollow, JsonRequestBehavior.AllowGet);
                }

                responseResult.Success = false;
                responseResult.Message = "User is missing.";
            }
            catch (Exception err)
            {
                responseResult.Success = false;
                responseResult.Message = "An error occured while following this user. Please try again.";
            }

            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetFollowers(string UserId)
        {
            ResponseResult<SP_FollowersCount> responseResult = new ResponseResult<SP_FollowersCount>
            {
                Success = true
            };

            try
            {
                if (!String.IsNullOrEmpty(UserId))
                {
                    var currentUserId = User.Identity.GetUserId();

                    _dbContext = new ApplicationDbContext();
                    //Update the User Profile Details
                    UserAccountService userAccountService = new UserAccountService(_dbContext);
                    responseResult.Data = userAccountService.GetFollowersCount(UserId);

                    return Json(responseResult, JsonRequestBehavior.AllowGet);
                }

                responseResult.Success = false;
                responseResult.Message = "User is missing.";
            }
            catch (Exception err)
            {
                responseResult.Success = false;
                responseResult.Message = "An error occured. Please try again.";
            }

            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

        /***********************************/
        /* Dispose */
        /***********************************/
        private bool disposed = false;

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