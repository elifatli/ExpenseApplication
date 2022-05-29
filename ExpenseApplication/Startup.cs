using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExpenseApplication.Startup))]
namespace ExpenseApplication
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
