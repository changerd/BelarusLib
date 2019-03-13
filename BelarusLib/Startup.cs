using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BelarusLib.Startup))]
namespace BelarusLib
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
