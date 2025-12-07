using BookingSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public ApplicationDbContext() 
    {
    }
    
    public required DbSet<Property> Properties { get; set; }
    public required DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.FirstName, u.LastName });
        
        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(p => p.Id);
            
            entity.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200);
                
            entity.Property(p => p.Description)
                .HasMaxLength(2000);
                
            entity.Property(p => p.PricePerNight)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
                
            entity.Property(p => p.Bedrooms)
                .IsRequired();
                
            entity.Property(p => p.MaxGuests)
                .IsRequired();
                
            entity.Property(p => p.Floor)
                .IsRequired();

            entity.HasIndex(p => p.IsAvailable)
                .HasFilter("[IsAvailable] = 1");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.Id);
            
            entity.Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
                
            entity.Property(b => b.NumberOfGuests)
                .IsRequired();
                
            entity.Property(b => b.CheckInDate)
                .IsRequired();
                
            entity.Property(b => b.CheckOutDate)
                .IsRequired();

            entity.Property(b => b.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasOne(b => b.Property)
                .WithMany()
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(b => b.UserId);
            
            entity.HasIndex(b => new { b.PropertyId, b.CheckInDate, b.CheckOutDate, b.Status });

            entity.HasCheckConstraint(
                "CK_Booking_ValidDates", 
                "[CheckInDate] < [CheckOutDate]");
                
            entity.HasCheckConstraint(
                "CK_Booking_NumberOfGuests", 
                "[NumberOfGuests] > 0");
        });

    }
}
