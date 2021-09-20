using MetricsAgent.DB;
using MetricsAgent.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : BaseController<HddMetric>
	{
		public HddMetricsController(ILogger<HddMetricsController> logger, IDbRepository<HddMetric> repository)
			: base(logger, repository)
		{
			logger?.LogDebug(1, "NLog встроен в HddMetricsController");
		}
	}
}
