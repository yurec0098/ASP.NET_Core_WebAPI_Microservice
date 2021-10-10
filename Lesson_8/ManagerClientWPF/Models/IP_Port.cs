namespace ManagerClientWPF.Models
{
	public class IP_Port
	{
		public override string ToString() => $"http://{IP}:{Port}/";

		public string IP { get; set; } = "localhost";
		public int Port { get; set; } = 5006;

		public IP_Port() { }
		public IP_Port(string ip, int port)
		{
			IP = ip;
			Port = port;
		}
	}
}
