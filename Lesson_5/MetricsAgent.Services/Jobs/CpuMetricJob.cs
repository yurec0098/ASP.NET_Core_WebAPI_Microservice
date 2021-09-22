using MetricsAgent.DB;
using MetricsAgent.DB.Entity;
using Quartz;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
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
		private PerformanceCounter _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
		private PerformanceCounter _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
		private PerformanceCounter _hddCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
		private PerformanceCounter _dotnetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps", "_Global_");
		private PerformanceCounter _networkCounter = new PerformanceCounter("Network Adapter", "Bytes Total/sec");

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

			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
				if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
					_networkCounter.InstanceName = ni.Description;
		}

		public Task Execute(IJobExecutionContext context)
		{
			var time = DateTime.UtcNow.Ticks;

			// получаем значения
			var cpuValue = Convert.ToInt32(_cpuCounter.NextValue());
			var ramValue = Convert.ToInt32(_ramCounter.NextValue());
			var hddValue = Convert.ToInt32(_hddCounter.NextValue());
			var dotnetValue = Convert.ToInt32(_dotnetCounter.NextValue());
			var networkValue = Convert.ToInt32(_networkCounter.NextValue());

			// теперь можно записать полученные данные при помощи репозиториев
			_cpuRepo.AddAsync(new CpuMetric { Time = time, Value = cpuValue });
			_ramRepo.AddAsync(new RamMetric { Time = time, Value = ramValue });
			_hddRepo.AddAsync(new HddMetric { Time = time, Value = hddValue });
			_dotnetRepo.AddAsync(new DotNetMetric { Time = time, Value = dotnetValue });
			_networkRepo.AddAsync(new NetworkMetric { Time = time, Value = networkValue });

			//Debug.WriteLine($"CpuMetricJob, {DateTime.Now: ddd HH:mm:ss} cpuPercents: {cpuValue}% RAM: {ramValue}MB Network: {networkValue}");
			return Task.CompletedTask;
		}
	}
}
