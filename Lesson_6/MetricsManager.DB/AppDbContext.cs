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
			Database.EnsureDeleted();
			Database.EnsureCreated();

			Agents.Add(new AgentInfo() { AgentAddress = new Uri("http://localhost:5000") });
			Agents.Add(new AgentInfo() { AgentAddress = new Uri("https://localhost:5001") });
			SaveChangesAsync();
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
