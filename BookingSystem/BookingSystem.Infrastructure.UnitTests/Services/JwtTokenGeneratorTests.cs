using System.IdentityModel.Tokens.Jwt;
using BookingSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using BookingSystem.Domain.Settings;
using BookingSystem.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;

namespace BookingSystem.Infrastructure.UnitTests.Services;

public class JwtTokenGeneratorTests
{
    private Mock<UserManager<User>> MockUserManager()
    {
        return new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null
        );
    }

    [Fact]
    public async Task GenerateToken_Should_Return_Valid_Jwt_And_Contain_Claims()
    {
        // Arrange
        var options = Options.Create(new JwtSettings
        {
            SecretKey = "super-secret-test-key-which-needs-to-be-long-enough",
            Issuer = "test-issuer",
            Audience = "test-audience",
            ExpiresInMinutes = 60
        });

        var userManagerMock = MockUserManager();
        userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "Admin", "User" });

        var generator = new JwtTokenGenerator(options, userManagerMock.Object);

        var user = new User
        {
            Id = "1",
            Email = "a@b.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "123"
        };

        // Act
        var token = await generator.GenerateToken(user);

        // Assert
        token.ShouldNotBeNullOrEmpty();

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        jwt.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Sub || c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        jwt.Claims.ShouldContain(c => c.Type == System.Security.Claims.ClaimTypes.Email && c.Value == user.Email);
        jwt.Claims.ShouldContain(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == "Admin");
        jwt.Claims.ShouldContain(c => c.Type == System.Security.Claims.ClaimTypes.GivenName && c.Value == user.FirstName);
    }
}
