using MetricsAgent.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetricsAgent.Tests
{
	public class CpuMetricsRepoTest : ICpuMetricsRepository
	{
		private List<CpuMetric> _repo =new List<CpuMetric>();
		private Random _random = new Random();

		public CpuMetricsRepoTest()
		{
			for (int i = 1; i <= 100; i++)
				_repo.Add(new CpuMetric() { Id = i, Value = _random.Next(100), Time = DateTime.Now.AddSeconds(i) });
		}

		public int Create(CpuMetric item)
		{
			var newId = _repo.Max(x => x.Id);
			_repo.Add(new CpuMetric() { Id = ++newId, Value = item.Value, Time = item.Time });
			return newId;
		}

		public void Delete(int id)
		{
			if(_repo.FirstOrDefault(x => x.Id == id) is CpuMetric metric)
				_repo.Remove(metric);
		}

		public IList<CpuMetric> GetAll()
		{
			return _repo;
		}

		public CpuMetric GetById(int id)
		{
			if (_repo.FirstOrDefault(x => x.Id == id) is CpuMetric metric)
				return metric;
			return null;
		}

		public IList<CpuMetric> GetByTimePeriod(DateTime from, DateTime to)
		{
			return _repo.Where(x => x.Time <= from && x.Time >= to).ToList();
		}

		public void Update(CpuMetric item)
		{
			if (_repo.FirstOrDefault(x => x.Id == item.Id) is CpuMetric metric)
			{
				metric.Value = item.Value;
				metric.Time = item.Time;
			}
		}
	}
}
