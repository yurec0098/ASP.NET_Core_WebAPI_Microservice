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

		//public DbSet<Animal> Animals { get; set; }

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		//	modelBuilder.Entity<Animal>().Property(a => a.Name).HasColumnType("varchar(200)");
		}
	}
}
