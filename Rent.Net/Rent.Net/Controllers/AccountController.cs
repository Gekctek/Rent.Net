using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Rent.Net.Models;
using Rent.Net.Entities;
using System;
using System.Web.Security;
using System.Web;

namespace Rent.Net.Controllers
{
    public class AccountController : BaseController
    {
        public static int Timeout = 30; // Minutes

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string encodedPassword = Entities.User.EncodePassword(model.Password);
            User user = this.Database.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == encodedPassword);
            if(user == null)
            {
                ModelState.AddModelError("", "Incorrect username and/or password");
                return this.View(model);
            }
            this.LoginInternal(user);
            return this.RedirectToAction("Index", "Home");
        }

        private void LoginInternal(User user)
        {
            FormsAuthentication.SetAuthCookie(user.UserName, true);
        }

        private void LogOffInternal()
        {
            FormsAuthentication.SignOut();
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
        public ActionResult Register(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            User user = new User(model.UserName, model.Password);
            user = this.Database.Users.Add(user);
            this.Database.SaveChanges();
            this.LoginInternal(user);
            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User user = this.Database.Users.FirstOrDefault(u => u.Password == model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "User doesn't exist.");
                return this.View(model);
            }
            user.Password = Entities.User.EncodePassword(model.Password);
            this.Database.SaveChanges();
            return this.RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public ActionResult LogOff()
        {
            this.LogOffInternal();
            return RedirectToAction("Index", "Home");
        }
    }
}