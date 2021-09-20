using MetricsManager.DB;
using MetricsManager.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : BaseController<RamMetric>
	{
		public RamMetricsController(ILogger<RamMetricsController> logger, IDbRepository<RamMetric> repository)
			: base(logger, repository)
		{
			logger?.LogDebug(1, "NLog встроен в RamMetricsController");
		}
	}
}
