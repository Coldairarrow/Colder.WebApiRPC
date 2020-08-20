using Microsoft.AspNetCore.Mvc;
using Server.Models;

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
