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
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MetricsAgent
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			RegisterToManager();
		}

		private void RegisterToManager()
		{
			var ManagerHost = Configuration.GetValue<Uri>("ManagerHost");
			var myHost = Configuration.GetValue<string>("Urls").Split(';').FirstOrDefault();

			var request = new HttpRequestMessage(HttpMethod.Post, $"{ManagerHost}api/Agents/register");
			request.Content = JsonContent.Create(new { agentAddress = myHost, agentName = Environment.UserName, isEnabled = true });
			request.Headers.Add("Accept", "application/vnd.github.v3+json");

			try
			{
				//Console.WriteLine($"Try connetc to {ManagerHost}");

				var client = new HttpClient();
				client.Timeout = new TimeSpan(0, 0, 15);
				var response = client.Send(request);
				if (response.IsSuccessStatusCode)
				{
					//Console.WriteLine($"Connetc to {ManagerHost} Is Success");
				}
				else
				{
					// ошибка при получении ответа
					//Console.WriteLine($"Connetc to {ManagerHost} Is Failed");
				}
			}
			catch(Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.InnerException.Message);
				Console.ResetColor();
			}
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(Configuration.GetConnectionString("MyConnection")));
			services.AddScoped<IDbRepository<CpuMetric>, DbRepository<CpuMetric>>();
			services.AddScoped<IDbRepository<HddMetric>, DbRepository<HddMetric>>();
			services.AddScoped<IDbRepository<RamMetric>, DbRepository<RamMetric>>();
			services.AddScoped<IDbRepository<DotNetMetric>, DbRepository<DotNetMetric>>();
			services.AddScoped<IDbRepository<NetworkMetric>, DbRepository<NetworkMetric>>();

			services.AddSingleton<IJobFactory, JobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
			services.AddSingleton<MetricsJob>();
			services.AddSingleton(new JobSchedule(typeof(MetricsJob),
					   Configuration.GetValue("CronExpression", "0/5 * * * * ?")));
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
