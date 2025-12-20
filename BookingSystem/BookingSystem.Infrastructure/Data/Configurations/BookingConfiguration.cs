using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(b => b.NumberOfGuests)
            .IsRequired();
            
        builder.Property(b => b.CheckInDate)
            .IsRequired();
            
        builder.Property(b => b.CheckOutDate)
            .IsRequired();

        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(b => b.Property)
            .WithMany()
            .HasForeignKey(b => b.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.UserId);
        
        builder.HasIndex(b => new { b.PropertyId, b.CheckInDate, b.CheckOutDate, b.Status });

        builder.HasCheckConstraint(
            "CK_Booking_ValidDates", 
            "[CheckInDate] < [CheckOutDate]");
            
        builder.HasCheckConstraint(
            "CK_Booking_NumberOfGuests", 
            "[NumberOfGuests] > 0");
    }
}
