using MetricsManager.DB;
using MetricsManager.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace MetricsManager.Controllers
{
	[Route("api/metrics/dotnet")]
	[ApiController]
	public class DotNetMetricsController : BaseMetricsController<DotNetMetric>
	{
		public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IHttpClientFactory clientFactory, IDbRepository<AgentInfo> repository)
			: base(logger, clientFactory, repository)
		{
			_route = "dotnet";
		}
	}
}
