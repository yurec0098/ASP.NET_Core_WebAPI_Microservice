using System;

namespace ManagerClientWPF.Models
{
	public class AgentInfo
	{
		public long Id { get; set; }
		public Uri AgentAddress { get; set; }
		public string AgentName { get; set; }
		public bool IsEnabled { get; set; }
	}
	public class AgentMetricEntity
	{
		public long Id { get; set; }
		public double Value { get; set; }
		//public DateTime Time { get; set; }
		public long Time { get; set; }
		//public long? AgentId { get; set; }
	}
}
