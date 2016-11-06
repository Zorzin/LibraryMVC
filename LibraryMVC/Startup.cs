using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibraryMVC.Startup))]
namespace LibraryMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
