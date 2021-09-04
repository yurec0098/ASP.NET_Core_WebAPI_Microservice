using System;

namespace Lesson_1
{
	public class WeatherForecast
	{
		public DateTime Date { get; set; }

		public int TemperatureC { get; set; }

		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);


		public bool EqualsDate(DateTime dateTime)
		{
			if (Date.Day != dateTime.Day)
				return false;
			if (Date.Month != dateTime.Month)
				return false;
			if (Date.Year != dateTime.Year)
				return false;

			return true;
		}

		public void CopyData(WeatherForecast weather)
		{
			Date = weather.Date;
			TemperatureC = weather.TemperatureC;
		}
	}
}
