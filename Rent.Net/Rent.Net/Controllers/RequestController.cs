using Rent.Net.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Rent.Net.Common;

namespace Rent.Net.Controllers
{
    public class RequestController : Controller
    {
        public RentDbContext Database = new RentDbContext();

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult Send()
        {
            Request request = new Request { PayeeId = this.User.Identity.GetUserId() };
            return this.SendView(request);
        }

        private ActionResult SendView(Request request)
        {
            this.ViewBag.Users = new SelectList(this.Database.Users, "Id", "UserName", null);
            return this.View(request);
        }

        [HttpPost]
        public ActionResult Send(Request request)
        {
            if (this.ModelState.IsValid)
            {
                this.Database.Requests.Add(request);
                this.Database.SaveChanges();
                return this.RedirectToAction("Index", "Home", null);
            }
            else
            {
                return this.SendView(request);
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.Database.Dispose();
            base.Dispose(disposing);
        }
    }

}