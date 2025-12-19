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
            .Take(100)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Property> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, 
        int pageSize,
        string? search = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? minBedrooms = null,
        int? maxBedrooms = null)
    {
        var query = context.Properties
            .Where(p => p.IsAvailable)
            .AsNoTracking();

        query = ApplyFilters(query, search, minPrice, maxPrice, minBedrooms, maxBedrooms);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Property>> GetAvailablePropertiesAsync(
        DateTime? checkIn, 
        DateTime? checkOut,
        string? search = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? minBedrooms = null,
        int? maxBedrooms = null)
    {
        var query = context.Properties
            .Where(p => p.IsAvailable);

        query = ApplyFilters(query, search, minPrice, maxPrice, minBedrooms, maxBedrooms);

        if (checkIn.HasValue && checkOut.HasValue)
        {
            var propertiesWithBookings = await context.Bookings
                .Where(b => b.CheckInDate < checkOut.Value && 
                           b.CheckOutDate > checkIn.Value &&
                           (b.Status == BookingStatus.Pending || 
                            b.Status == BookingStatus.Confirmed))
                .Select(b => b.PropertyId)
                .ToListAsync();
            query = query.Where(p => !propertiesWithBookings.Contains(p.Id));
        }

        return await query
            .OrderBy(p => p.PricePerNight)
            .Take(100)
            .ToListAsync();
    }

    private static IQueryable<Property> ApplyFilters(
        IQueryable<Property> query,
        string? search,
        decimal? minPrice,
        decimal? maxPrice,
        int? minBedrooms,
        int? maxBedrooms)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => 
                p.Title!.Contains(search) || 
                p.Description!.Contains(search));
        }
        if (minPrice.HasValue)
        {
            query = query.Where(p => p.PricePerNight >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.PricePerNight <= maxPrice.Value);
        }
        if (minBedrooms.HasValue)
        {
            query = query.Where(p => p.Bedrooms >= minBedrooms.Value);
        }
        if (maxBedrooms.HasValue)
        {
            query = query.Where(p => p.Bedrooms <= maxBedrooms.Value);
        }

        return query;
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
