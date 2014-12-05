using Microsoft.AspNet.Identity;
using Rent.Net.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Rent.Net.Controllers
{
    [System.Web.Mvc.Authorize]
    public class BaseController : Controller
    {
        public RentDbContext Database = new RentDbContext();

        public IQueryable<ApplicationUser> OtherUsers
        {
            get
            {
                string userId = this.UserId;
                return this.Database.Users.Where(u => u.Id != userId);
            }
        }

        public string UserId
        {
            get
            {
                return this.User.Identity.GetUserId();
            }
        }

        public void AddUsersToViewBag()
        {
            this.ViewBag.Users = new SelectList(this.OtherUsers, "Id", "UserName", null);
        }
        protected override void Dispose(bool disposing)
        {
            this.Database.Dispose();
            base.Dispose(disposing);
        }
    }
}

namespace Rent.Net.ApiControllers
{
    [System.Web.Http.Authorize]
    public class BaseApiController : ApiController
    {
        public RentDbContext Database = new RentDbContext();

        public const string FilterName = "filter";
        public const string ReceivedFilter = "received";
        public const string SentFilter = "sent";

        public string UserId
        {
            get
            {
                return this.User.Identity.GetUserId();
            }
        }
        
        public IQueryable<ApplicationUser> OtherUsers
        {
            get
            {
                string userId = this.UserId;
                return this.Database.Users.Where(u => u.Id != userId);
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.Database.Dispose();
            base.Dispose(disposing);
        }
    }
}