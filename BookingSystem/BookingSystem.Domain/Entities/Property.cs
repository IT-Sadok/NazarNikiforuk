using BookingSystem.Domain.Common;

namespace BookingSystem.Domain.Entities;

public class Property : BaseEntity
{
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public decimal PricePerNight { get; set; }
    
    public int Bedrooms { get; set; }
    
    public int MaxGuests { get; set; }
    
    public int Floor { get; set; }
    
    public bool IsAvailable { get; set; } = true;
}
