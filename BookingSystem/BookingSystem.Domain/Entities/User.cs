using BookingSystem.Domain.Common;
using BookingSystem.Domain.Enums;

namespace BookingSystem.Domain.Entities;

public class User : BaseEntity
{
    public required string Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string PhoneNumber { get; set; }
    
    public bool IsActive { get; set; } =  true;
    
    public UserRole Role { get; set; }
}
