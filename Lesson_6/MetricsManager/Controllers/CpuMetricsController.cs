using MetricsManager.DB;
using MetricsManager.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace MetricsManager.Controllers
{
	[Route("api/metrics/cpu")]
	[ApiController]
	public class CpuMetricsController : BaseMetricsController<CpuMetric>
	{
		public CpuMetricsController(ILogger<CpuMetricsController> logger, IHttpClientFactory clientFactory, IDbRepository<AgentInfo> repository)
			: base(logger, clientFactory, repository)
		{
			_route = "cpu";
		}
	}
}
