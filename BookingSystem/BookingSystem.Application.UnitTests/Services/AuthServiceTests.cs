using AutoMapper;
using BookingSystem.Application.DTOs.Auth;
using BookingSystem.Application.Interfaces;
using BookingSystem.Application.Services;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;

public class AuthServiceTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IJwtTokenGenerator> _jwtMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _userManagerMock = MockUserManager();
        _jwtMock = new Mock<IJwtTokenGenerator>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();

        _mapperMock = new Mock<IMapper>();

        _service = new AuthService(
            _userManagerMock.Object,
            _jwtMock.Object,
            _passwordHasherMock.Object,
            _mapperMock.Object
        );
    }

    private Mock<UserManager<User>> MockUserManager()
    {
        return new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null
        );
    }

    [Fact]
    public async Task RegisterAsync_Should_Register_When_Data_Is_Valid()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Email = "test@mail.com",
            Password = "pass123",
            FirstName = "John",
            LastName = "Doe"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), dto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), "User"))
            .ReturnsAsync(IdentityResult.Success);

        _jwtMock.Setup(x => x.GenerateToken(It.IsAny<User>()))
            .ReturnsAsync("jwt-token");

        _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<User>(), dto.Password))
            .Returns("hashed");
        
        _mapperMock.Setup(m => m.Map<User>(It.IsAny<RegisterDto>())).Returns((RegisterDto r) => new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = r.Email ?? string.Empty,
            FirstName = r.FirstName ?? string.Empty,
            LastName = r.LastName ?? string.Empty,
            PhoneNumber = r.PhoneNumber
        });

        // Act
        var result = await _service.RegisterAsync(dto);

        // Assert
        result.Token.ShouldBe("jwt-token");
        result.User.Email.ShouldBe(dto.Email);
    }

    [Fact]
    public async Task RegisterAsync_Should_Throw_When_Email_Exists()
    {
        // Arrange
        var dto = new RegisterDto { Email = "exists@mail.com" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync(new User { FirstName = "a", LastName = "b" });

        // Act
        var act = async () => await _service.RegisterAsync(dto);

        // Assert
        await act.ShouldThrowAsync<DomainException>();
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Token_When_Credentials_Valid()
    {
        // Arrange
        var dto = new LoginDto { Email = "valid@mail.com", Password = "pass" };
        var user = new User
        {
            Id = "1",
            Email = dto.Email,
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hashed",
            IsActive = true
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _passwordHasherMock.Setup(x =>
            x.VerifyHashedPassword(user, user.PasswordHash, dto.Password))
            .Returns(PasswordVerificationResult.Success);

        _jwtMock.Setup(x => x.GenerateToken(user))
            .ReturnsAsync("jwt-token");

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        result.Token.ShouldBe("jwt-token");
        result.User.Email.ShouldBe(dto.Email);
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_When_User_Not_Found()
    {
        var dto = new LoginDto { Email = "bad@mail.com", Password = "pass" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);

        var act = async () => await _service.LoginAsync(dto);

        await act.ShouldThrowAsync<DomainException>();
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_When_Password_Incorrect()
    {
        var dto = new LoginDto { Email = "valid@mail.com", Password = "pass" };
        var user = new User { Id = "1", Email = dto.Email, PasswordHash = "hashed", IsActive = true, FirstName = "John", LastName = "Doe" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync(user);

        _passwordHasherMock.Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, dto.Password))
            .Returns(PasswordVerificationResult.Failed);

        var act = async () => await _service.LoginAsync(dto);

        await act.ShouldThrowAsync<DomainException>();
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_When_User_Is_Deactivated()
    {
        var dto = new LoginDto { Email = "valid@mail.com", Password = "pass" };
        var user = new User { Id = "1", Email = dto.Email, PasswordHash = "hashed", IsActive = false, FirstName = "John", LastName = "Doe" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync(user);

        _passwordHasherMock.Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, dto.Password))
            .Returns(PasswordVerificationResult.Success);

        var act = async () => await _service.LoginAsync(dto);

        await act.ShouldThrowAsync<DomainException>();
    }
}
