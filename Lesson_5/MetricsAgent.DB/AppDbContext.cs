using MetricsAgent.DB.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace MetricsAgent.DB
{
	public sealed class AppDbContext : DbContext
	{
		/// <summary>
		/// AppDbContext
		/// </summary>
		/// <param name="options">options</param>
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}

		public DbSet<CpuMetric> CpuMetrics { get; set; }
		public DbSet<HddMetric> HddMetrics { get; set; }
		public DbSet<RamMetric> RamMetrics { get; set; }
		public DbSet<NetworkMetric> NetworkMetrics { get; set; }
		public DbSet<DotNetMetric> DotNetMetrics { get; set; }

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Filename=metrics.db");
		}
	}
}
