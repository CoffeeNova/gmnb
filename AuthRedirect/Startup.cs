using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AuthRedirect.Startup))]
namespace AuthRedirect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
