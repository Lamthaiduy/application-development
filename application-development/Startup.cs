using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(application_development.Startup))]
namespace application_development
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
