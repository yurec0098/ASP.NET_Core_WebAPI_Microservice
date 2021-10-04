using MetricsManager.DB;
using MetricsManager.DB.Entity;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManager.Jobs
{
	public class AgentsJob : IJob, IDisposable
	{
		private IServiceScope scope;
		private readonly IDbRepository<AgentInfo> _repo;

		public AgentsJob(IServiceProvider scopeFactory)
		{
			scope = scopeFactory.CreateScope();
			_repo = scope.ServiceProvider.GetRequiredService<IDbRepository<AgentInfo>>();
		}

		public Task Execute(IJobExecutionContext context)
		{
			Ping pingSender = new Ping();
			PingOptions options = new PingOptions(128, true);

			// Create a buffer of 32 bytes of data to be transmitted.
			byte[] buffer = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
			int timeout = 120;
		
			foreach (var agent in _repo.GetAll())
			{
				PingReply reply = pingSender.Send(agent.AgentAddress.AbsoluteUri, timeout, buffer, options);
				if (reply.Status == IPStatus.Success)
					agent.IsEnabled = true;
				else
					agent.IsEnabled = true;

				_repo.SaveChangesAsync();
			}

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			scope?.Dispose();
		}
	}
}
