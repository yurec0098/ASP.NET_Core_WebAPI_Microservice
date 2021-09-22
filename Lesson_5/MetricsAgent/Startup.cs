using MetricsAgent.Controllers;
using MetricsAgent.DB;
using MetricsAgent.DB.Entity;
using MetricsAgent.Jobs;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(Configuration.GetConnectionString("MyConnection")), ServiceLifetime.Singleton);
			services.AddSingleton<IDbRepository<CpuMetric>, DbRepository<CpuMetric>>();
			services.AddSingleton<IDbRepository<HddMetric>, DbRepository<HddMetric>>();
			services.AddSingleton<IDbRepository<RamMetric>, DbRepository<RamMetric>>();
			services.AddSingleton<IDbRepository<DotNetMetric>, DbRepository<DotNetMetric>>();
			services.AddSingleton<IDbRepository<NetworkMetric>, DbRepository<NetworkMetric>>();

			services.AddSingleton<IJobFactory, JobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

			services.AddSingleton<MetricsJob>();
			services.AddSingleton(new JobSchedule(
					   typeof(MetricsJob),
					   Configuration.GetValue<string>("CronExpression"))); /*"0/5 * * * * ?"*/

			services.AddHostedService<QuartzHostedService>();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsAgent v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
