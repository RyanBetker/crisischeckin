using System;
using System.IdentityModel.Selectors;
using Microsoft.IdentityModel.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(crisicheckinweb.App_Start.ClaimsCookieConfig), "PreAppStart")]

namespace crisicheckinweb.App_Start
{
    public static class ClaimsCookieConfig
    {
        public static void PreAppStart()
        {
            DynamicModuleUtility.RegisterModule(typeof(ClaimsCookie.ClaimsCookieModule));
        }
    }
}