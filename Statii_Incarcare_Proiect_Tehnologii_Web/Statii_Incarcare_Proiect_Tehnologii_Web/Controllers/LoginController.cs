using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Statii_Incarcare_Proiect_Tehnologii_Web.Context;
using Statii_Incarcare_Proiect_Tehnologii_Web.Dto;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly StatiiIncarcareContext _incarcareContext;

        public LoginController(StatiiIncarcareContext context)
        {
            _incarcareContext = context;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userModel)
        {
            var user = await _incarcareContext.Users.FirstAsync(u => u.name == userModel.email && u.password == userModel.password);
            if (user == null)
                return Unauthorized(new { message = "Username or password is incorrect" });
            else
            {
                if (user.is_admin != "True")
                    return Ok(new
                        {
                            id = user.id,
                            email = user.name,
                            isAdmin = user.is_admin
                        }
                    );
                else
                    return Ok(new
                        {
                            id = user.id,
                            email = user.name,
                            isAdmin = user.is_admin
                        }
                    );
            }
        }
        
    }
}
