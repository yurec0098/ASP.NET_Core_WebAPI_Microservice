using System;

namespace MetricsManager.DB.Entity
{
	public abstract class BaseEntity
	{
		public long Id { get; set; }
		//public DateTime Time { get; set; }
		public long Time { get; set; }
	}
}
