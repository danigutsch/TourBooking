using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using TourBooking.Tours.Application;
using TourBooking.Tours.Domain;

namespace TourBooking.Tours.Persistence;

[MustDisposeResource]
internal sealed class ToursDbContext(DbContextOptions<ToursDbContext> options) : DbContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(500);
            entity.Property(t => t.Price).HasColumnType("decimal(18,2)");
            entity.Property(t => t.StartDate).IsRequired();
            entity.Property(t => t.EndDate).IsRequired();
        });
    }

    public async Task SaveChanges(CancellationToken ct) => await SaveChangesAsync(ct).ConfigureAwait(false);
}
