using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rent.Net.Startup))]
namespace Rent.Net
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
