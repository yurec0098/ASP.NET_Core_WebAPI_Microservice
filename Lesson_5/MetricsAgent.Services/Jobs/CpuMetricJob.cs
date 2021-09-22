using MetricsAgent.DB;
using MetricsAgent.DB.Entity;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
	public class MetricsJob : IJob
	{
		private readonly IDbRepository<CpuMetric> _cpuRepo;
		private readonly IDbRepository<RamMetric> _ramRepo;
		private readonly IDbRepository<HddMetric> _hddRepo;
		private readonly IDbRepository<DotNetMetric> _dotnetRepo;
		private readonly IDbRepository<NetworkMetric> _networkRepo;

		// счетчик для метрики CPU
		private PerformanceCounter _cpuCounter;
		private PerformanceCounter _ramCounter;
		private PerformanceCounter _hddCounter;
		private PerformanceCounter _networkCounter;
		private PerformanceCounter _dotnetCounter;

		public MetricsJob(
			IDbRepository<CpuMetric> cpuRepo,
			IDbRepository<RamMetric> ramRepo,
			IDbRepository<HddMetric> hddRepo,
			IDbRepository<DotNetMetric> dotnetRepo,
			IDbRepository<NetworkMetric> networkRepo)
		{
			_cpuRepo = cpuRepo;
			_ramRepo = ramRepo;
			_hddRepo = hddRepo;
			_dotnetRepo = dotnetRepo;
			_networkRepo = networkRepo;

			_cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
			_ramCounter = new PerformanceCounter("Memory", "Available MBytes");
			_hddCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
			//_networkCounter = new PerformanceCounter("Network Adapter", "Bytes Total/sec", "intel[r] centrino[r] advanced-n 6200 agn");
			_networkCounter = new PerformanceCounter("Network Adapter", "Bytes Total/sec", "_Total");
			_dotnetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps", "_Global");
		}

		public Task Execute(IJobExecutionContext context)
		{
			var time = DateTime.UtcNow.Ticks;

			// получаем значения
			var cpuValue = Convert.ToInt32(_cpuCounter.NextValue());
			var ramValue = Convert.ToInt32(_ramCounter.NextValue());
			var hddValue = Convert.ToInt32(_hddCounter.NextValue());
			var networkValue = Convert.ToInt32(_networkCounter.NextValue());
			var dotnetValue = Convert.ToInt32(_dotnetCounter.NextValue());

			// теперь можно записать полученные данные при помощи репозиториев
			_cpuRepo.AddAsync(new CpuMetric { Time = time, Value = cpuValue });
			_ramRepo.AddAsync(new RamMetric { Time = time, Value = ramValue });
			_hddRepo.AddAsync(new HddMetric { Time = time, Value = hddValue });
			_dotnetRepo.AddAsync(new DotNetMetric { Time = time, Value = networkValue });
			_networkRepo.AddAsync(new NetworkMetric { Time = time, Value = dotnetValue });

			//Debug.WriteLine($"CpuMetricJob, {DateTime.Now: ddd HH:mm:ss} cpuPercents: {cpuUsageInPercents}% RAM: {ramCounter}MB");
			return Task.CompletedTask;
		}
	}
}
