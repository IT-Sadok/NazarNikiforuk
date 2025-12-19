using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.DTOs;

public class BookingDto
{
    public string? Id { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }  = BookingStatus.Pending;
    public string? PropertyId { get; set; }
    public string? UserId { get; set; }
}
