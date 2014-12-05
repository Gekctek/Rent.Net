using Rent.Net.ApiControllers;
using System.Linq;
using System.Web.Http;

namespace Rent.Net.Controllers
{
    public class UserController : BaseApiController
    {
        public IHttpActionResult Get()
        {
            var users = this.OtherUsers.Select(u => new { UserName = u.UserName, Id = u.Id });
            return this.Ok(users);
        }
    }
}
