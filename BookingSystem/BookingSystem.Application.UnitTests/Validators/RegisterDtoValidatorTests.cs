using BookingSystem.Application.DTOs.Auth;
using BookingSystem.Application.Validators;
using Shouldly;

namespace BookingSystem.Application.UnitTests.Validators;

public class RegisterDtoValidatorTests
{
    private readonly RegisterDtoValidator _validator = new RegisterDtoValidator();

    [Fact]
    public void Validate_Should_Pass_For_Valid_Dto()
    {
        var dto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "long-enough",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "123456"
        };

        var result = _validator.Validate(dto);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Validate_Should_Fail_For_Missing_Email()
    {
        var dto = new RegisterDto { Password = "long-enough", FirstName = "John", LastName = "Doe" };

        var result = _validator.Validate(dto);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Email");
    }
}
