using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SimpleBlog.Startup))]
namespace SimpleBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
