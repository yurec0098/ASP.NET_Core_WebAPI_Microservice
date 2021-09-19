using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
	[Route("api/metrics/cpu")]
	[ApiController]
	public class CpuMetricsController : ControllerBase
	{
		private readonly ILogger<CpuMetricsController> _logger;
		//private readonly IMetricsService<CpuMetric> _service;

		public CpuMetricsController(ILogger<CpuMetricsController> logger/*, IMetricsService<CpuMetric> service*/)
		{
			_logger = logger;
			_logger?.LogDebug(1, "NLog встроен в CpuMetricsController");

			//_service = service;
		}


		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger?.LogTrace($"GetMetricsFromAgent({agentId}, {fromTime}, {toTime})");
			//_service.GetMetricsFromAgent(agentId, fromTime, toTime);
			return Ok();
		}

		[HttpGet("cluster/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger?.LogTrace($"GetMetricsFromAllCluster({fromTime}, {toTime})");
			//_service.GetMetricsFromAllCluster(fromTime, toTime);
			return Ok();
		}
	}
}
