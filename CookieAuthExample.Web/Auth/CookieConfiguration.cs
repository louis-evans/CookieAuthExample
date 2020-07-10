using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace CookieAuthExample.Web.Auth
{
    public class CookieConfiguration : IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        public void Configure(string name, CookieAuthenticationOptions options)
        {
            
        }

        public void Configure(CookieAuthenticationOptions options) => Configure(Options.DefaultName, options);
    }
}