using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Colder.WebApiRPC.Hosting
{
    public class WebApiRPCControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            return typeInfo.IsWebApiRPCController();
        }
    }
}