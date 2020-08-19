using System;

namespace Colder.WebApiRPC.Hosting.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class NonDynamicWebApiAttribute:Attribute
    {
        
    }
}