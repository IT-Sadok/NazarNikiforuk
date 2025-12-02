using BookingSystem.Application.DTOs.Auth;

namespace BookingSystem.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
}
