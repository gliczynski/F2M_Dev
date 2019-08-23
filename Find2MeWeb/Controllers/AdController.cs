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

        public ActionResult AdWizardStep1()
        {
            return View();
        }

        public ActionResult UploadAdImage(HttpPostedFileBase adImage, string returnUrl, int adImageNumber)
        {
            string newAdImageFilename = "";
            int adID = 0;
            if (adImage != null)
            {
                if (adImage.ContentLength > 0)
                {
                    _dbContext = new ApplicationDbContext();
                    var userAdService = new UserAdService(_dbContext);

                    //Save an Ad first and get newly created Ad's ID
                    var userAdVM = new UserAdVM();
                    userAdVM.UserId = User.Identity.GetUserId();
                    userAdVM.CreatedOn = DateTime.UtcNow;
                    adID = userAdService.CreateAd(userAdVM);


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
                    var userAdImageVM = new UserAdImageVM();
                    userAdImageVM.AdImageOriginal = adImageName;
                    userAdImageVM.AdImageSelected = adImageName;
                    userAdImageVM.ImageNumber = adImageNumber;
                    userAdImageVM.CreatedOn = DateTime.UtcNow;
                    userAdImageVM.AdID = adID;
                    userAdService.AddUserAdImage(userAdImageVM);

                    newAdImageFilename = adImageName;


                    //userProfile.ProfileImageOriginal = profileImageName;
                    //userProfile.ProfileImageSelected = profileImageName;
                    //newProfileImageFilename = profileImageName;
                    //userAccountService.UpdateUserProfileImage(userProfile, false);

                    //Set the Image Uploaded 
                    TempData["ImageUploaded"] = true;
                    TempData["ResponseResult"] = new ResponseResult<object>
                    {
                        Success = true,
                        Message = "Ad Image is uploaded successfully.",
                        MessageCode = ResponseResultMessageCode.ProfileImageUploaded,
                    };
                }
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
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
    }
}
