namespace BookingSystem.Application.DTOs;

public class CreateBookingDto
{
    public string? PropertyId { get; set; }
    
    public DateTime CheckInDate { get; set; }
    
    public DateTime CheckOutDate { get; set; }
    
    public int NumberOfGuests { get; set; }    
}
