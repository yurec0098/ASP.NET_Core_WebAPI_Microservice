using MetricsManager.DB;
using MetricsManager.DB.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AgentsController : ControllerBase
	{
		private readonly ILogger<AgentsController> _logger;
		private readonly IDbRepository<AgentInfo> _repository;

		public AgentsController(ILogger<AgentsController> logger, IDbRepository<AgentInfo> repository)
		{
			_logger = logger;
			_repository = repository;
			_logger?.LogDebug(1, "NLog встроен в AgentsController");
		}


		[HttpPost("register")]
		public async Task<IActionResult> RegisterAgent([FromBody] AgentInfo agentInfo)
		{
			//var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
			//var remotePort = Request.HttpContext.Connection.RemotePort;

			if(_repository.GetAll().FirstOrDefault(x=> x.AgentAddress == agentInfo.AgentAddress) is AgentInfo agent)
			{
				agent.IsEnabled = true;
				await _repository.SaveChangesAsync();
			}
			else
			{
				await _repository.AddAsync(agentInfo);
			}

			return Ok();
		}

		//[HttpPut("enable/{agentId}")]
		//public IActionResult EnableAgentById([FromRoute] int agentId)
		//{
		//    return Ok();
		//}

		//[HttpPut("disable/{agentId}")]
		//public IActionResult DisableAgentById([FromRoute] int agentId)
		//{
		//    return Ok();
		//}

		[HttpGet("agents")]
		public async Task<List<AgentInfo>> GetAgents()
		{
			return await _repository.GetAll().ToListAsync();
		}
	}
}
