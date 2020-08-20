using Colder.WebApiRPC.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Colder.WebApiRPC.Hosting
{
    public class WebApiRPCConvention : IApplicationModelConvention
    {
        private static readonly Assembly _mvcCoreAssembly = Assembly.Load("Microsoft.AspNetCore.Mvc.Core");
        public void Apply(ApplicationModel application)
        {
            foreach (var aController in application.Controllers.ToList())
            {
                var type = aController.ControllerType.AsType();
                if (!typeof(IWebApiRPC).IsAssignableFrom(aController.ControllerType))
                    continue;

                //ApiExplorer
                if (aController.ApiExplorer.GroupName.IsNullOrEmpty())
                {
                    aController.ApiExplorer.GroupName = aController.ControllerName;
                }

                if (aController.ApiExplorer.IsVisible == null)
                {
                    aController.ApiExplorer.IsVisible = true;
                }

                foreach (var aAction in aController.Actions)
                {
                    if (aAction.ApiExplorer.IsVisible == null)
                    {
                        aAction.ApiExplorer.IsVisible = true;
                    }
                }

                //Action
                foreach (var aAction in aController.Actions)
                {
                    //参数校验
                    List<string> actionFilters = new List<string>
                    {
                        "Microsoft.AspNetCore.Mvc.Infrastructure.ClientErrorResultFilterFactory",
                        "Microsoft.AspNetCore.Mvc.Infrastructure.ModelStateInvalidFilterFactory"
                    };
                    actionFilters.ForEach(aFilterTypeString =>
                    {
                        var filterType = _mvcCoreAssembly.GetType(aFilterTypeString);
                        aAction.Filters.Add(Activator.CreateInstance(filterType) as IFilterMetadata);
                    });

                    //统一POST
                    var actionSelectorModel = aAction.Selectors[0];
                    actionSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { "POST" }));
                    actionSelectorModel.EndpointMetadata.Add(new HttpPostAttribute());

                    foreach (var selector in aAction.Selectors)
                    {
                        string route = $"{aController.ControllerName}/{aAction.ActionName}".Replace("//", "/");
                        selector.AttributeRouteModel = new AttributeRouteModel(new Microsoft.AspNetCore.Mvc.RouteAttribute(route));
                    }

                    //JSON支持,只有一个参数并且为复杂类型
                    if (aAction.Parameters.Count == 1 && !aAction.Parameters[0].ParameterType.IsSimpleType())
                    {
                        aAction.Parameters[0].BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                    }
                }
            }
        }
    }
}