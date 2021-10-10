using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace ManagerClientWPF.Controls
{
	/// <summary>
	/// Логика взаимодействия для MaterialCards.xaml
	/// </summary>
	public partial class MaterialCards : UserControl, INotifyPropertyChanged
	{
		public MaterialCards()
		{
			InitializeComponent();

			ColumnSeriesValues = new SeriesCollection
			{
				new ColumnSeries
				{
					Values = new ChartValues<double> { 10,20,30,40,50,60,70,80,90.100 }
				}
			};

			DataContext = this;
		}

		public SeriesCollection ColumnSeriesValues { get; set; }
		public double Percent
		{
			get
			{
				double sum = 0;
				foreach (double x in ColumnSeriesValues[0].Values)
					sum = sum + x;

				return sum / ColumnSeriesValues[0].Values.Count;
			}

			set => OnPropertyChanged();
		}
		public string HeaderName { get; set; } = "Cpu Load";


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void UpdateOnСlick(object sender, RoutedEventArgs e)
		{
			TimePowerChart.Update(true);
		}

		public void SetValues(IEnumerable<double> enumerable)
		{
			ColumnSeriesValues[0].Values = new ChartValues<double>(enumerable);
			OnPropertyChanged("Percent");
		}
	}
}
