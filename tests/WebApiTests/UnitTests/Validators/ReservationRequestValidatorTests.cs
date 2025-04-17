using FluentValidation.TestHelper;
using Xunit;
using CarRental.src.DTOs.Reservation;

namespace WebApiTests.UnitTests.Validators;
public class ReservationRequestValidatorTests
{
    private readonly ReservationRequestValidator _validator;

    public ReservationRequestValidatorTests()
    {
        _validator = new ReservationRequestValidator();
    }

    [Fact]
    public void Validate_WithValidRequest_ShouldNotHaveErrors()
    {
        // Arrange
        var request = new ReservationRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com",
            PhoneNumber: "+1234567890",
            StartDate: DateTime.UtcNow.AddDays(1),
            EndDate: DateTime.UtcNow.AddDays(3),
            CarModel: "TESLA_MODEL_3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("", "First name is required")]
    [InlineData("John123", "First name can only contain letters, spaces, and hyphens")]
    [InlineData("ThisIsAReallyLongFirstNameThatExceedsFiftyCharactersLimit", "First name must not exceed 50 characters")]
    public void Validate_WithInvalidFirstName_ShouldHaveErrors(string firstName, string expectedError)
    {
        // Arrange
        var request = new ReservationRequest(
            FirstName: firstName,
            LastName: "Doe",
            Email: "john.doe@example.com",
            PhoneNumber: "+1234567890",
            StartDate: DateTime.UtcNow.AddDays(1),
            EndDate: DateTime.UtcNow.AddDays(3),
            CarModel: "TESLA_MODEL_3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage(expectedError);
    }
}