using Colder.WebApiRPC.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Colder.WebApiRPC.Server
{
    /// <summary>
    /// 注入拓展
    /// </summary>
    public static class WebApiRPCDIExtensions
    {
        private static void Validate()
        {
            //校验,每个接口与方法必须有Route特性
            //每个实现类只能实现一个接口
            //接口中方法名不能重复,路由不能重复
            var implements = AssemblyHelper.AllTypes.Where(x => x.IsWebApiRPCImplement()).ToList();
            implements.ForEach(aImplement =>
            {
                var interfaces = aImplement.GetInterfaces().Where(x => x.IsWebApiRPCInterface()).ToList();
                if (interfaces.Count > 1)
                    throw new Exception($"{aImplement}禁止实现多个接口");

                if (interfaces.Count == 0)
                    throw new Exception($"{aImplement}未实现接口");

                var theInterface = interfaces.FirstOrDefault();
                if (theInterface.GetCustomAttribute<Abstraction.RouteAttribute>() == null)
                    throw new Exception($"{theInterface}必须定义RouteAttribute");

                var methods = theInterface.GetMethods().ToList();
                methods.ForEach(aMethod =>
                {
                    if (aMethod.GetCustomAttribute<Abstraction.RouteAttribute>() == null)
                        throw new Exception($"{theInterface}.{aMethod.Name}必须定义RouteAttribute");
                });

                if (methods.Count != methods.Select(x => x.Name).Distinct().Count())
                    throw new Exception($"{theInterface}禁止方法名重复");
                if (methods.Count !=
                    methods.Select(x => x.GetCustomAttribute<Abstraction.RouteAttribute>().Template).Distinct().Count())
                    throw new Exception($"{theInterface}禁止路由重复");
            });
        }

        /// <summary>
        /// 注入WebApiRPC
        /// </summary>
        /// <param name="mvcBuilder">IMvcBuilder</param>
        /// <param name="routePrefix">路由前缀</param>
        /// <returns></returns>
        public static IMvcBuilder AddWebApiRPC(this IMvcBuilder mvcBuilder, string routePrefix = null)
        {
            Validate();

            Constant.RoutePrefix = routePrefix;

            //扫描程序集
            var parts = AssemblyHelper.AllTypes.Where(x => x.IsWebApiRPCImplement())
                .Select(x => x.Assembly)
                .Distinct()
                .Select(x => new AssemblyPart(x))
                .ToList();
            parts.ForEach(aPart =>
            {
                mvcBuilder.PartManager.ApplicationParts.Add(aPart);
            });

            mvcBuilder.PartManager.FeatureProviders.Add(new WebApiRPCProvider());
            mvcBuilder.Services.Configure<MvcOptions>(o =>
            {
                o.Conventions.Add(new WebApiRPCConvention());
            });

            return mvcBuilder;
        }
    }
}