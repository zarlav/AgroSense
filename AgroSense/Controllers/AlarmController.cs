using AgroSense.DTOs.Alarm;
using AgroSense.Services;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet("danas/{lokacijaId}")]
        public ActionResult<List<AlarmResponseDTO>> VratiDanasnjeAlarme(Guid lokacijaId)
        {
            var alarmi = _alarmService.GenerisiAlarmeZaDanas(lokacijaId);
            return Ok(alarmi);
        }
    }
}
