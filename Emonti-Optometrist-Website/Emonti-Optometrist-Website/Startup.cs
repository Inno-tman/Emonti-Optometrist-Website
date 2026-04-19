using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Emonti_Optometrist_Website.Startup))]
namespace Emonti_Optometrist_Website
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
