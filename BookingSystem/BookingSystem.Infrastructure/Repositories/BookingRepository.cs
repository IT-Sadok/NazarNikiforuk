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
            .AsNoTracking()
            .Include(b => b.Property)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await context.Bookings
            .AsNoTracking()
            .Include(b => b.Property)
            .Include(b => b.User)
            .OrderByDescending(b => b.CheckInDate)
            .Take(100)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Booking> Items, int TotalCount)> GetByUserIdPaginatedAsync(
        string userId,
        int pageNumber,
        int pageSize,
        BookingStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        var query = context.Bookings
            .AsNoTracking()
            .Include(b => b.Property)
            .Where(b => b.UserId == userId);

        if (status.HasValue)
        {
            query = query.Where(b => b.Status == status.Value);
        }
        if (fromDate.HasValue)
        {
            query = query.Where(b => b.CheckInDate >= fromDate.Value);
        }
        if (toDate.HasValue)
        {
            query = query.Where(b => b.CheckOutDate <= toDate.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(b => b.CheckInDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(string userId)
    {
        return await context.Bookings
            .AsNoTracking()
            .Include(b => b.Property)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CheckInDate)
            .Take(50)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByPropertyIdAsync(string propertyId)
    {
        return await context.Bookings
            .AsNoTracking()
            .Include(b => b.User)
            .Where(b => b.PropertyId == propertyId)
            .OrderBy(b => b.CheckInDate)
            .Take(50)
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
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == propertyId && p.IsAvailable);

        if (property == null) return false;
        
        var hasConflictingBooking = await context.Bookings
            .AsNoTracking()
            .AnyAsync(b => b.PropertyId == propertyId &&
                           b.CheckInDate < checkOut &&
                           b.CheckOutDate > checkIn &&
                           (b.Status == BookingStatus.Pending || 
                            b.Status == BookingStatus.Confirmed));

        return !hasConflictingBooking;
    }
}