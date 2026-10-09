using Microsoft.EntityFrameworkCore;
using ReservationService.Models;

namespace ReservationService.Data;

public class ReservationDbContext(DbContextOptions<ReservationDbContext> options) : DbContext(options)
{
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hotel>().HasIndex(h => h.HotelUid).IsUnique();
        modelBuilder.Entity<Reservation>().HasIndex(r => r.ReservationUid).IsUnique();
    }
}