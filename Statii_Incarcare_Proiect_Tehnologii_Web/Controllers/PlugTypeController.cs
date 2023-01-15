using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Statii_Incarcare_Proiect_Tehnologii_Web.Context;
using Statii_Incarcare_Proiect_Tehnologii_Web.Dto;
using Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Controllers;

[ApiController]
[Route("[controller]")]
public class PlugTypeController : ControllerBase
{
    private readonly StatiiIncarcareContext _incarcareContext;

    public PlugTypeController(StatiiIncarcareContext context)
    {
        _incarcareContext = context;
    }

    [AllowAnonymous]
    [HttpGet("getAllPlugsType")]
    public async Task<IActionResult> GetAllPlugsType()
    {
        var listPlugsType =  await _incarcareContext.PlugTypes.Select(s => new
        {
            id = s.id,
            name = s.name,
            power = s.power
        }).ToListAsync();
        return Ok(listPlugsType);
    }
        
    [AllowAnonymous]
    [HttpPost("addPlugType")]
    public async Task<IActionResult> AddPlugType(PlugTypeDto plugTypeDto)
    {
        var plugType = new PlugType
        {
            name = plugTypeDto.name,
            power = plugTypeDto.power
        };
        await _incarcareContext.PlugTypes.AddAsync(plugType);
        await _incarcareContext.SaveChangesAsync();
        return Ok();
    }
}