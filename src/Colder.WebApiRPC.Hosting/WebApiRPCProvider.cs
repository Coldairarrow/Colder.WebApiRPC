using Colder.WebApiRPC.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Colder.WebApiRPC.Hosting
{
    public class WebApiRPCProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            return (typeof(IWebApiRPC).IsAssignableFrom(typeInfo)
                || typeof(ControllerBase).IsAssignableFrom(typeInfo))
                && !typeInfo.IsAbstract
                && !typeInfo.IsInterface;
        }
    }
}