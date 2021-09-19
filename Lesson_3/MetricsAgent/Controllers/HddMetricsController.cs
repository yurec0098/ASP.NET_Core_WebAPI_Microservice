using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;
		private IHddMetricsRepository _repository;

		public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository)
		{
			_repository = repository;

			_logger = logger;
            _logger?.LogDebug(1, "NLog встроен в HddMetricsController");
        }


        [HttpGet("left/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics([FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
		{
			var metrics = _repository.GetByTimePeriod(fromTime, toTime);

			return Ok(metrics);
		}

		[HttpPost("create")]
		public IActionResult Create([FromBody] HddMetricCreateRequest request)
		{
			var id = _repository.Create(new HddMetric
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
