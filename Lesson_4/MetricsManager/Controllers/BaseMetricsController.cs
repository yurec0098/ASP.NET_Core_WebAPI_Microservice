using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
	[ApiController]
	public abstract class BaseMetricsController<TEntity, TDto> : ControllerBase
    {
        private readonly ILogger<TEntity> _logger;

        protected BaseMetricsController(ILogger<TEntity> logger)
        {
            _logger = logger;
            _logger?.LogDebug(1, $"NLog встроен в {GetType()}");
        }


        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger?.LogTrace($"GetMetricsFromAgent({agentId}, {fromTime}, {toTime})");
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger?.LogTrace($"GetMetricsFromAllCluster({fromTime}, {toTime})");
            return Ok();
        }
    }
}
