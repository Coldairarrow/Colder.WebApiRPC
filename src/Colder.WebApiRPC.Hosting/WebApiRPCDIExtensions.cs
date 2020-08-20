using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Colder.WebApiRPC.Hosting
{
    public static class WebApiRPCDIExtensions
    {
        public static IMvcBuilder AddWebApiRPC(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.PartManager.FeatureProviders.Add(new WebApiRPCProvider());

            mvcBuilder.Services.Configure<MvcOptions>(o =>
            {
                o.Conventions.Add(new WebApiRPCConvention());
            });

            return mvcBuilder;
        }
    }
}