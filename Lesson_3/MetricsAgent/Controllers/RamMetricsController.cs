using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private IRamMetricsRepository _repository;

        public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository)
        {
            _repository = repository;

            _logger = logger;
            _logger?.LogDebug(1, "NLog встроен в RamMetricsController");
        }


        [HttpGet("available/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics([FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
		{
			var metrics = _repository.GetByTimePeriod(fromTime, toTime);

			return Ok(metrics);
		}

		[HttpPost("create")]
		public IActionResult Create([FromBody] RamMetricCreateRequest request)
		{
			var id = _repository.Create(new RamMetric
			{
				Time = request.Time,
				Value = request.Value
			});

			return Ok(id);
		}

		[HttpGet("all")]
		public IActionResult GetAll()
		{
			var metrics = _repository.GetAll();

			return Ok(metrics);
		}

		[HttpGet("id/{id}")]
		public IActionResult GetById([FromRoute] int id)
		{
			var response = _repository.GetById(id);

			return response != null ? Ok(response) : NotFound();
		}
	}
}
