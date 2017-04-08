using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IPLFUN.Startup))]
namespace IPLFUN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
        }
    }
}
