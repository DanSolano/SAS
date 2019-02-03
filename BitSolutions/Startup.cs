using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BitSolutions.Startup))]
namespace BitSolutions
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
