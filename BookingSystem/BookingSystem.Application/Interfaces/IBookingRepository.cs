using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(string id);
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<(IEnumerable<Booking> Items, int TotalCount)> GetByUserIdPaginatedAsync(
        string userId,
        int pageNumber,
        int pageSize,
        BookingStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null);
    Task<IEnumerable<Booking>> GetByUserIdAsync(string userId);
    Task<IEnumerable<Booking>> GetByPropertyIdAsync(string propertyId);
    Task AddAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task DeleteAsync(string id);
    Task<bool> IsPropertyAvailableAsync(string propertyId, DateTime checkIn, DateTime checkOut);
}
