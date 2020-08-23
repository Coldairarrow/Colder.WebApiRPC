using Castle.DynamicProxy;
using Colder.WebApiRPC.Abstraction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Colder.WebApiRPC.Client
{
    /// <summary>
    /// 客户端工厂
    /// </summary>
    public static class WebApiRPCClientFactory
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <typeparam name="TService">服务接口</typeparam>
        /// <param name="httpClient">HttpClient</param>
        /// <param name="jsonSerializerSettings">自定义序列化配置</param>
        /// <returns></returns>
        public static TService GetClient<TService>(HttpClient httpClient, JsonSerializerSettings jsonSerializerSettings = null) where TService : class, IWebApiRPC
        {
            return _generator.CreateInterfaceProxyWithoutTarget<TService>(new ApiRPCClientProxyInterceptor(httpClient, jsonSerializerSettings).ToInterceptor());
        }
        private class ApiRPCClientProxyInterceptor : AsyncInterceptorBase
        {
            private readonly HttpClient _httpClient;
            private readonly JsonSerializerSettings _jsonSerializerSettings;
            public ApiRPCClientProxyInterceptor(HttpClient httpClient, JsonSerializerSettings jsonSerializerSettings)
            {
                _httpClient = httpClient;
                _jsonSerializerSettings = jsonSerializerSettings;
            }

            private async Task<object> InternelInterceptAsync(IInvocation invocation)
            {
                var path = Helper.GetRoute(invocation.Method);
                string body = string.Empty;
                MediaTypeHeaderValue contentType;
                if (Helper.IsJsonParamter(invocation.Method))
                {
                    body = JsonConvert.SerializeObject(invocation.Arguments[0]);
                    contentType = new MediaTypeHeaderValue("application/json");
                }
                else
                {
                    contentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    List<string> formParamters = new List<string>();
                    var paramters = invocation.Method.GetParameters().ToList();
                    for (int i = 0; i < paramters.Count; i++)
                    {
                        var name = paramters[i].Name;
                        var value = invocation.Arguments[i]?.ToString();

                        formParamters.Add($"{name}={value ?? string.Empty}");

                        body = string.Join("&", formParamters);
                    }
                }

                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent content = new StringContent(body);
                content.Headers.ContentType = contentType;
                HttpResponseMessage response = await _httpClient.PostAsync(path, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                var returnType = invocation.Method.ReturnType;
                if (returnType == typeof(void) || returnType == typeof(Task))
                {
                    return null;
                }
                else if (typeof(Task).IsAssignableFrom(returnType) && returnType.IsGenericType)
                {
                    var dataType = returnType.GetGenericArguments()[0];
                    object returnData = JsonConvert.DeserializeObject(responseBody, dataType, _jsonSerializerSettings);
                    invocation.ReturnValue = Task.FromResult(returnData);
                    return returnData;
                }
                else if (!typeof(Task).IsAssignableFrom(returnType))
                {
                    var dataType = returnType;
                    object returnData = JsonConvert.DeserializeObject(responseBody, dataType, _jsonSerializerSettings);
                    invocation.ReturnValue = returnData;
                    return returnData;
                }

                return null;
            }

            protected override async Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
            {
                await InternelInterceptAsync(invocation);
            }

            protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
            {
                var data = await InternelInterceptAsync(invocation);

                return await Task.FromResult((TResult)data);
            }
        }
    }
}
