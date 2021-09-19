using System;

namespace MetricsAgent.Responses
{
	public class RamMetricCreateRequest : BaseMetricCreateRequest<int>
    {
        public DateTime Time { get; set; }
        public int Value { get; set; }
    }
}
