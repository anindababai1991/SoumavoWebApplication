using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SoumavoWebApplication.Startup))]
namespace SoumavoWebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
