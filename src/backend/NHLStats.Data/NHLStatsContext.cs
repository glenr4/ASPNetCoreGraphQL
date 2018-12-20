using Microsoft.EntityFrameworkCore;
using NHLStats.Core.Models;

namespace NHLStats.Data
{
	public sealed class NHLStatsContext : DbContext
	{
		public NHLStatsContext(DbContextOptions options)
			: base(options)
		{
			// these are mutually exclusive, migrations cannot be used with EnsureCreated()
			// Database.EnsureCreated();
			Database.Migrate();
		}

		public DbSet<Player> Players { get; set; }
		public DbSet<Season> Seasons { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<League> Leagues { get; set; }
		public DbSet<SkaterStatistic> SkaterStatistics { get; set; }
		public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Address>(e =>
            {
                e.HasOne(o => o.Player)
                    .WithMany(o => o.Addresses)
                    .HasForeignKey(o => o.PlayerId);
            });
        }
    }
}