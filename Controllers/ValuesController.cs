using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library77.Controllers
{
    public class ValuesController : Controller
    {
        [ApiController]
        [Route("api/[controller]")]
        public class ValuesControlle : Controller
        {
            [Authorize]
            [Route("getlogin")]
            public IActionResult GetLogin()
            {
                return Ok($"Ваш логин: {User.Identity.Name}");
            }

            [Authorize(Roles = "admin")]
            [Route("getrole")]
            public IActionResult GetRole()
            {
                return Ok("Ваша роль: администратор");
            }
        }
    }
}
