using Microsoft.Owin;
using Owin;
using SimpleBlog.Migrations;
using SimpleBlog.Models;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(SimpleBlog.Startup))]
namespace SimpleBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlogDbContext, Configuration>()); 
            ConfigureAuth(app);
        }
    }
}
