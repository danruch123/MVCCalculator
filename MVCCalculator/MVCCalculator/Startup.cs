using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCCalculator.Startup))]
namespace MVCCalculator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
