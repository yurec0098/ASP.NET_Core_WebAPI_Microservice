using MetricsManager.DB;
using MetricsManager.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/cpu")]
	[ApiController]
	public class CpuMetricsController : BaseController<CpuMetric>
	{
		public CpuMetricsController(ILogger<CpuMetricsController> logger, IDbRepository<CpuMetric> repository)
			: base(logger, repository)
		{
			logger?.LogDebug(1, "NLog встроен в CpuMetricsController");
		}
	}
}