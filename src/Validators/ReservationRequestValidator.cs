using FluentValidation;
using CarRental.src.DTOs.Reservation;
using System.Text.RegularExpressions;

public class ReservationRequestValidator : AbstractValidator<ReservationRequest>
{
    private static readonly Regex PhoneRegex = new(@"^\+?[\d\s-]{8,}$", RegexOptions.Compiled);
    
    public ReservationRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters")
            .Matches("^[a-zA-Z\\s-]+$").WithMessage("First name can only contain letters, spaces, and hyphens");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .Matches("^[a-zA-Z\\s-]+$").WithMessage("Last name can only contain letters, spaces, and hyphens");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(PhoneRegex).WithMessage("Invalid phone number format");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .Must(date => date.Date >= DateTime.UtcNow.Date)
            .WithMessage("Start date must be in the future");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .Must((request, endDate) => endDate.Date >= request.StartDate.Date)
            .WithMessage("End date must be equal to or after start date");

        RuleFor(x => x.CarModel)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.PickupLocation)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.ReturnLocation)
            .NotEmpty()
            .MaximumLength(10);
    }
}