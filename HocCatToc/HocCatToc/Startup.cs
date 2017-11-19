using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HocCatToc.Startup))]
namespace HocCatToc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
