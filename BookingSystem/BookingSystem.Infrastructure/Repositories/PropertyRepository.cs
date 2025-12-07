using Microsoft.EntityFrameworkCore;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Enums;
using BookingSystem.Infrastructure.Data;

namespace BookingSystem.Infrastructure.Repositories;

public class PropertyRepository(ApplicationDbContext context) : IPropertyRepository
{
    public async Task<Property?> GetByIdAsync(string id)
    {
        return await context.Properties
           .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await context.Properties
            .Where(p => p.IsAvailable)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetAvailablePropertiesAsync(DateTime? checkIn, DateTime? checkOut)
    {
        var query = context.Properties
            .Where(p => p.IsAvailable);

        if (checkIn.HasValue && checkOut.HasValue)
        {
            var propertiesWithBookings = await context.Bookings
                .Where(b => b.CheckInDate < checkOut.Value && b.CheckOutDate > checkIn.Value &&
                           (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed))
                .Select(b => b.PropertyId)
                .ToListAsync();

            query = query.Where(p => !propertiesWithBookings.Contains(p.Id));
        }

        return await query.ToListAsync();
    }

    public async Task CreateAsync(Property property)
    {
        await context.Properties.AddAsync(property);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Property property)
    {
        context.Properties.Update(property);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var property = await context.Properties.FindAsync(id);
        if (property != null)
        {
            context.Properties.Remove(property);
            await context.SaveChangesAsync();
        }
    }
}