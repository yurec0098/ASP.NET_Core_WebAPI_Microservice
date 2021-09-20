using MetricsAgent.DB;
using MetricsAgent.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : BaseController<NetworkMetric>
	{
		public NetworkMetricsController(ILogger<NetworkMetricsController> logger, IDbRepository<NetworkMetric> repository)
			: base(logger, repository)
		{
			logger?.LogDebug(1, "NLog встроен в NetworkMetricsController");
		}
	}
}
