using Common;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        [HttpPost]
        public UserInfoDTO Hello([FromBody] UserInfoDTO input)
        {
            return input;
        }
    }
}
