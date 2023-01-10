using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Statii_Incarcare_Proiect_Tehnologii_Web.Context;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly StatiiIncarcareContext _incarcareContext;
    
    public UserController(StatiiIncarcareContext incarcareContext)
    {
        _incarcareContext = incarcareContext;
    }
    
    [AllowAnonymous]
    [HttpGet("getUserProfile")]
    public async Task<IActionResult> GetUserProfile(String userId)
    {
        var user = await _incarcareContext.Users.FirstOrDefaultAsync(user => user.id.ToString() == userId);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(new
        {
            user.name,
            admin = user.is_admin,
            charging = user.is_charging
        });
    }
}