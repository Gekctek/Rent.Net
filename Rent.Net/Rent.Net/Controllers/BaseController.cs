using Microsoft.AspNet.Identity;
using Rent.Net.Entities;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace Rent.Net.Controllers
{
    public class BaseController : Controller
    {
        public RentDbContext Database = new RentDbContext();

        public IQueryable<User> OtherUsers
        {
            get
            {
                string userName = this.User.Identity.Name;
                return this.Database.Users.Where(u => string.Equals(userName, u.UserName));
            }
        }

        public string UserId
        {
            get
            {
                return this.User.Identity.GetUserId();
            }
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
    public class BaseApiController : ApiController
    {
        public RentDbContext Database = new RentDbContext();

        public const string FilterName = "filter";
        public const string ReceivedFilter = "received";
        public const string SentFilter = "sent";

        public int UserId
        {
            get
            {
                User user = this.Database.Users.First(u => u.UserName == this.User.Identity.Name);
                return user.UserId;
            }
        }

        public IQueryable<User> OtherUsers
        {
            get
            {
                string userName = this.User.Identity.Name;
                return this.Database.Users.Where(u => !string.Equals(userName, u.UserName));
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.Database.Dispose();
            base.Dispose(disposing);
        }
    }
}

namespace Rent.Net
{
    public class RentAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                //http://www.dotnet-tricks.com/Tutorial/mvc/G54G220114-Custom-Authentication-and-Authorization-in-ASP.NET-MVC.html
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}