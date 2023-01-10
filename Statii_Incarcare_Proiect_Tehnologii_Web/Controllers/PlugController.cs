using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statii_Incarcare_Proiect_Tehnologii_Web.Context;
using Statii_Incarcare_Proiect_Tehnologii_Web.Dto;
using Statii_Incarcare_Proiect_Tehnologii_Web.Entities;


namespace Statii_Incarcare_Proiect_Tehnologii_Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlugController : ControllerBase
    {
        private readonly StatiiIncarcareContext _incarcareContext;

        public PlugController(StatiiIncarcareContext context)
        {
            _incarcareContext = context;
        }

        [AllowAnonymous]
        [HttpGet("getAvailablePlugs")]
        public async Task<IActionResult> GetAvailablePlugs(String id)
        {
            var plugs = _incarcareContext.Plugs
                .Where(p => p.station_id.ToString() == id && p.is_charging == "false")
                .Select(p => new
                {
                    id = p.id,
                    name = p.typeNavigation.name
                })
                .ToList();
            return Ok(plugs);
        }
        
        [AllowAnonymous]
        [HttpPost("addPlug")]
        public async Task<IActionResult> AddPlug(PlugDto plug)
        {
            var plugToAdd = new Plug
            {
                station_id = new Guid(plug.stationId),
                type = new Guid("8d34f5dd-7d63-4408-8411-b566ef0adf3b"),
                price = plug.price,
                is_charging = "false"
            };
            _incarcareContext.Plugs.Add(plugToAdd);
            await _incarcareContext.SaveChangesAsync();
            return Ok();
        }
    }
}