using MetricsManager.DB;
using MetricsManager.DB.Entity;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
			foreach (var agent in _repo.GetAll())
			{
				if (PingHostPort(agent.AgentAddress))
					agent.IsEnabled = true;
				else
					agent.IsEnabled = false;
			}

			_repo.SaveChangesAsync();
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			scope?.Dispose();
		}

		private bool PingHostPort(Uri uri, bool throwExceptionOnError = false)
		{
			bool pingable = false;

			var ipAddress = Dns.GetHostAddresses(uri.Host);
			if (ipAddress.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork) is IPAddress ipAddr)
			{
				var ip = new IPEndPoint(ipAddr, uri.Port);
				using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
				{
					try
					{
						socket.Connect(ip);
						pingable = socket.Connected;
					}
					catch (PingException e)
					{
						if (throwExceptionOnError) throw e;
						pingable = false;
					}
				}
			}
			return pingable;
		}
	}
}
