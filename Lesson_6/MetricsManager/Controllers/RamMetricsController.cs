using MetricsManager.DB;
using MetricsManager.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace MetricsManager.Controllers
{
	[Route("api/metrics/ram")]
	[ApiController]
	public class RamMetricsController : BaseMetricsController<RamMetric>
	{
		public RamMetricsController(ILogger<RamMetricsController> logger, IHttpClientFactory clientFactory, IDbRepository<AgentInfo> repository)
			: base(logger, clientFactory, repository)
		{
			_route = "ram";
		}
	}
}
