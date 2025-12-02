using BookingSystem.Application.DTOs.Auth;
using BookingSystem.Application.Validators;
using Shouldly;

namespace BookingSystem.Application.UnitTests.Validators;

public class LoginDtoValidatorTests
{
    private readonly LoginDtoValidator _validator = new LoginDtoValidator();

    [Fact]
    public void Validate_Should_Pass_For_Valid_Dto()
    {
        var dto = new LoginDto { Email = "test@example.com", Password = "long-enough" };

        var result = _validator.Validate(dto);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Validate_Should_Fail_For_Short_Password()
    {
        var dto = new LoginDto { Email = "test@example.com", Password = "short" };

        var result = _validator.Validate(dto);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Password");
    }
}
