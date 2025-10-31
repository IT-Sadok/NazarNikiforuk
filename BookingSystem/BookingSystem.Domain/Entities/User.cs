using Microsoft.AspNetCore.Identity;

namespace BookingSystem.Domain.Entities;

public class User : IdentityUser
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }
    
    public bool IsActive { get; set; } =  true;
    
    public required IdentityRole Role { get; set; }
}
