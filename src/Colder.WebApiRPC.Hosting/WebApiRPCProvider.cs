using Colder.WebApiRPC.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Colder.WebApiRPC.Hosting
{
    internal class WebApiRPCProvider : ControllerFeatureProvider
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