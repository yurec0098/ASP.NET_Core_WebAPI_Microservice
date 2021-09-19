using System;

namespace MetricsAgent.DAL
{
	public abstract class BaseMetric<T>
	{
		public int Id { get; set; }

		public T Value { get; set; }

		public DateTime Time { get; set; }
	}
}
