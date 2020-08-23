using Colder.WebApiRPC.Abstraction;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Common
{
    [Route("test")]
    [Description("测试接口")]
    public interface ITestService : IWebApiRPC
    {
        [Route("hello")]
        UserInfoDTO Hello(UserInfoDTO input);

        [Route("hi")]
        Task<string> Hi(string userId, string userName);

        [Route("isok")]
        Task<bool> IsOK(string userId, string userName);
    }
}
