using MetricsManager.DB;
using MetricsManager.DB.Entity;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace MetricsManager.Controllers
{
	[ApiController]
	public abstract class BaseMetricsController<TEntity> : ControllerBase where TEntity : BaseMetricEntity
	{
		private readonly ILogger<ControllerBase> _logger;
		private IHttpClientFactory _clientFactory;
		private IDbRepository<AgentInfo> _repository;
		protected string _route = string.Empty;


		protected BaseMetricsController(ILogger<ControllerBase> logger, IHttpClientFactory clientFactory, IDbRepository<AgentInfo> repository)
		{
			_logger = logger;
			_clientFactory = clientFactory;
			_repository = repository;

		    _logger?.LogTrace(1, $"NLog встроен в {GetType()}");
		    Console.WriteLine($"NLog встроен в {GetType()}");
		}

		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
		public async Task<IActionResult> GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
		{
			if (_repository.GetAll().FirstOrDefault(x => x.Id == agentId) is AgentInfo agent)
			{
				var request = new HttpRequestMessage(HttpMethod.Get, $"{agent.AgentAddress}api/metrics/{_route}/from/{fromTime:s}/to/{toTime:s}");
				request.Headers.Add("Accept", "application/vnd.github.v3+json");

				var client = _clientFactory.CreateClient();
				var response = await client.SendAsync(request);

				if (response.IsSuccessStatusCode)
				{
					await using var responseStream = await response.Content.ReadAsStreamAsync();
					return Ok(await JsonSerializer.DeserializeAsync<TEntity[]>(responseStream, new JsonSerializerOptions(JsonSerializerDefaults.Web)));
				}
				else
				{
					// ошибка при получении ответа
					return StatusCode((int)response.StatusCode);
				}
			}
			return NotFound("Agent not found!");
		}

		//[HttpGet("cluster/from/{fromTime}/to/{toTime}")]
		//public IActionResult GetMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		//{
		//	_logger?.LogTrace($"GetMetricsFromAllCluster({fromTime}, {toTime})");
		//	return Ok();
		//}
	}
}
