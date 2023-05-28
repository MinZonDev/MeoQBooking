using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBooking.Startup))]
namespace WebBooking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
