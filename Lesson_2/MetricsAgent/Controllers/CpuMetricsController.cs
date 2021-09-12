using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/cpu")]
    [ApiController]
	public class CpuMetricsController : ControllerBase
    {
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
