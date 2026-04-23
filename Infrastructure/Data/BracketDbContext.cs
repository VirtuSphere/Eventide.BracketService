using Eventide.BracketService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eventide.BracketService.Infrastructure.Data;

public class BracketDbContext : DbContext
{
    public DbSet<Bracket> Brackets => Set<Bracket>();

    public BracketDbContext(DbContextOptions<BracketDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bracket>(builder =>
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Type).HasConversion<string>().IsRequired();
            builder.Property(b => b.Status).HasConversion<string>().IsRequired();
            builder.HasIndex(b => b.TournamentId).IsUnique();
            builder.OwnsMany(b => b.Rounds, round =>
            {
                round.HasKey(r => r.Id);
                round.OwnsMany(r => r.Matches, match =>
                {
                    match.HasKey(m => m.Id);
                    match.Property(m => m.Status).HasConversion<string>();
                });
            });
        });
    }
}