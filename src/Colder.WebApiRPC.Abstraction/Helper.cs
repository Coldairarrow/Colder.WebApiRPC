using System;
using System.Reflection;

namespace Colder.WebApiRPC.Abstraction
{
    internal static class Helper
    {
        public static string GetRoute(MethodInfo methodInfo)
        {
            var actionRoute = methodInfo.GetCustomAttribute<RouteAttribute>();
            if (actionRoute == null || string.IsNullOrEmpty(actionRoute.Template))
                throw new Exception($"{methodInfo.DeclaringType.Name}.{methodInfo.Name}缺少Route");

            var controllerRoute = methodInfo.DeclaringType.GetCustomAttribute<RouteAttribute>();
            if (controllerRoute == null || string.IsNullOrEmpty(controllerRoute.Template))
                throw new Exception($"{methodInfo.DeclaringType.Name}缺少Route");

            string fullRoute = $"{controllerRoute.Template}/{actionRoute.Template}";

            return fullRoute.BuildUrl();
        }

        public static bool IsJsonParamter(MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Length == 1
                && !methodInfo.GetParameters()[0].ParameterType.IsSimpleType();
        }
    }
}
