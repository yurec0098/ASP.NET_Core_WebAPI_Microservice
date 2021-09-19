using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/cpu")]
	[ApiController]
	public class CpuMetricsController : ControllerBase
	{
		private readonly ILogger<CpuMetricsController> _logger;
		private ICpuMetricsRepository _repository;

		public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository repository)
		{
			_repository = repository;

			_logger = logger;
			_logger?.LogDebug(1, "NLog встроен в CpuMetricsController");
		}

		[HttpPost("create")]
		public IActionResult Create([FromBody] CpuMetricCreateRequest request)
		{
			var id = _repository.Create(new CpuMetric
			{
				Time = request.Time,
				Value = request.Value
			});
						
			return Ok(id);
			//return Ok(GetById(id));
		}

		[HttpGet("all")]
		public IActionResult GetAll()
		{
			var metrics = _repository.GetAll();
			//  не понимаю зачем подобное делать, если поля идентичные
			//var response = new AllCpuMetricsResponse();

			//foreach (var metric in metrics)
			//{
			//    response.Metrics.Add(new CpuMetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
			//}

			//return Ok(response);
			return Ok(metrics);
		}

		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics([FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
		{
			var metrics = _repository.GetByTimePeriod(fromTime, toTime);

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
