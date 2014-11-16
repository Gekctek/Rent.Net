using Rent.Net.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Rent.Net.Common;
using System.Web.Http;

namespace Rent.Net.Controllers
{
    public class RequestController : BaseController
    {

        public ActionResult Index()
        {
            return this.View();
        }

        [System.Web.Mvc.HttpGet]
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

        [System.Web.Mvc.HttpPost]
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


namespace Rent.Net.ApiControllers
{
    public class RequestController : BaseApiController
    {
        public IHttpActionResult Delete(int id)
        {
            Request request = this.Database.Requests.FirstOrDefault(r => r.RequestId == id);
            if(request == null)
            {
                return this.BadRequest("Request does not exist with the id of " + id);
            }
            this.Database.Entry(request).State = System.Data.Entity.EntityState.Deleted;
            this.Database.SaveChanges();
            return this.Ok();
        }
    }
}