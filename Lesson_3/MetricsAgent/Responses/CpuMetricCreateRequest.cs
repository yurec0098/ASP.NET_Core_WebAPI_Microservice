using System;

namespace MetricsAgent.Responses
{
	public class CpuMetricCreateRequest : BaseMetricCreateRequest<int>
    {
        public DateTime Time { get; set; }
        public int Value { get; set; }
    }
}
