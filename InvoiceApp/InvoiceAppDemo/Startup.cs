using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InvoiceAppDemo.Startup))]
namespace InvoiceAppDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
