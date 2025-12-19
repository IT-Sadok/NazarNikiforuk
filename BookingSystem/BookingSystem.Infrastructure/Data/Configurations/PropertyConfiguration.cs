using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Infrastructure.Data.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(p => p.Description)
            .HasMaxLength(2000);
            
        builder.Property(p => p.PricePerNight)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(p => p.Bedrooms)
            .IsRequired();
            
        builder.Property(p => p.MaxGuests)
            .IsRequired();
            
        builder.Property(p => p.Floor)
            .IsRequired();

        builder.HasIndex(p => p.IsAvailable)
            .HasFilter("[IsAvailable] = 1");
    }
}
