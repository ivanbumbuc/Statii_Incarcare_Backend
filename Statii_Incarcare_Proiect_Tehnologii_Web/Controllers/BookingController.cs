using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statii_Incarcare_Proiect_Tehnologii_Web.Context;
using Statii_Incarcare_Proiect_Tehnologii_Web.Dto;
using Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly StatiiIncarcareContext _incarcareContext;

    public BookingController(StatiiIncarcareContext incarcareContext)
    {
        _incarcareContext = incarcareContext;
    }

    [AllowAnonymous]
    [HttpGet("getBookings")]
    public async Task<IActionResult> GetBookings(string userId)
    {
        var bookings = _incarcareContext.Bookings
            .Where(b => b.user_id.ToString() == userId)
            .Select(b => new
            {
                start_time = b.start_time.ToString("dd.MM.yyyy hh:mm"),
                end_time = b.end_time.ToString("dd.MM.yyyy hh:mm"),
                plug_type = _incarcareContext.Plugs.First(p => p.id == b.plug_id).typeNavigation.name
            })
            .ToList();
        return Ok(bookings);
    }

    [AllowAnonymous]
    [HttpPost("addBooking")]
    public async Task<IActionResult> AddBooking(BookingAdd bookingAdd)
    {
        if (_incarcareContext.Users.Where(user => user.id == bookingAdd.userId && user.is_charging == "true")
                .Count() == 1)
            return BadRequest("User has already booked a charging station plug.");

        var booking = new Booking
        {
            plug_id = bookingAdd.plugId,
            user_id = bookingAdd.userId,
            start_time = DateTime.ParseExact(bookingAdd.startingHour, "HH:mm", CultureInfo.InvariantCulture),
            end_time = DateTime.ParseExact(bookingAdd.endingHour, "HH:mm", CultureInfo.InvariantCulture)
        };
        await _incarcareContext.AddAsync(booking);
        await _incarcareContext.SaveChangesAsync();

        var result = _incarcareContext.Users.FirstOrDefault(user => user.id == bookingAdd.userId);
        if (result != null)
        {
            result.is_charging = "true";
            _incarcareContext.Update(result);
            await _incarcareContext.SaveChangesAsync();
        }

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("getAvailableStartingHours")]
    public async Task<IActionResult> GetAvailableStartingHours(string plugId)
    {
        if (_incarcareContext.Plugs.Where(p => p.id.ToString() == plugId).Count() == 0)
            return NotFound("Plug does not exist.");

        var availableHours = new List<string>();
        var bookings = _incarcareContext.Bookings.Where(b => b.plug_id.ToString() == plugId).ToList();
        var hours = Enumerable.Repeat(true, 24).ToArray();
        var nextAvailableHour = DateTime.Now.TimeOfDay.Hours + 1;
        for (var i = 0; i < nextAvailableHour; i++)
            hours[i] = false;

        foreach (var booking in bookings)
            if (booking.start_time.Date == DateTime.Now.Date)
            {
                var startHour = booking.start_time.TimeOfDay.Hours;
                var endHour = booking.end_time.TimeOfDay.Hours;

                for (var i = startHour; i <= endHour; i++) hours[i] = false;
            }

        for (var i = 0; i < 24; i++)
            if (hours[i])
            {
                if (i < 10)
                    availableHours.Add("0" + i + ":00");
                else
                    availableHours.Add(i + ":00");
            }

        return Ok(availableHours);
    }

    [AllowAnonymous]
    [HttpGet("getAvailableEndingHours")]
    public async Task<IActionResult> GetAvailableEndingHours(string startingHour, string plugId)
    {
        if (_incarcareContext.Plugs.Where(p => p.id.ToString() == plugId).Count() == 0)
            return NotFound("Plug does not exist.");

        var availableHours = new List<string>();
        var bookings = _incarcareContext.Bookings.Where(b => b.plug_id.ToString() == plugId).ToList();
        var hours = Enumerable.Repeat(true, 24).ToArray();
        var nextAvailableHour = int.Parse(startingHour.Split(':')[0]);

        for (var i = 0; i < nextAvailableHour; i++) hours[i] = false;

        foreach (var booking in bookings)
            if (booking.start_time.Date == DateTime.Now.Date)
            {
                var startHour = booking.start_time.TimeOfDay.Hours;
                var endHour = booking.end_time.TimeOfDay.Hours;

                for (var i = startHour; i < endHour; i++) hours[i] = false;
            }

        for (var i = nextAvailableHour; i < 24; i++)
        {
            if (hours[i])
            {
                if (i < 10)
                    availableHours.Add("0" + (i) + ":59");
                else
                    availableHours.Add(i + ":59");
            }
            else
            {
                break;
            }
        }

        return Ok(availableHours);
    }
}