using MetricsAgent.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
			ConfigureSqlLiteConnection(services);
			services.AddScoped<ICpuMetricsRepository, CpuMetricsRepositorySQLite>();
			services.AddScoped<IDotNetMetricsRepository, DotNetMetricsRepositorySQLite>();
			services.AddScoped<IHddMetricsRepository, HddMetricsRepositorySQLite>();
			services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepositorySQLite>();
			services.AddScoped<IRamMetricsRepository, RamMetricsRepositorySQLite>();

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent", Version = "v1" });
			});
		}


		private void ConfigureSqlLiteConnection(IServiceCollection services)
		{
			var tables = new string[] { "cpumetrics", "dotnetmetrics", "hddmetrics", "networkmetrics", "rammetrics", };

		//	const string connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
			string ConnectionString = Configuration["ConnectionString:MyConnection"];
			var connection = new SQLiteConnection(ConnectionString);
			connection.Open();

			foreach (var table in tables)
			{
				PrepareSchema(connection, table);
				GenereteMetricsData(connection, table);
			}
		}

		private void PrepareSchema(SQLiteConnection connection, string table)
		{
			// задаем новый текст команды для выполнения
			// удаляем таблицу с метриками если она существует в базе данных
			var commandText = $"DROP TABLE IF EXISTS {table}";

			using (var command = new SQLiteCommand(commandText, connection))
			{
				// отправляем запрос в базу данных
				command.ExecuteNonQuery();

				command.CommandText = @$"CREATE TABLE {table}(id INTEGER PRIMARY KEY, value INT, time INTEGER)";
				command.ExecuteNonQuery();
			}
		}
		private void GenereteMetricsData(SQLiteConnection connection, string table)
		{
			var random = new Random(DateTime.Now.Millisecond);
			var time = DateTime.Now;

			for (int i = 0; i < 100; i++)
			{
				// прописываем в команду SQL запрос на вставку данных
				var commandText = $"INSERT INTO {table}(value, time) VALUES(@value, @time)";
				// создаем команду
				using var cmd = new SQLiteCommand(commandText, connection);
				cmd.Parameters.AddWithValue("@value", random.Next(100));
				cmd.Parameters.AddWithValue("@time", time.AddSeconds(i).Ticks);
				// подготовка команды к выполнению
				cmd.Prepare();

				// выполнение команды
				cmd.ExecuteNonQuery();
			}
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
