using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.DTOs.Auth;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BookingSystem.Application.Services;

public class AuthService(
    UserManager<User> userManager,
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher<User> passwordHasher,
    IMapper mapper) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                throw new DomainException("User with this email already exists");
            }

            var user = mapper.Map<User>(registerDto);
            user.PasswordHash = passwordHasher.HashPassword(user, registerDto.Password);
            
            await userManager.CreateAsync(user, registerDto.Password);

            await userManager.AddToRoleAsync(user, "User");
            var token = jwtTokenGenerator.GenerateToken(user);
            
            return new AuthResponseDto
            {
                Token = await token, 
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                }
            };
        }
    
    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        
        var result = passwordHasher.VerifyHashedPassword(user, user?.PasswordHash ?? string.Empty, loginDto.Password);

        if (user == null || result == PasswordVerificationResult.Failed)
        {
            throw new DomainException("Invalid email or password");
        }

        if (!user.IsActive)
        {
            throw new DomainException("Account is deactivated");
        }

        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = await token,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            }
        };
    }
}
