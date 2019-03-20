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
                var currentUser = manager.FindById(User.Identity.GetUserId());

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
    }
}
