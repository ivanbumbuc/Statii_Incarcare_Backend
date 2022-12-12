using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Statii_Incarcare_Proiect_Tehnologii_Web.Context;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StationController : ControllerBase
    {
        private readonly StatiiIncarcareContext _incarcareContext;

        public StationController(StatiiIncarcareContext context)
        {
            _incarcareContext = context;
        }
        
        [AllowAnonymous]
        [HttpGet("getAllStations")]
        public async Task<IActionResult> GetAllStationsFromCity(String city)
        {
            var stations = _incarcareContext.Stations
                .Include(x => x.Plugs)
                .Where(s => s.city == city)
                .Select(x => new
                {
                    x.id,
                    x.name,
                    x.city,
                    x.address,
                    plugs = string.Join("\n", x.Plugs.Select(y => string.Join(" ",
                        "Type:", y.typeNavigation.name, "Power:",
                        y.typeNavigation.power, "Price:", y.price, "??:", y.is_charging))),
                    latitude = x.coordX,
                    longitude = x.coordY
                }).ToList();
            return Ok(stations);
        }

        [AllowAnonymous]
        [HttpGet("getAvailable")]
        public async Task<IActionResult> GetAllAvailableStationsFromCity(String city)
        {
            var stations = _incarcareContext.Stations
                .Include(x => x.Plugs)
                .Where(s => s.city == city)
                .Where(s => s.Plugs.Any(p => p.is_charging == "false"))
                .Select(x => new
                {
                    x.id,
                    x.name,
                    x.city,
                    x.address,
                    plugs = string.Join("\n", x.Plugs.Where(p => p.is_charging == "false").Select(y => string.Join(" ",
                        y.typeNavigation.name + ";", "Price:", y.price + ';',
                        y.is_charging == "false" ? "Availabe" : "In use"))),
                    latitude = x.coordX,
                    longitude = x.coordY
                }).ToList();
            return Ok(stations);
        }
    }
}