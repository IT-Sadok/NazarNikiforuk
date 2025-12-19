using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(string id);
    Task<IEnumerable<Property>> GetAllAsync();
    Task<IEnumerable<Property>> GetAvailablePropertiesAsync(DateTime? checkIn, DateTime? checkOut);
    Task CreateAsync(Property property);
    Task UpdateAsync(Property property);
    Task DeleteAsync(string id);
}
