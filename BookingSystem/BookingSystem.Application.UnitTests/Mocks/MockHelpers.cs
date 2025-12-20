using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BookingSystem.Application.UnitTests.Mocks;

public static class MockHelpers
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
    
    public static Mock<IPropertyRepository> CreatePropertyRepositoryMock()
    {
        var mock = new Mock<IPropertyRepository>();
        
        mock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Property>());
        mock.Setup(x => x.GetAvailablePropertiesAsync(
                It.IsAny<DateTime?>(), 
                It.IsAny<DateTime?>(),
                It.IsAny<string?>(),
                It.IsAny<decimal?>(),
                It.IsAny<decimal?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
            .ReturnsAsync(new List<Property>());
        mock.Setup(x => x.GetPaginatedAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<decimal?>(),
                It.IsAny<decimal?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
            .ReturnsAsync((new List<Property>(), 0));
        mock.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Property?)null);
        mock.Setup(x => x.CreateAsync(It.IsAny<Property>()))
            .Returns(Task.CompletedTask);
        mock.Setup(x => x.UpdateAsync(It.IsAny<Property>()))
            .Returns(Task.CompletedTask);
        mock.Setup(x => x.DeleteAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);
            
        return mock;
    }

    public static Mock<IBookingRepository> CreateBookingRepositoryMock()
    {
        var mock = new Mock<IBookingRepository>();
        
        mock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Booking>());
        mock.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Booking?)null);
        mock.Setup(x => x.GetByUserIdPaginatedAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<BookingStatus?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>()))
            .ReturnsAsync((new List<Booking>(), 0));
        mock.Setup(x => x.GetByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<Booking>());
        mock.Setup(x => x.GetByPropertyIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<Booking>());
        mock.Setup(x => x.IsPropertyAvailableAsync(
                It.IsAny<string>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>()))
            .ReturnsAsync(true);
        mock.Setup(x => x.AddAsync(It.IsAny<Booking>()))
            .Returns(Task.CompletedTask);
        mock.Setup(x => x.UpdateAsync(It.IsAny<Booking>()))
            .Returns(Task.CompletedTask);
        mock.Setup(x => x.DeleteAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);
            
        return mock;
    }

    public static Mock<UserManager<User>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null);
    }
}
