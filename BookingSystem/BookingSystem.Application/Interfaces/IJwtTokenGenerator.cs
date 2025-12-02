using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IJwtTokenGenerator
{
    Task<string> GenerateToken(User user);
}
