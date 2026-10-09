using LoyaltyService.Models;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyService.Data;

public class LoyaltyDbContext(DbContextOptions<LoyaltyDbContext> options) : DbContext(options)
{
    public DbSet<Loyalty> Loyalties => Set<Loyalty>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Loyalty>().HasIndex(l => l.Username).IsUnique();
    }
}