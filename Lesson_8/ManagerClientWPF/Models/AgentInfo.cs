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
}
