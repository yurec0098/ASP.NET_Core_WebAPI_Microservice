using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson_1.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private readonly IList<WeatherForecast> _weathers;

		public WeatherForecastController(List<WeatherForecast> weathers)
		{
			_weathers = weathers;
		}


		[HttpGet]
		public IList<WeatherForecast> GetRange([FromQuery] DateTime from, [FromQuery] DateTime to)
		{
			return _weathers.Where(w => w.Date >= from && w.Date <= to).OrderBy(w => w.Date).ToList();
		}

		[HttpPost]
		public IActionResult Add([FromQuery] WeatherForecast weatherForecast)
		{
			if (_weathers.Any(w => w.EqualsDate(weatherForecast.Date)))
				return BadRequest();
			
			_weathers.Add(weatherForecast);
			return Ok();
		}

		[HttpDelete]
		public IActionResult Delete([FromQuery] DateTime from, [FromQuery] DateTime to)
		{
			foreach(var item in _weathers.Where(w => w.Date >= from && w.Date <= to).ToList())
				_weathers.Remove(item);

			return Ok();
		}

		[HttpPut]
		public IActionResult Update([FromQuery] WeatherForecast weatherForecast)
		{
			if (_weathers.FirstOrDefault(w => w.EqualsDate(weatherForecast.Date)) is WeatherForecast weather)
			{
				weather.CopyData(weatherForecast);
				return Ok();
			}
			else
				return BadRequest();
		}
	}
}
