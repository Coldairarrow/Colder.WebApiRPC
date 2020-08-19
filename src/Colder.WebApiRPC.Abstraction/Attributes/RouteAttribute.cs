using System;

namespace Colder.WebApiRPC.Abstraction
{
    /// <summary>
    /// 仅支持简单路由,禁用api/[controller]这种高级路由
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class RouteAttribute : Attribute
    {
        public RouteAttribute(string template)
        {
            Template = template;
        }
        public string Template { get; }
    }
}
