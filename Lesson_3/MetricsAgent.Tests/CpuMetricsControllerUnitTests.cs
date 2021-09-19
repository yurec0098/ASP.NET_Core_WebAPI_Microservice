using MetricsAgent.Controllers;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgent.Tests
{
	public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsController controller;
        private DateTime time;
		private Random _random = new Random();

		public CpuMetricsControllerUnitTests()
        {
            time = DateTime.Now;
            controller = new CpuMetricsController(null, new CpuMetricsRepoTest());
        }

        [Fact]
        public void GetMetrics_ReturnsOk()
        {
            //Arrange
            var fromTime = time.AddSeconds(10);
            var toTime = time.AddSeconds(50);

            //Act
            var result = controller.GetMetrics(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Create_ReturnsOk()
        {
			var request = new CpuMetricCreateRequest() { Time = DateTime.Now, Value = _random.Next() };

			//Act
			var result = controller.Create(request);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetAll_ReturnsOk()
        {
			//Act
			var result = controller.GetAll();

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetById_ReturnsOk()
        {
			//Act
			var result = controller.GetById(_random.Next(100));

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
	}
}
