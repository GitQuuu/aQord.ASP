using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(aQord.ASP.Startup))]
namespace aQord.ASP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
