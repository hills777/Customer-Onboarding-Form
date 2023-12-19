using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PractiseController : ControllerBase
    {
        [HttpPost]
        public string HelloWord()
        {
            return "Hello World.";
        }
    }
}
