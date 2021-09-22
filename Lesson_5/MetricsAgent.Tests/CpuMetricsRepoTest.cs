using MetricsAgent.DB;
using MetricsAgent.DB.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Tests
{
	public class CpuMetricsRepoTest : IDbRepository<CpuMetric>
	{
		private List<CpuMetric> _repo =new List<CpuMetric>();
		private Random _random = new Random();

		public CpuMetricsRepoTest()
		{
			for (int i = 1; i <= 100; i++)
				_repo.Add(new CpuMetric() { Id = i, Value = _random.Next(100), Time = DateTime.Now.AddSeconds(i).Ticks });
		}


		public Task AddAsync(CpuMetric entity)
		{
			var newId = _repo.Max(x => x.Id);
			return new Task(() => _repo.Add(new CpuMetric() { Id = ++newId, Value = entity.Value, Time = entity.Time }));
		}

		public Task DeleteAsync(CpuMetric entity)
		{
			return new Task(() => {
				if (_repo.FirstOrDefault(x => x.Id == entity.Id) is CpuMetric metric)
					_repo.Remove(metric);
			});
		}

		public Task UpdateAsync(CpuMetric entity)
		{
			return new Task(() =>
			{
				if (_repo.FirstOrDefault(x => x.Id == entity.Id) is CpuMetric metric)
				{
					metric.Value = entity.Value;
					metric.Time = entity.Time;
				}
			});
		}

		IQueryable<CpuMetric> IDbRepository<CpuMetric>.GetAll()
		{
			return _repo.AsQueryable<CpuMetric>();
		}
	}
}
