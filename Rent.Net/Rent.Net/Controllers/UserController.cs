using Rent.Net.ApiControllers;
using System.Linq;
using System.Web.Http;

namespace Rent.Net.Controllers
{
    public class UserController : BaseApiController
    {
        public IHttpActionResult Get()
        {
            return this.Ok(this.OtherUsers.ToList());
        }
    }
}
