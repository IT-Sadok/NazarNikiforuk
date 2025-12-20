namespace BookingSystem.Application.DTOs;

public class PropertyDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal PricePerNight { get; set; }
    public int Bedrooms { get; set; }
    public int MaxGuests { get; set; }
    public int Floor { get; set; }
    public bool IsAvailable { get; set; } = true;    
}
