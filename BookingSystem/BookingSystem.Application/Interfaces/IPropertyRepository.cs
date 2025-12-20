using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(string id);
    Task<IEnumerable<Property>> GetAllAsync();
    Task<(IEnumerable<Property> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, 
        int pageSize,
        string? search = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? minBedrooms = null,
        int? maxBedrooms = null);
    Task<IEnumerable<Property>> GetAvailablePropertiesAsync(
        DateTime? checkIn, 
        DateTime? checkOut,
        string? search = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? minBedrooms = null,
        int? maxBedrooms = null);
    Task CreateAsync(Property property);
    Task UpdateAsync(Property property);
    Task DeleteAsync(string id);
}
