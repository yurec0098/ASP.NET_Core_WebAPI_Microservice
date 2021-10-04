using System;

namespace MetricsManager.Requests
{
	public class BaseMetricsApiRequest
	{
		public Uri ClientBaseAddress { get; set; }
		public DateTime FromTime { get; set; }
		public DateTime ToTime { get; set; }
	}
}
