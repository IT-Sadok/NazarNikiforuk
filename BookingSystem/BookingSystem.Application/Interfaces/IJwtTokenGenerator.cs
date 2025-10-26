using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
