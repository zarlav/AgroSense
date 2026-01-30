using AgroSense.DTOs.Alarm;
using AgroSense.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Cassandra;
using System.Collections.Generic;

namespace AgroSense.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlarmController : ControllerBase
    {
        private readonly AlarmService _alarmService;

        public AlarmController(AlarmService alarmService)
        {
            _alarmService = alarmService;
        }

      [HttpGet("po_jedinici")]
public IActionResult VratiAlarmePoJedinici(
    [FromQuery] Guid idJedinice,
    [FromQuery] DateTime dan,
    [FromQuery] DateTime vremeOd,
    [FromQuery] DateTime vremeDo)
{
    try
    {
        var cassandraDan = new LocalDate(dan.Year, dan.Month, dan.Day);

        var alarmi = _alarmService.VratiAlarmePoJedinici(
            idJedinice,
            cassandraDan,
            vremeOd,
            vremeDo);

        if (alarmi.Count == 0)
            return NotFound("Nema alarma za dati period.");

        return Ok(alarmi);
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex.Message);
    }
}

    }    
}
