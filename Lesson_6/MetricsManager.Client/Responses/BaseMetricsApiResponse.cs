using MetricsManager.DB.Entity;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	public abstract class BaseMetricsApiResponse<TEntity> where TEntity : BaseEntity
	{
		public List<TEntity> MetricsList { get; set; }
	}
}
