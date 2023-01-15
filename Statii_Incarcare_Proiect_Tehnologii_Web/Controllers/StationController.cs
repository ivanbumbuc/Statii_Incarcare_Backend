using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Statii_Incarcare_Proiect_Tehnologii_Web.Context;
using Statii_Incarcare_Proiect_Tehnologii_Web.Dto;
using Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

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

        [AllowAnonymous]
        [HttpPost("addStation")]
        public async Task<IActionResult> AddStation(StationDto station)
        {
            var newStation = new Station
            {
                name = station.name,
                city = station.city,
                address = station.address,
                coordX = station.coordX,
                coordY = station.coordY
            };
            _incarcareContext.Stations.Add(newStation);
            await _incarcareContext.SaveChangesAsync();
            var stationToAdmin = new StationToAdmin
            {
                station_id = newStation.id,
                admin_id = station.idUser
            };
            _incarcareContext.StationToAdmins.Add(stationToAdmin);
            await _incarcareContext.SaveChangesAsync();
            return Ok(newStation.id);
        }
    }
}