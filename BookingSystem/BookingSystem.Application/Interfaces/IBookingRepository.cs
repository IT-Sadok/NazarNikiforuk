using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(string id);
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<IEnumerable<Booking>> GetByUserIdAsync(string userId);
    Task<IEnumerable<Booking>> GetByPropertyIdAsync(string propertyId);
    Task AddAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task DeleteAsync(string id);
    Task<bool> IsPropertyAvailableAsync(string propertyId, DateTime checkIn, DateTime checkOut);
}
