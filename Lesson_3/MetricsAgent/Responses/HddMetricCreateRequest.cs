using System;

namespace MetricsAgent.Responses
{
	public class HddMetricCreateRequest : BaseMetricCreateRequest<int>
    {
        public DateTime Time { get; set; }
        public int Value { get; set; }
    }
}
