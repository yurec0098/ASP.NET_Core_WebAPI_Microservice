using System;
using System.Collections.Generic;

namespace MetricsManager.DB.Entity
{
	public class AgentInfo : BaseEntity
	{
		public Uri AgentAddress { get; set; }
		public string AgentName { get; set; }
		public bool IsEnabled { get; set; }
	}
}
