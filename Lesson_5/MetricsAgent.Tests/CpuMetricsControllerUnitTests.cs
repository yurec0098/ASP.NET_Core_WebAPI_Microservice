using MetricsAgent.Controllers;
using MetricsAgent.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async void GetMetrics_ReturnsOk()
        {
            //Arrange
            var fromTime = time.AddSeconds(10);
            var toTime = time.AddSeconds(50);

            //Act
            var result = await controller.GetByTimePeriod(fromTime, toTime);

            // Assert
            Assert.IsAssignableFrom<List<CpuMetric>>(result);
        }

        [Fact]
        public void Create_ReturnsOk()
        {
			var request = new CpuMetric() { Time = DateTime.Now.Ticks, Value = _random.Next() };

			//Act
			var result = controller.Add(request);

            // Assert
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void GetAll_ReturnsOk()
        {
			//Act
			var result = controller.GetAll();

            // Assert
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void GetById_ReturnsOk()
        {
			//Act
			var result = controller.GetById(_random.Next(100));

            // Assert
            Assert.True(result.IsCompletedSuccessfully);
        }
	}
}
