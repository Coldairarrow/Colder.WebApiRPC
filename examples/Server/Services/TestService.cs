using Server.Models;

namespace Server.Services
{
    public class TestService : ITestService
    {
        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public UserInfoDTO Hello(UserInfoDTO input)
        {
            return input;
        }
    }
}
