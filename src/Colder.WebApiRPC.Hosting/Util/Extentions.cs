using Colder.WebApiRPC.Abstraction;
using System;

namespace Colder.WebApiRPC.Hosting
{
    internal static class Extentions
    {
        public static bool IsWebApiRPCController(this Type type)
        {
            return typeof(IWebApiRPC).IsAssignableFrom(type)
                && !type.IsAbstract
                && !type.IsGenericType
                && !type.IsInterface;
        }
    }
}
