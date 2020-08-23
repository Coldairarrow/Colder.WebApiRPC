using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Colder.WebApiRPC.Abstraction
{
    internal static class AssemblyHelper
    {
        static AssemblyHelper()
        {
            string rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var assemblies = Directory.GetFiles(rootPath, "*.dll")
                .Where(x => !new FileInfo(x).Name.StartsWith("System.")
                    && !new FileInfo(x).Name.StartsWith("Microsoft."))
                .Select(x => Assembly.LoadFrom(x))
                .Where(x => !x.IsDynamic)
                .ToList();

            assemblies.ForEach(aAssembly =>
            {
                try
                {
                    AllTypes.AddRange(aAssembly.GetTypes());
                    AllAssemblies.Add(aAssembly);
                }
                catch
                {

                }
            });
        }

        /// <summary>
        /// 解决方案所有程序集
        /// </summary>
        public static readonly List<Assembly> AllAssemblies = new List<Assembly>();

        /// <summary>
        /// 解决方案所有自定义类
        /// </summary>
        public static readonly List<Type> AllTypes = new List<Type>();
    }
}
