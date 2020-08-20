using Colder.WebApiRPC.Abstraction;
using Server.Models;

namespace Server.Services
{
    public interface ITestService: IWebApiRPC
    {
        UserInfoDTO Hello(UserInfoDTO input);
    }
}
