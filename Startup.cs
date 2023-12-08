using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(James_Thew.Startup))]
namespace James_Thew
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
