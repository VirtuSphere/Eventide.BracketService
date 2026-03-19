using Microsoft.EntityFrameworkCore;
using BracketService.Models;

namespace BracketService.Data
{
    public class BracketDbContext : DbContext
    {
        public BracketDbContext(DbContextOptions<BracketDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bracket> Brackets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bracket>()
                .HasIndex(b => b.TournamentId)
                .IsUnique();

            modelBuilder.Entity<Bracket>()
                .Property(b => b.StructureJson)
                .HasColumnType("jsonb");
        }
    }
}