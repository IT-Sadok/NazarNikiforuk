using Microsoft.EntityFrameworkCore;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Enums;
using BookingSystem.Infrastructure.Data;

namespace BookingSystem.Infrastructure.Repositories;

public class BookingRepository(ApplicationDbContext context) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(string id)
    {
        return await context.Bookings
            .Include(b => b.Property)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await context.Bookings
            .Include(b => b.Property)
            .Include(b => b.User)
            .OrderBy(b => b.CheckInDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(string userId)
    {
        return await context.Bookings
            .Include(b => b.Property)
            .Where(b => b.UserId == userId)
            .OrderBy(b => b.CheckInDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByPropertyIdAsync(string propertyId)
    {
        return await context.Bookings
            .Include(b => b.User)
            .Where(b => b.PropertyId == propertyId)
            .ToListAsync();
    }

    public async Task AddAsync(Booking booking)
    {
        await context.Bookings.AddAsync(booking);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        context.Bookings.Update(booking);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var booking = await context.Bookings.FindAsync(id);
        if (booking != null)
        {
            context.Bookings.Remove(booking);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsPropertyAvailableAsync(string propertyId, DateTime checkIn, DateTime checkOut)
    {
        var property = await context.Properties
            .FirstOrDefaultAsync(p => p.Id == propertyId && p.IsAvailable);

        if (property == null) return false;
        
        var hasConflictingBooking = await context.Bookings
            .AnyAsync(b => b.PropertyId == propertyId &&
                           b.CheckInDate < checkOut &&
                           b.CheckOutDate > checkIn &&
                           (b.Status == BookingStatus.Pending || 
                            b.Status == BookingStatus.Confirmed));

        return !hasConflictingBooking;
    }
}