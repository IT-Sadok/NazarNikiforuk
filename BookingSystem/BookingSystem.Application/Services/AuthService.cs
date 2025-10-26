using BookingSystem.Application.DTOs;
using BookingSystem.Application.DTOs.Auth;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;
using FluentValidation;

namespace BookingSystem.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IValidator<RegisterDto> registerValidator) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var validationResult = await registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (await userRepository.ExistsByEmailAsync(registerDto.Email))
            {
                throw new DomainException("User with this email already exists");
            }

            var user = new User
            {
                Email = registerDto.Email,
                Password = passwordHasher.HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                Role = registerDto.Role
            };

            await userRepository.AddAsync(user);

            var token = jwtTokenGenerator.GenerateToken(user);
            
            return new AuthResponseDto
            {
                Token = token, 
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role
                }
            };
        }
    
    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetByEmailAsync(loginDto.Email);

        if (user == null || !passwordHasher.VerifyPassword(loginDto.Password, user.Password))
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
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            }
        };
    }
}
