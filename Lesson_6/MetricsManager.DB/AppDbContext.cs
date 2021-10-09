using MetricsManager.DB.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace MetricsManager.DB
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

		public DbSet<AgentInfo> Agents { get; set; }

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
