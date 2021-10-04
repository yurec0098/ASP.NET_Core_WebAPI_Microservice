using MetricsManager.DB;
using MetricsManager.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace MetricsManager.Controllers
{
	[Route("api/metrics/hdd")]
	[ApiController]
	public class HddMetricsController : BaseMetricsController<HddMetric>
	{
		public HddMetricsController(ILogger<HddMetricsController> logger, IHttpClientFactory clientFactory, IDbRepository<AgentInfo> repository)
			: base(logger, clientFactory, repository)
		{
			_route = "hdd";
		}
	}
}
