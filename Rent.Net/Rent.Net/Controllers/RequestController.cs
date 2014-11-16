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
    public class RequestController : BaseController
    {

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult Send()
        {
            Request request = new Request { PayeeId = this.UserId };
            return this.SendView(request);
        }

        private ActionResult SendView(Request request)
        {
            this.AddUsersToViewBag();
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
    }
}