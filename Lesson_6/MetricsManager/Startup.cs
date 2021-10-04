using MetricsManager.Client;
using MetricsManager.DB;
using MetricsManager.DB.Entity;
using MetricsManager.Jobs;
using MetricsManager.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager
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

			services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(Configuration.GetConnectionString("MyConnection")));
			services.AddScoped<IDbRepository<AgentInfo>, DbRepository<AgentInfo>>();
			services.AddScoped<IDbRepository<CpuMetric>, DbRepository<CpuMetric>>();
			services.AddScoped<IDbRepository<HddMetric>, DbRepository<HddMetric>>();
			services.AddScoped<IDbRepository<RamMetric>, DbRepository<RamMetric>>();
			services.AddScoped<IDbRepository<DotNetMetric>, DbRepository<DotNetMetric>>();
			services.AddScoped<IDbRepository<NetworkMetric>, DbRepository<NetworkMetric>>();

			services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>()
				.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));

			services.AddSingleton<IJobFactory, JobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
			services.AddSingleton<AgentsJob>();
			services.AddSingleton(new JobSchedule(typeof(AgentsJob),
					   Configuration.GetValue("CronExpression", "0/600 * * * * ?")));
			services.AddHostedService<QuartzHostedService>();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsManager", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsManager v1"));
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
