using MetricsManager.DB;
using MetricsManager.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace MetricsManager.Controllers
{
	[Route("api/metrics/network")]
	[ApiController]
	public class NetworkMetricsController : BaseMetricsController<NetworkMetric>
	{
		public NetworkMetricsController(ILogger<NetworkMetricsController> logger, IHttpClientFactory clientFactory, IDbRepository<AgentInfo> repository)
			: base(logger, clientFactory, repository)
		{
			_route = "network";
		}
	}
}
