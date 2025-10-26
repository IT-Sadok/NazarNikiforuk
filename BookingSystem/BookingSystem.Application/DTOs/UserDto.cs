using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public UserRole Role { get; set; }
}
