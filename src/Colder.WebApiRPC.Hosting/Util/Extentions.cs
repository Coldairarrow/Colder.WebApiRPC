using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Colder.WebApiRPC.Hosting
{
    internal static class Extentions
    {
        public static bool IsSimpleType(this Type type)
        {
            return
                type.IsPrimitive ||
                new Type[]
                {
                    typeof(string),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                type.IsEnum ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
                ;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            return (T)services.FirstOrDefault(d => d.ServiceType == typeof(T))?.ImplementationInstance;
        }
    }
}
