using Microsoft.Owin;
using Owin;
[assembly: OwinStartupAttribute(typeof(Ptpn8.Startup))]
namespace Ptpn8
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}