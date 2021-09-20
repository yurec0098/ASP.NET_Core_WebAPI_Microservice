using MetricsAgent.DB;
using MetricsAgent.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : BaseController<DotNetMetric>
	{
		public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDbRepository<DotNetMetric> repository)
			: base(logger, repository)
		{
			logger?.LogDebug(1, "NLog встроен в DotNetMetricsController");
		}
	}
}
