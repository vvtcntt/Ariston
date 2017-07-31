using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ariston.Startup))]
namespace Ariston
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
