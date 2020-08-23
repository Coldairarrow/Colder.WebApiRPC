using Common;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Server.Services
{
    internal class TestService : ITestService
    {
        private readonly ILogger _logger;
        public TestService(ILogger<TestService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// hello
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public UserInfoDTO Hello(UserInfoDTO input)
        {
            _logger.LogInformation("Id:{Id} Name:{Name}", input.id, input.name);
            return input;
        }

        /// <summary>
        /// hi
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<string> Hi(string userId, string userName)
        {
            return await Task.FromResult($"{userName} Say Hi");
        }

        public async Task<bool> IsOK(string userId, string userName)
        {
            return await Task.FromResult(true);
        }
    }
}
