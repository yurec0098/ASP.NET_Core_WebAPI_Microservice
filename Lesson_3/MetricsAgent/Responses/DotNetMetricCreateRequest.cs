using System;

namespace MetricsAgent.Responses
{
	public class DotNetMetricCreateRequest : BaseMetricCreateRequest<int>
    {
        public DateTime Time { get; set; }
        public int Value { get; set; }
    }
}
