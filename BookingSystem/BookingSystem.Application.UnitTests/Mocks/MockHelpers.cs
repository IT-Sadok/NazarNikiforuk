using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BookingSystem.Application.UnitTests.Mocks;

public class MockHelpers
{
    public static Mock<IUserRepository> CreateUserRepositoryMock()
    {
        var mock = new Mock<IUserRepository>();
        return mock;
    }

    public static Mock<IJwtTokenGenerator> CreateJwtTokenGeneratorMock()
    {
        var mock = new Mock<IJwtTokenGenerator>();
        mock.Setup(x => x.GenerateToken(It.IsAny<User>()))
            .ReturnsAsync("fake-jwt-token");
        return mock;
    }

    public static Mock<IPasswordHasher<User>> CreatePasswordHasherMock()
    {
        var mock = new Mock<IPasswordHasher<User>>();
        
        mock.Setup(x => x.HashPassword(
                It.IsAny<User>(), 
                It.IsAny<string>()))
            .Returns("hashed-password");
            
        mock.Setup(x => x.VerifyHashedPassword(
                It.IsAny<User>(), 
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Success);
            
        return mock;
    }
}
