using LiveCharts;
using LiveCharts.Wpf;
using ManagerClientWPF.Controls;
using ManagerClientWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManagerClientWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public IP_Port Host { get; set; } = new IP_Port();

		public MainWindow()
		{
			InitializeComponent();

			connectStack.DataContext = Host;
		}

		private void Button_Click_GetAgents(object sender, RoutedEventArgs e)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, $"{Host}api/Agents/agents");
			request.Headers.Add("Accept", "application/vnd.github.v3+json");

			Agents_ListBox.ItemsSource = GetData<AgentInfo>(request);
			connectButton.IsEnabled = false;
		}

		private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Agents_ListBox.SelectedItem is AgentInfo agent &&
				tabMetrics.SelectedItem is TabItem tab &&
				tab.Tag is string tabType &&
				tab.Content is MaterialCards card)
			{
				var toTime = DateTime.UtcNow;
				var fromTime = DateTime.UtcNow.AddMinutes(-10);

				var request = new HttpRequestMessage(HttpMethod.Get, $"{Host}api/metrics/{tabType}/agent/{agent.Id}/from/{fromTime:s}/to/{toTime:s}");
				request.Headers.Add("Accept", "application/vnd.github.v3+json");

				var result = GetData<AgentMetricEntity>(request);
				card.SetValues(result.Select(x => x.Value));
			}
		}

		private TResult[] GetData<TResult>(HttpRequestMessage request)
		{
			try
			{
				var client = new HttpClient();
				client.Timeout = new TimeSpan(0, 0, 15);
				var response = client.SendAsync(request).Result;

				if (response.IsSuccessStatusCode)
				{
					using var responseStream = response.Content.ReadAsStreamAsync().Result;
					return JsonSerializer.DeserializeAsync<TResult[]>(responseStream, new JsonSerializerOptions(JsonSerializerDefaults.Web)).Result;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			return new TResult[] { };
		}

		private void Agents_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(sender is ListBox listBox)
			{
				tabMetrics.SelectedIndex = -1;
				tabMetrics.SelectedIndex = 0;
			}
		}
	}
}
