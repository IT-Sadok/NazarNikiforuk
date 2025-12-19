namespace BookingSystem.Application.DTOs;

public class CreatePropertyDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal PricePerNight { get; set; }
    public int Bedrooms { get; set; }
    public int MaxGuests { get; set; }
    public int Floor { get; set; }
}
