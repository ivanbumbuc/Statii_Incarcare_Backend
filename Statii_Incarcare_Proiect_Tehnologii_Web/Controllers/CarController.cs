using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statii_Incarcare_Proiect_Tehnologii_Web.Context;
using Statii_Incarcare_Proiect_Tehnologii_Web.Dto;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CarController : ControllerBase
{
    private readonly StatiiIncarcareContext _incarcareContext;

    public CarController(StatiiIncarcareContext incarcareContext)
    {
        _incarcareContext = incarcareContext;
    }

    [AllowAnonymous]
    [HttpGet("getCars")]
    public async Task<IActionResult> GetCars(string userId)
    {
        var cars = _incarcareContext.Cars
            .Where(c => c.user_id.ToString() == userId)
            .Select(c => new
            {
                car_id = c.id, 
                car_plate = c.car_plate
            })
            .ToList();
        return Ok(cars);
    }

    [AllowAnonymous]
    [HttpPut("updateCars")]
    public async Task<IActionResult> UpdateCars(CarDto carDto)
    {
        var car = _incarcareContext.Cars.FirstOrDefault(c => c.id == carDto.carId);
        if (car == null) return BadRequest("Car not found");

        car.car_plate = carDto.car_plate;
        _incarcareContext.Cars.Update(car);
        _incarcareContext.SaveChanges();
        return Ok();
    }

    [AllowAnonymous]
    [HttpDelete("deleteCar")]
    public async Task<IActionResult> DeleteCar(string carId)
    {
        var car = _incarcareContext.Cars.FirstOrDefault(c => c.id.ToString() == carId);
        if (car == null) return BadRequest("Car not found");

        _incarcareContext.Cars.Remove(car);
        _incarcareContext.SaveChanges();
        return Ok();
    }
}