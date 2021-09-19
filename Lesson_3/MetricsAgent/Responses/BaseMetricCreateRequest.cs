using System;

namespace MetricsAgent.Responses
{
	public abstract class BaseMetricCreateRequest<T>
    {
        public DateTime Time { get; set; }
        public T Value { get; set; }
    }
}
