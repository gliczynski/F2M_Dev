using Find2Me.Infrastructure.DbModels;
using Find2MeWeb.ActionFilters;
using Find2Me.Resources;
using Find2MeWeb.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Find2Me.Services;
using Find2Me.Infrastructure.ViewModels;
using System.Globalization;
using Find2Me.Infrastructure;
using Find2Me.Resources;

namespace Find2MeWeb.Controllers
{
  [Internationalization]
  public class AdController : Controller
  {
    private ApplicationDbContext _dbContext;
    // GET: Ad
    public ActionResult Index()
    {
      return View();
    }

    // GET: Ad/Details/5
    public ActionResult Details(int id)
    {
      return View();
    }

    // GET: Ad/Create
    // [Internationalization]
    public ActionResult Create()
    {
      _dbContext = new ApplicationDbContext();
      UserAdService userAdService = new UserAdService(_dbContext);
      var userAdList = new List<UserAdVM>();

      var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));
      var currentUser = manager.FindById(User.Identity.GetUserId());

      if (currentUser != null)
      {
        userAdList = userAdService.GetAllUserAds(currentUser.UserName);
      }

      return View(userAdList);
    }

    // POST: Ad/Create
    [HttpPost]
    public ActionResult Create(FormCollection collection)
    {
      try
      {
        _dbContext = new ApplicationDbContext();
        UserAdService userAdService = new UserAdService(_dbContext);

        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));

        userAdService.CreateAd(new UserAdVM
        {
          CreatedOn = DateTime.UtcNow,
          UserId = User.Identity.GetUserName()
        });

        return RedirectToAction("Create");
      }
      catch
      {
        return View();
      }
    }

    // GET: Ad/Edit/5
    public ActionResult Edit(int id)
    {
      return View();
    }

    // POST: Ad/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add update logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Ad/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: Ad/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add delete logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    #region AdWizardStep1
    [HttpGet]
    [Authorize]
    public ActionResult AdWizardStep1(int? adID)
    {
      _dbContext = new ApplicationDbContext();
      var userAdService = new UserAdService(_dbContext);
      UserAdImageVM adImageVM = new UserAdImageVM();

      if (TempData["ImageUploadedId"] != null)
      {
        adImageVM = userAdService.GetUserAdImageByAdImageId(Convert.ToInt32(TempData["ImageUploadedId"]));
      }

      // get user ad images 
      if (adID != null)
      {
        ViewBag.AdId = adID;
        ViewBag.AdImages = userAdService.GetUserAdImages((int)adID);
        ViewBag.TotalImagesUploaded = userAdService.GetUserAdImages((int)adID).Count;
        ViewBag.ImageNumber = ViewBag.TotalImagesUploaded + 1;
      }
      else
      {
        // if first image is not uploaded then set AdId = 0
        ViewBag.AdId = 0;
        ViewBag.AdImages = null;
        ViewBag.TotalImagesUploaded = 0;
        ViewBag.ImageNumber = 1;
      }

      if (adImageVM == null) { return HttpNotFound(); }

      if (TempData["ImageUploaded"] != null)
      {
        ViewBag.OpenEditProfileImageModel = true;
      }

      ViewBag.OpenEditProfileImageModel = false;

      // check if first image is uploaded or not
      if (adID != null)
      {
        // open the cropping tool based on image upload success
        ViewBag.OpenEditProfileImageModel = true;
      }

      return View(adImageVM);
    }
    #endregion

    #region UploadAdImage
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public ActionResult UploadAdImage(HttpPostedFileBase adImage, string returnUrl, int adImageNumber, int adID, string SubmitAction = "")
    {
      string newAdImageFilename = "";
      //int adID = 0;
      if (adImage != null)
      {
        if (adImage.ContentLength > 0)
        {
          _dbContext = new ApplicationDbContext();
          var userAdService = new UserAdService(_dbContext);

          //Save an Ad first and get newly created Ad's ID
          if (adID == 0)
          {
            var userAdVM = new UserAdVM
            {
              UserId = User.Identity.GetUserId(),
              CreatedOn = DateTime.UtcNow,
              IsDraft = true
            };
            adID = userAdService.CreateAd(userAdVM);
          }

          // UnComment After making it dynamic
          var userAdImage = userAdService.GetUserAdImages(adID).Where(w => w.ImageNumber == adImageNumber).FirstOrDefault();

          //Save New Ad Image and Remove Old Image
          string adImageName = DateTime.UtcNow.ToString("yyyyddmmhhmmss") + "_ad_original.jpg";
          string adImagePath = Server.MapPath("~" + _FileSavingPaths.UserAdsPath + "/" + adImageName);

          //Save New Image
          if (!System.IO.File.Exists(adImagePath))
          {
            adImage.SaveAs(adImagePath);
          }

          //Remove Old Image
          // UnComment After making it dynamic
          if (userAdImage != null)
          {
            try
            {
              //Delete Original and Selected Images 

              if (System.IO.File.Exists(Server.MapPath("~" + _FileSavingPaths.UserAdsPath + "/" + userAdImage.AdImageOriginal)))
              {
                System.IO.File.Delete(Server.MapPath("~" + _FileSavingPaths.UserAdsPath + "/" + userAdImage.AdImageOriginal));
              }
              if (System.IO.File.Exists(Server.MapPath("~" + _FileSavingPaths.UserAdsPath + "/" + userAdImage.AdImageSelected)))
              {
                System.IO.File.Delete(Server.MapPath("~" + _FileSavingPaths.UserAdsPath + "/" + userAdImage.AdImageSelected));
              }

            }
            catch { }
          }

          //Add user Ad image in database
          var userAdImageVM = new UserAdImageVM
          {
            AdImageOriginal = adImageName,
            AdImageSelected = adImageName,
            ImageNumber = adImageNumber,
            CreatedOn = DateTime.UtcNow,
            AdID = adID
          };
          var uploadedImageId = userAdService.AddUserAdImage(userAdImageVM);

          newAdImageFilename = adImageName;

          //// Increment the image number after saving the previous image
          //ViewBag.

          //userProfile.ProfileImageOriginal = profileImageName;
          //userProfile.ProfileImageSelected = profileImageName;
          //newProfileImageFilename = profileImageName;
          //userAccountService.UpdateUserProfileImage(userProfile, false);

          //Set the Image Uploaded 
          TempData["ImageUploaded"] = true;
          TempData["ImageUploadedId"] = uploadedImageId;
          TempData["ResponseResult"] = new ResponseResult<object>
          {
            Success = true,
            Message = "Ad Image is uploaded successfully.",
            MessageCode = ResponseResultMessageCode.ProfileImageUploaded,
          };
        }
      }

      if (SubmitAction.ToLower().Equals(Strings.Next.ToLower()) && adID != 0)
      {
        // store the adId in tempData
        //TempData["AdId"] = adID;
        return RedirectToAction("AdWizardStep2", "Ad", new { adID });
      }

      if (Request.IsAjaxRequest())
      {
        return Json(new ResponseResult<string>
        {
          Success = true,
          Data = newAdImageFilename
        }, JsonRequestBehavior.AllowGet);
      }
      else
      {
        if (!string.IsNullOrEmpty(returnUrl))
        {
          if (adImage == null)
          {
            TempData["ResponseResult"] = new ResponseResult<object>
            {
              Success = false,
              Message = "Please upload atleast one image to continue to the next step.",
              MessageCode = ResponseResultMessageCode.ProfileImageUploaded,
            };

            return Redirect(returnUrl);
          }

          return RedirectToAction("AdWizardStep1", "Ad", new { adID });
        }
        else
        {
          return RedirectToAction("Index");
        }
      }
    }
    #endregion

    #region AdWizardStep2
    [HttpGet]
    [Authorize]
    public ActionResult AdWizardStep2(int adID)
    {
      _dbContext = new ApplicationDbContext();

      UserAdService userAdService = new UserAdService(_dbContext);
      CategoryService categoryService = new CategoryService(_dbContext);
      var userAdInfoVM = userAdService.GetUserAdInfoByAdId(adID);

      ViewBag.Category = categoryService.GetAllCategories();
      ViewBag.AdId = adID;
      return View(userAdInfoVM);
    }
    #endregion

    #region AdWizardStep2(Form Submission)
    [HttpPost]
    [Authorize]
    public ActionResult AdWizardStep2(UserAdInformationVM userAdInfo, string SubmitAction = "")
    {
      // let the adId
      var adID = userAdInfo.AdID;
      // if back button is pressed then go to the previous step (Ad Wizard Step 1)
      if (SubmitAction.ToLower().Equals(Strings.Back.ToLower()))
      {
        return RedirectToAction("AdWizardStep1", "Ad", new { adID });
      }

      _dbContext = new ApplicationDbContext();
      UserAdService userAdService = new UserAdService(_dbContext);
      CategoryService categoryService = new CategoryService(_dbContext);
      ViewBag.Category = categoryService.GetAllCategories();
      if (!ModelState.IsValid)
      {
        userAdService.UpdateUserAdInfo(userAdInfo);
      }

      if (SubmitAction.ToLower().Equals(Strings.Next.ToLower()))
      {
        return RedirectToAction("AdWizardStep3", "Ad", new { adID });
      }

      return RedirectToAction("AdWizardStep2", "Ad", new { adID });
    }
    #endregion

    #region DeleteImage
    public ActionResult DeleteImage(int imageId)
    {
      try
      {
        _dbContext = new ApplicationDbContext();
        UserAdService userAdService = new UserAdService(_dbContext);

        var userAdImage = userAdService.GetUserAdImageByAdImageId(imageId);

        if (userAdImage != null)
        {
          if (System.IO.File.Exists(Server.MapPath("~" + _FileSavingPaths.UserAdsPath + "/" + userAdImage.AdImageOriginal)))
          {
            System.IO.File.Delete(Server.MapPath("~" + _FileSavingPaths.UserAdsPath + "/" + userAdImage.AdImageOriginal));
          }
          if (System.IO.File.Exists(Server.MapPath("~" + _FileSavingPaths.UserAdsPath + "/" + userAdImage.AdImageSelected)))
          {
            System.IO.File.Delete(Server.MapPath("~" + _FileSavingPaths.UserAdsPath + "/" + userAdImage.AdImageSelected));
          }

          // delete the image and get back the id
          var userAdImageId = userAdService.DeleteUserAdImage(imageId);

          // show a success message
          TempData["ResponseResult"] = new ResponseResult<object>
          {
            Success = true,
            Message = "The image has been deleted successfully.",
            MessageCode = ResponseResultMessageCode.ProfileImageUploaded,
          };

          return RedirectToAction("AdWizardStep1", "Ad", new { userAdImage.AdID });
        }

        return Json(-1, JsonRequestBehavior.AllowGet);
      }
      catch (Exception ex)
      {
        ViewBag.Error = ex.InnerException + ex.Message;
        return View("Error");
      }

    }
    #endregion

    #region AdWizardStep3
    [HttpGet]
    [Authorize]
    //[Authorize]
    public ActionResult AdWizardStep3(int adID)
    {
      try
      {
        _dbContext = new ApplicationDbContext();

        // get user Id
        string currentUserId = User.Identity.GetUserId();
        // define service
        UserAccountService userAccountService = new UserAccountService(_dbContext);
        UserAdService userAdService = new UserAdService(_dbContext);
        ConfigurationService configurationService = new ConfigurationService(_dbContext);
        CurrencyService currencyService = new CurrencyService(_dbContext);

        // get useradPriceReward
        var userAdPriceReward = userAdService.GetUserAdPriceRewardByAdId(adID);
        // get user preferred currency
        var currencyUser = userAccountService.GetUserProfileById(currentUserId).PreferredCurrency;
        // get exclusive ad period
        ViewBag.ExclusiveAdPriceOneDay = configurationService.GetConfigurationByConfigurationName("wiz_ad_p3_exlusive_ad_one_day").Value;
        ViewBag.ExclusiveAdPriceTwoDays = configurationService.GetConfigurationByConfigurationName("wiz_ad_p3_exlusive_ad_two_days").Value;
        ViewBag.ExclusiveAdPriceOneWeek = configurationService.GetConfigurationByConfigurationName("wiz_ad_p3_exlusive_ad_one_week").Value;
        ViewBag.ExclusiveAdPriceTwoWeeks = configurationService.GetConfigurationByConfigurationName("wiz_ad_p3_exlusive_ad_two_weeks").Value;
        ViewBag.ExclusiveAdPriceOneMonth = configurationService.GetConfigurationByConfigurationName("wiz_ad_p3_exlusive_ad_one_month").Value;
        ViewBag.ExclusiveAdPriceTwoMonths = configurationService.GetConfigurationByConfigurationName("wiz_ad_p3_exlusive_ad_two_months").Value;
        ViewBag.ExclusiveAdPriceThreeMonths = configurationService.GetConfigurationByConfigurationName("wiz_ad_p3_exlusive_ad_three_months").Value;
        ViewBag.ExclusiveAdPriceSixMonths = configurationService.GetConfigurationByConfigurationName("wiz_ad_p3_exlusive_ad_six_months").Value;
        ViewBag.ExclusiveAdPriceOneYear = configurationService.GetConfigurationByConfigurationName("wiz_ad_p3_exlusive_ad_one_year").Value;
        // get configurations for ad
        ViewBag.AdPrice = configurationService.GetConfigurationByConfigurationName("wiz_ad_p3_ad_price").Value;
        // get currency symbol by user currency
        ViewBag.Currency = currencyService.GetCurrencyByCurrencyCode(currencyUser).Symbol;

        ViewBag.AdId = adID;
        return View();
      }
      catch (Exception ex)
      {
        ViewBag.Error = ex.InnerException + ex.Message;
        return View("Error");
      }
    }
    #endregion

    #region AdWizardStep3(Form Submission)
    [HttpPost]
    [Authorize]
    public ActionResult AdWizardStep3(UserAdPriceRewardVM userAdPriceReward, int AdId, string SubmitAction = "")
    {
      _dbContext = new ApplicationDbContext();
      UserAdService userAdService = new UserAdService(_dbContext);

      var adID = AdId;
      // if back button is pressed then go to the previous step (Ad Wizard Step 2)
      if (SubmitAction.ToLower().Equals(Strings.Back.ToLower()))
      {
        return RedirectToAction("AdWizardStep2", "Ad", new { adID });
      }

      // Backend code goes here
      if (!ModelState.IsValid)
      {
        // save the data
        userAdService.UpdateUserAdPriceReward(userAdPriceReward);
      }

      // if next button is pressed then go to the next step (Ad Wizard Step 4)
      if (SubmitAction.ToLower().Equals(Strings.Next.ToLower()))
      {
        return RedirectToAction("AdWizardStep4", "Ad", new { adID });
      }

      // TODO: Change To Dynamic, Change back to AdWizardStep3 when next button is dynamic
      return RedirectToAction("AdWizardStep4", "Ad", new { adID });
    }
    #endregion

    #region AdWizardStep4
    [HttpGet]
    [Authorize]
    public ActionResult AdWizardStep4(int adID)
    {
      try
      {
        _dbContext = new ApplicationDbContext();

        // get user Id
        string currentUserId = User.Identity.GetUserId();
        // define service
        UserAccountService userAccountService = new UserAccountService(_dbContext);
        UserAdService userAdService = new UserAdService(_dbContext);
        ConfigurationService configurationService = new ConfigurationService(_dbContext);
        CurrencyService currencyService = new CurrencyService(_dbContext);

        // Get The User
        ViewBag.User = userAccountService.GetUserProfileById(currentUserId);
        // Get the ad info
        ViewBag.AdInfo = userAdService.GetUserAdInfoByAdId(adID);
        ViewBag.AdInfoImages = userAdService.GetUserAdImages(adID)[0];
        ViewBag.AdPriceReward = userAdService.GetUserAdPriceRewardByAdId(adID);
        // get user preferred currency
        var currencyUser = userAccountService.GetUserProfileById(currentUserId).PreferredCurrency;
        // get currency symbol by user currency
        ViewBag.Currency = currencyService.GetCurrencyByCurrencyCode(currencyUser).Symbol;

        ViewBag.AdId = adID;
        return View();
      }
      catch (Exception ex)
      {
        ViewBag.Error = ex.InnerException + ex.Message;
        return View("Error");
      }
    }
    #endregion

    #region AdWizardStep4(Form Submission)
    [HttpPost]
    [Authorize]
    public ActionResult AdWizardStep4(int AdId, string SubmitAction = "")
    {
      var adID = AdId;
      // if back button is pressed then go to the previous step (Ad Wizard Step 2)
      if (SubmitAction.ToLower().Equals(Strings.Back.ToLower()))
      {
        return RedirectToAction("AdWizardStep3", "Ad", new { adID });
      }

      // Backend code goes here

      // if next button is pressed then go to the next step (Ad Wizard Step 4)
      if (SubmitAction.ToLower().Equals(Strings.Next.ToLower()))
      {
        return RedirectToAction("AdWizardStep5", "Ad", new { adID });
      }

      return RedirectToAction("AdWizardStep4", "Ad", new { adID });
    }
    #endregion

    #region AdWizardStep5
    [HttpGet]
    [Authorize]
    public ActionResult AdWizardStep5(int adID)
    {
      try
      {// get user Id
        string currentUserId = User.Identity.GetUserId();
        // define service
        UserAccountService userAccountService = new UserAccountService(_dbContext);
        ViewBag.User = userAccountService.GetUserProfileById(currentUserId);

        return View();
      }
      catch (Exception ex)
      {
        ViewBag.Error = ex.InnerException + ex.Message;
        return View("Error");
      }
    }
    #endregion

    #region MyAds    
    [HttpGet]
    [Authorize]
    public ActionResult MyAds()
    {
      try
      {
        _dbContext = new ApplicationDbContext();

        // Get the current logged in user id
        string currentUserId = User.Identity.GetUserId();

        UserAdService userAdService = new UserAdService(_dbContext);
        CategoryService categoryService = new CategoryService(_dbContext);
        var userAds = userAdService.GetAllUserAds(currentUserId);

        ViewBag.UserAds = userAds;

        return View();
      }
      catch (Exception ex)
      {
        ViewBag.Error = ex.InnerException + ex.Message;
        return View("Error");
      }
    }
    #endregion

  }
}
