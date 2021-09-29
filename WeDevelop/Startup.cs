using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeDevelop.Startup))]
namespace WeDevelop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
